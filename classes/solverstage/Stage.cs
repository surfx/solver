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

            trySolve(conjuntoFormulas, formula);

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

                // TODO: abstrair estas funções de apply
                _iListaRegras.RegrasUnarias?.ForEach(ru =>
                {
                    if (!ru.isValid(cf1)) { return; }
                    ConjuntoFormula? cfAux = ru.apply(cf1);
                    if (cfAux == null || conjuntoFormulas.Contains(cfAux) || (rt != null && rt.Contains(cfAux))) { return; }
                    if (rt == null) { rt = new(); }
                    p(string.Format("unaria: {0} ({1}): {2}", ru.RULE, cf1, cfAux));
                    rt.Add(cfAux);
                });

                // encontrou uma contradição
                if (isClosed(conjuntoFormulas))
                {
                    formula.isClosed = true;
                    return rt;
                }

                _iListaRegras.RegrasUnariasDouble?.ForEach(rudb =>
                {
                    if (!rudb.isValid(cf1)) { return; }
                    ConjuntoFormula[]? lcfAux = rudb.apply(cf1);
                    if (lcfAux == null) { return; }
                    for (int k = 0; k < lcfAux.Length; k++) { if (conjuntoFormulas.Contains(lcfAux[k]) || (rt != null && rt.Contains(lcfAux[k]))) { return; } }

                    rt ??= new();
                    p(string.Format("unaria d: {0} ({1}): {2}, {3}", rudb.RULE, cf1, lcfAux[0], lcfAux[1]));
                    rt.AddRange(lcfAux);
                });

                // encontrou uma contradição
                if (isClosed(conjuntoFormulas))
                {
                    formula.isClosed = true;
                    return rt;
                }

                for (int j = 0; j < count; j++)
                {
                    if (i == j) { continue; } //same
                    ConjuntoFormula cf2 = conjuntoFormulas[j];

                    // encontrou uma contradição
                    if (_iListaRegras.RegraClosedProp != null && _iListaRegras.RegraClosedProp.apply(cf1, cf2))
                    {
                        p(string.Format("contradição {0} e {1}", cf1, cf2));
                        formula.isClosed = true;
                        return rt;
                    }

                    _iListaRegras.RegrasBinarias?.ForEach(rb =>
                    {
                        if (!rb.isValid(cf1, cf2)) { return; }
                        ConjuntoFormula? cfAux = rb.apply(cf1, cf2);
                        if (cfAux == null || conjuntoFormulas.Contains(cfAux) || (rt != null && rt.Contains(cfAux))) { return; }
                        rt ??= new();
                        p(string.Format("binaria: {0} ({1}, {2}): {3}", rb.RULE, cf1, cf2, cfAux));
                        rt.Add(cfAux);
                    });

                    // encontrou uma contradição
                    if (_iListaRegras.RegraClosedProp != null && _iListaRegras.RegraClosedProp.apply(cf1, cf2))
                    {
                        p(string.Format("contradição {0} e {1}", cf1, cf2));
                        formula.isClosed = true;
                        return rt;
                    }

                }

            }

            // se rt != null, então regra(s) fora(m) aplicada(s)
            if (rt != null && rt.Count > 0)
            {
                conjuntoFormulas.AddRange(rt);
                List<ConjuntoFormula>? laux = null;
                laux = trySolve(conjuntoFormulas, formula);
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
            trySolve(conjuntoFormulas, formula);
            updateClosed(formula);
            if (formula.isClosed) { return; }

            formulasJaAplicadas ??= new();
            conjuntoFormulas ??= new();

            formula.LConjuntoFormula?.ForEach(f =>
                {
                    if (conjuntoFormulas.Contains(f)) { return; }
                    conjuntoFormulas.Add(f);
                });


            List<ConjuntoFormula> formulasCandidatas = formulasJaAplicadas == null ? conjuntoFormulas :
                conjuntoFormulas.FindAll(cf => cf != null && cf.AtomoConectorProp != null && !formulasJaAplicadas.Contains(cf.GetHashCode()));
            if (formulasCandidatas.Count <= 0) { return; }

            // TODO: escolher a regra mais promissora - rever counts átomos, conectores, etc
            ConjuntoFormula[]? pbReturn = null;

            Func<ConjuntoFormula, ConjuntoFormula[]?> applyRuleBeta = fc =>
            {
                if (fc == null) { return null; }
                IRegraUnariaDouble? regraBeta = _iListaRegras.RegrasBeta?.FindAll(rb => rb != null && fc != null && rb.isValid(fc)).FirstOrDefault();
                if (regraBeta == null) { return null; }
                ConjuntoFormula[]? rt = regraBeta.apply(fc);
                if (rt == null) { return null; }
                p(string.Format("beta: {0} ({1}): {2}, {3}", regraBeta.RULE, fc, rt[0], rt[1]));
                return rt;
            };

            foreach (ConjuntoFormula fc in formulasCandidatas)
            {
                pbReturn = applyRuleBeta(fc);
                if (pbReturn == null || pbReturn.Length != 2) { continue; }

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

            formula.addEsquerda(pbReturn[0]);
            formula.addDireita(pbReturn[1]);

            List<ConjuntoFormula> conjuntoFormulasEsquerda = new(), conjuntoFormulasDireita = new();
            conjuntoFormulas.ForEach(f => { conjuntoFormulasEsquerda.Add(f); conjuntoFormulasDireita.Add(f); });
            conjuntoFormulasEsquerda.Add(pbReturn[0]);
            conjuntoFormulasDireita.Add(pbReturn[1]);

            trySolve(conjuntoFormulas, formula);
            if (formula.isClosed) { return; }
            updateClosed(formula);
            if (formula.isClosed) { return; }

            applyBeta(conjuntoFormulasEsquerda, formula.Esquerda, formulasJaAplicadas);
            applyBeta(conjuntoFormulasDireita, formula.Direita, formulasJaAplicadas);
        }

        #region closed
        private bool isClosed(List<ConjuntoFormula> conjuntoFormulas)
        {
            if (_iListaRegras.RegraClosedProp == null || conjuntoFormulas == null || conjuntoFormulas.Count <= 0) { return false; }
            // procura por uma contradição
            int count = conjuntoFormulas.Count;
            for (int i = 0; i < count; i++)
            {
                ConjuntoFormula cf1 = conjuntoFormulas[i];
                for (int j = 0; j < count; j++)
                {
                    if (i == j) { continue; } //same
                    ConjuntoFormula cf2 = conjuntoFormulas[j];
                    if (_iListaRegras.RegraClosedProp.apply(cf1, cf2))
                    {
                        p(string.Format("contradição {0} e {1}", cf1, cf2));
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

            // if (formulas.Esquerda != null) { formulas.Esquerda.isClosed = isClosedFormula(formulas.Esquerda.Esquerda) && isClosedFormula(formulas.Esquerda.Direita); }
            // if (formulas.Direita != null) { formulas.Direita.isClosed = isClosedFormula(formulas.Direita.Esquerda) && isClosedFormula(formulas.Direita.Direita); }
            // return isClosedFormula(formulas.Esquerda) && isClosedFormula(formulas.Direita);
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