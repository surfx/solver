using classes.auxiliar.formulas;
using classes.formulas;
using classes.regras;
using classes.solverstage.estrategias;
using classes.solverstage.estrategias.listaregras;

namespace classes.solverstage
{
    public class Stage : IDisposable
    {
        // TODO: strategy
        private readonly IListaRegras _iListaRegras;

        public Stage()
        {
            _iListaRegras = new ListaRegrasLCP();
        }

        public void solve(Formulas? formula = null)
        {
            if (formula == null) { p("Informe o conjunto de fórmulas para o Solver"); return; }

            p(formula.ToString()); p(); p("");

            List<ConjuntoFormula> conjuntoFormulas = formula.LConjuntoFormula ?? new();
            //if (formula.LConjuntoFormula != null) { conjuntoFormulas.AddRange(formula.LConjuntoFormula); }

            List<ConjuntoFormula>? lts = trySolve(conjuntoFormulas, formula);
            if (lts != null && lts.Count >= 0) { lts.Where(f => f != null && !formula.LConjuntoFormula.Contains(f)).ToList().ForEach(formula.LConjuntoFormula.Add); }
            lts?.Clear();

            formula.updateFormulas(conjuntoFormulas);

            updateClosed(formula);

            if (!formula.isClosed)
            {
                p(); p("-- verificar a regras beta");
                applyBeta(conjuntoFormulas, formula);
            }
            else
            {
                p(); p("-- Ramo fechado");
            }

            // não 'matar' as fórmulas aqui para não prejudicar usos futuros (ex: gerar imagem da árvore)
            //formula.Dispose(); formula = null;

            //p(FormulasProp.ToString()); p(); p("");
            p("-- end");
        }

        private List<ConjuntoFormula>? trySolve(List<ConjuntoFormula> conjuntoFormulas, Formulas? formula)
        {
            if (conjuntoFormulas == null || conjuntoFormulas.Count <= 0 || formula == null) { return null; }
            List<ConjuntoFormula>? rt = null;

            int count = conjuntoFormulas.Count;
            for (int i = 0; i < count; i++)
            {
                ConjuntoFormula cf1 = conjuntoFormulas[i];
                aplicarRegrasUnarias(cf1, ref conjuntoFormulas, ref rt);

                // encontrou uma contradição
                if (isClosed(conjuntoFormulas, ref formula)) { return rt; }

                aplicarRegrasUnariasDouble(cf1, ref conjuntoFormulas, ref rt);

                // encontrou uma contradição
                if (isClosed(conjuntoFormulas, ref formula)) { return rt; }

                for (int j = 0; j < count; j++)
                {
                    if (i == j) { continue; } //same
                    ConjuntoFormula cf2 = conjuntoFormulas[j];

                    // encontrou uma contradição
                    if (isContradicao(cf1, cf2, ref formula)) { return rt; }

                    aplicarRegrasBinarias(cf1, cf2, ref conjuntoFormulas, ref rt);

                    // encontrou uma contradição
                    if (isContradicao(cf1, cf2, ref formula)) { return rt; }
                }

            }

            // se rt != null, então regra(s) fora(m) aplicada(s)
            if (rt != null && rt.Count > 0)
            {
                conjuntoFormulas.AddRange(rt);
                List<ConjuntoFormula>? laux = trySolve(conjuntoFormulas, formula);
                if (laux != null && laux.Count > 0) { rt.AddRange(laux); }
                // enquanto regra(s) fora(m) aplicada(s)
                while (laux != null && laux.Count > 0)
                {
                    laux = trySolve(conjuntoFormulas, formula);
                    if (laux != null && laux.Count > 0) { rt.AddRange(laux); }
                }
            }

            return rt;
        }

        // regras que bifurcam o tableaux
        // formulasJaAplicadas: contém o hash das fórmulas já analisadas
        private void applyBeta(List<ConjuntoFormula> conjuntoFormulas, Formulas? formula, List<int>? formulasJaAplicadas = null)
        {
            if (formula == null || formula.isClosed) { return; }

            List<ConjuntoFormula>? lts = trySolve(conjuntoFormulas, formula);
            if (lts != null && lts.Count >= 0) { lts.Where(f => f != null && !formula.LConjuntoFormula.Contains(f)).ToList().ForEach(formula.LConjuntoFormula.Add); }
            lts?.Clear();
            updateClosed(formula);
            if (formula.isClosed) { return; }

            formulasJaAplicadas ??= new();
            conjuntoFormulas ??= new();

            formula.LConjuntoFormula?.ForEach(f =>
                {
                    if (conjuntoFormulas.Contains(f)) { return; }
                    conjuntoFormulas.Add(f);
                });

            List<ConjuntoFormula> formulasCandidatas = formulasJaAplicadas == null || formulasJaAplicadas.Count <= 0 ? conjuntoFormulas :
                conjuntoFormulas.FindAll(cf => cf != null && cf.AtomoConectorProp != null && !formulasJaAplicadas.Contains(cf.GetHashCode()));
            if (formulasCandidatas.Count <= 0) { return; }

            // TODO: escolher a regra mais promissora - rever counts átomos, conectores, etc
            StRetornoRegras? pbReturn = null;

            foreach (ConjuntoFormula fc in formulasCandidatas)
            {
                pbReturn = applyRuleBeta(fc, conjuntoFormulas);
                if (pbReturn == null) { continue; }

                // adiciona a fórmula à lista de fórmulas já aplicadas
                formulasJaAplicadas ??= new();
                formulasJaAplicadas.Add(fc.GetHashCode());
                break;

                // // verifica se alguma fórmula já está no conjunto de fórmulas base
                // if (!conjuntoFormulas.Contains(pbReturn[0]) || !conjuntoFormulas.Contains(pbReturn[1]))
                // {
                //     // add à lista de fórmulas já aplicadas
                //     if (formulasJaAplicadas == null) { formulasJaAplicadas = new(); }
                //     formulasJaAplicadas.Add(fc.GetHashCode());
                //     formulasJaAplicadas.Add(pbReturn[0].GetHashCode());
                //     formulasJaAplicadas.Add(pbReturn[1].GetHashCode());
                //     break;
                // }
                // pbReturn = null;
            }


            // não encontrou
            if (pbReturn == null) { return; }

            formula.addEsquerda(pbReturn.Value.Esquerda);
            formula.addDireita(pbReturn.Value.Direita);

            List<ConjuntoFormula> conjuntoFormulasEsquerda = new(), conjuntoFormulasDireita = new();
            conjuntoFormulas.ForEach(f => { conjuntoFormulasEsquerda.Add(f); conjuntoFormulasDireita.Add(f); });
            conjuntoFormulasEsquerda.Add(pbReturn.Value.Esquerda);
            conjuntoFormulasDireita.Add(pbReturn.Value.Direita);

            lts = trySolve(conjuntoFormulas, formula);
            if (lts != null && lts.Count >= 0) { lts.Where(f => f != null && !formula.LConjuntoFormula.Contains(f)).ToList().ForEach(formula.LConjuntoFormula.Add); }
            lts?.Clear();
            if (formula.isClosed) { return; }
            updateClosed(formula);
            if (formula.isClosed) { return; }

            applyBeta(conjuntoFormulasEsquerda, formula.Esquerda, formulasJaAplicadas);
            applyBeta(conjuntoFormulasDireita, formula.Direita, formulasJaAplicadas);
            updateClosed(formula);
        }

        #region closed
        private bool isClosed(List<ConjuntoFormula> conjuntoFormulas, ref Formulas? formula)
        {
            if (_iListaRegras.RegraClosedProp == null || conjuntoFormulas == null || conjuntoFormulas.Count <= 0 || formula == null) { return false; }
            // procura por uma contradição
            int count = conjuntoFormulas.Count;
            for (int i = 0; i < count; i++)
            {
                ConjuntoFormula cf1 = conjuntoFormulas[i];
                for (int j = 0; j < count; j++)
                {
                    if (i == j) { continue; } //same
                    ConjuntoFormula cf2 = conjuntoFormulas[j];
                    if (isContradicao(cf1, cf2, ref formula))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void updateClosed(Formulas? formulas)
        {
            if (formulas == null) { return; }
            if (formulas.Esquerda != null) { updateClosed(formulas.Esquerda); }
            if (formulas.Direita != null) { updateClosed(formulas.Direita); }
            formulas.isClosed = isClosedFormula(formulas);
        }

        private bool isClosedFormula(Formulas? formulas)
        {
            // se é null, considero fechado
            if (formulas == null) { return true; }
            // não tem nem esquerda nem direita
            if (formulas.Esquerda == null && formulas.Direita == null) { return formulas.isClosed; }

            // para o ramo se considerado fechado, esquerda e direita, também devem estar
            // se um subramo estiver aberto, o ramo atual também está

            if (formulas.Esquerda != null && formulas.Direita != null)
            {
                return isClosedFormula(formulas.Esquerda) && isClosedFormula(formulas.Direita);
            }

            return formulas.Esquerda != null ? isClosedFormula(formulas.Esquerda) : isClosedFormula(formulas.Direita);
        }

        #region contradições
        private bool isContradicao(ConjuntoFormula? cf1, ConjuntoFormula? cf2, ref Formulas? formulas)
        {
            if (cf1 == null || cf2 == null || formulas == null || _iListaRegras.RegraClosedProp == null || !_iListaRegras.RegraClosedProp.apply(cf1, cf2)) { return false; }
            p(string.Format("contradição {0} e {1}", cf1, cf2));
            formulas.isClosed = true;
            return true;
        }
        #endregion

        #endregion


        #region aplicar fórmulas
        private void aplicarRegrasUnarias(ConjuntoFormula? cf1, ref List<ConjuntoFormula> conjuntoFormulas, ref List<ConjuntoFormula>? rt)
        {
            if (cf1 == null || _iListaRegras.RegrasUnarias == null || _iListaRegras.RegrasUnarias.Count <= 0) { return; }
            foreach (IRegraUnaria ru in _iListaRegras.RegrasUnarias)
            {
                if (!ru.isValid(cf1)) { continue; }
                ConjuntoFormula? cfAux = ru.apply(cf1);
                if (cfAux == null || conjuntoFormulas.Contains(cfAux) || (rt != null && rt.Contains(cfAux))) { continue; }
                rt ??= new();
                p(string.Format("unaria: {0} ({1}): {2}", ru.RULE, cf1, cfAux));
                rt.Add(cfAux);
            }
        }

        private void aplicarRegrasUnariasDouble(ConjuntoFormula? cf1, ref List<ConjuntoFormula> conjuntoFormulas, ref List<ConjuntoFormula>? rt)
        {
            if (cf1 == null || _iListaRegras.RegrasUnariasDouble == null || _iListaRegras.RegrasUnariasDouble.Count <= 0) { return; }
            foreach (IRegraUnariaDouble rudb in _iListaRegras.RegrasUnariasDouble)
            {
                if (!rudb.isValid(cf1)) { continue; }
                StRetornoRegras? stED = rudb.apply(cf1);
                if (stED == null) { continue; }

                // se tiver ambas as fórmulas na lista de conjuntoFormulas, deve continuar
                // caso contrário, adiciona ao rt as fórmulas que não estão na lista conjuntoFormulas
                if (conjuntoFormulas.Contains(stED.Value.Esquerda) && conjuntoFormulas.Contains(stED.Value.Direita))
                {
                    continue;
                }

                rt ??= new();
                p(string.Format("unaria d: {0} ({1}): {2}, {3}", rudb.RULE, cf1, stED.Value.Esquerda, stED.Value.Direita));
                if (!conjuntoFormulas.Contains(stED.Value.Esquerda)) { rt.Add(stED.Value.Esquerda); }
                if (!conjuntoFormulas.Contains(stED.Value.Direita)) { rt.Add(stED.Value.Direita); }
                //rt.AddRange(lcfAux);
            }
        }

        private void aplicarRegrasBinarias(ConjuntoFormula? cf1, ConjuntoFormula? cf2, ref List<ConjuntoFormula> conjuntoFormulas, ref List<ConjuntoFormula>? rt)
        {
            if (cf1 == null || cf2 == null || _iListaRegras.RegrasBinarias == null || _iListaRegras.RegrasBinarias.Count <= 0) { return; }
            foreach (IRegraBinaria rb in _iListaRegras.RegrasBinarias)
            {
                if (!rb.isValid(cf1, cf2)) { continue; }
                ConjuntoFormula? cfAux = rb.apply(cf1, cf2);
                if (cfAux == null || conjuntoFormulas.Contains(cfAux) || (rt != null && rt.Contains(cfAux))) { continue; }
                rt ??= new();
                p(string.Format("binaria: {0} ({1}, {2}): {3}", rb.RULE, cf1, cf2, cfAux));
                rt.Add(cfAux);
            }
        }

        private StRetornoRegras? applyRuleBeta(ConjuntoFormula fc, List<ConjuntoFormula> conjuntoFormulas)
        {
            if (fc == null) { return null; }
            IRegraUnariaDouble? regraBeta = _iListaRegras.RegrasBeta?.FindAll(rb => rb != null && fc != null && rb.isValid(fc)).FirstOrDefault();
            if (regraBeta == null) { return null; }
            StRetornoRegras? rt = regraBeta.apply(fc);
            if (rt == null) { return null; }

            // se alguma regra já existe no conjunto de conjuntoFormulas, retorna null, pois não há variação do conjunto de fórmulas base
            if (conjuntoFormulas != null && (conjuntoFormulas.Contains(rt.Value.Esquerda) || conjuntoFormulas.Contains(rt.Value.Direita))) { return null; }

            p(string.Format("beta: {0} ({1}): {2}", regraBeta.RULE, fc, rt.Value.ToString()));
            return rt;
        }
        #endregion


        public void Dispose()
        {
            //if (FormulasProp != null) { FormulasProp.Dispose(); } FormulasProp = null;
            _iListaRegras?.Dispose();
        }

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion

    }
}