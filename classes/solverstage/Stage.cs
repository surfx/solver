using classes.auxiliar.diagnosticos;
using classes.auxiliar.valoracoes;
using classes.formulas;
using classes.regras;
using classes.solverstage.auxiliar;
using classes.solverstage.estrategias;
using classes.solverstage.estrategias.listaregras;
using classes.solverstage.estrategias.selecao;
using classes.solverstage.parameters;
using static classes.auxiliar.formulas.UtilFormulas;

namespace classes.solverstage
{
    public class Stage : IDisposable
    {
        // TODO: strategy
        private readonly IListaRegras _iListaRegras;

        // armazena apenas o hashcode das fórmulas que apresentam contradição
        private readonly HashSet<Contradicoes<int>> _contradicoesHash;
        private readonly HashSet<ApplyRegraUnaria<int>> _regrasUnarias;
        private readonly HashSet<ApplyRegraBinaria<int>> _regrasBinarias;
        private readonly HashSet<ApplyRegraUnariaDouble<int>> _regrasRegraUnariaDouble;
        private readonly HashSet<ApplyRegraUnariaDouble<int>> _regrasBeta;

        public Stage()
        {
            _iListaRegras = new ListaRegrasLCP();
            _contradicoesHash = new();
            _regrasUnarias = new();
            _regrasBinarias = new();
            _regrasRegraUnariaDouble = new();
            _regrasBeta = new();
        }

        public DadosSolver? solve(SolverParameters solverParameters)
        {
            if (solverParameters.Formulas == null) { p("Informe o conjunto de fórmulas para o Solver"); return null; }

            DadosSolver rt = new()
            {
                DadosConsumo = new DiagnosticosMemoriaTempo().MesurarConsumo(() =>
                {
                    p(solverParameters.Formulas.ToString()); p(); p("");

                    List<ConjuntoFormula> conjuntoFormulas = solverParameters.Formulas.LConjuntoFormula ?? new();
                    //if (formula.LConjuntoFormula != null) { conjuntoFormulas.AddRange(formula.LConjuntoFormula); }

                    List<ConjuntoFormula>? lts = trySolve(conjuntoFormulas, solverParameters.Formulas);
                    if (lts != null && lts.Count >= 0)
                    {
                        lts.Where(f => f != null && solverParameters.Formulas.LConjuntoFormula != null && !solverParameters.Formulas.LConjuntoFormula.Contains(f))
                            .ToList()
                            .ForEach(solverParameters.Formulas.addConjuntoFormula);
                    }
                    lts?.Clear();

                    solverParameters.Formulas.updateFormulas(conjuntoFormulas);

                    updateClosed(solverParameters.Formulas);

                    if (!solverParameters.Formulas.isClosed)
                    {
                        p(); p("-- verificar a regras beta");
                        applyBeta(conjuntoFormulas, solverParameters.Formulas);
                    }
                    else
                    {
                        p(); p("-- Ramo fechado");
                    }

                    // não 'matar' as fórmulas aqui para não prejudicar usos futuros (ex: gerar imagem da árvore)
                    //formula.Dispose(); formula = null;

                    //p(FormulasProp.ToString()); p(); p("");
                    p("-- end");
                }),
                isClosed = solverParameters.Formulas.isClosed,
                Complexidade = Valoracoes.complexidade(solverParameters.Formulas),
                Bifurcacoes = Valoracoes.bifurcacoes(solverParameters.Formulas),
                NumeroAtomosLivres = Valoracoes.numeroAtomosLivres(solverParameters.Formulas),
                NumeroFormulas = Valoracoes.numeroFormulas(solverParameters.Formulas),
                RamosAbertosFechados = Valoracoes.ramosAbertosEFechados(solverParameters.Formulas),
                Alturas = Valoracoes.altura(solverParameters.Formulas),
                NumeroConectores = Valoracoes.numeroConectores(solverParameters.Formulas),
            };

            solverParameters.Formulas.updateNumeroFormulas();

            #region retornos
            rt.Contradicoes = _contradicoesHash?.Select(
                        ch => new Contradicoes<ConjuntoFormula?>(
                            findConjuntoFormula(solverParameters.Formulas, ch.Formula1),
                            findConjuntoFormula(solverParameters.Formulas, ch.Formula2)
                        )
                    ).ToList();
            rt.ApplyRegraUnarias = _regrasUnarias?.Select(
                        ch => new ApplyRegraUnaria<ConjuntoFormula?>(
                            ch.RULE,
                            findConjuntoFormula(solverParameters.Formulas, ch.InputFormula),
                            findConjuntoFormula(solverParameters.Formulas, ch.OutputFormula)
                        )
                    ).ToList();
            rt.ApplyRegraBinarias = _regrasBinarias?.Select(
                        ch => new ApplyRegraBinaria<ConjuntoFormula?>(
                            ch.RULE,
                            findConjuntoFormula(solverParameters.Formulas, ch.InputFormula1),
                            findConjuntoFormula(solverParameters.Formulas, ch.InputFormula2),
                            findConjuntoFormula(solverParameters.Formulas, ch.OutputFormula)
                        )
                    ).ToList();
            rt.ApplyRegraUnariaDoubleProp = _regrasRegraUnariaDouble?.Select(
                        ch => new ApplyRegraUnariaDouble<ConjuntoFormula?>(
                            ch.RULE,
                            findConjuntoFormula(solverParameters.Formulas, ch.InputFormula),
                            findConjuntoFormula(solverParameters.Formulas, ch.OutputFormula1),
                            findConjuntoFormula(solverParameters.Formulas, ch.OutputFormula2)
                        )
                    ).ToList();
            rt.ApplyRegraBeta = _regrasBeta?.Select(
                        ch => new ApplyRegraUnariaDouble<ConjuntoFormula?>(
                            ch.RULE,
                            findConjuntoFormula(solverParameters.Formulas, ch.InputFormula),
                            findConjuntoFormula(solverParameters.Formulas, ch.OutputFormula1),
                            findConjuntoFormula(solverParameters.Formulas, ch.OutputFormula2)
                        )
                    ).ToList();
            #endregion

            return rt;
        }




        private List<ConjuntoFormula>? trySolve(List<ConjuntoFormula> conjuntoFormulas, Formulas? formula)
        {
            if (conjuntoFormulas == null || conjuntoFormulas.Count <= 0 || formula == null) { return null; }
            List<ConjuntoFormula>? rt = null;

            SelecaoFormulas.ordernarMaiorTaxa(formula);

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
                // enquanto regra(s) for(am) aplicada(s)
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
            if (lts != null && lts.Count >= 0)
            {
                lts.Where(
                    f => f != null &&
                    formula.LConjuntoFormula != null &&
                    !formula.LConjuntoFormula.Contains(f)
                )
                .ToList()
                .ForEach(formula.addConjuntoFormula);
            }
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

            if (formulasJaAplicadas != null)
            {
                SelecaoFormulas.ordernarMaiorTaxa(formulasCandidatas, formulasJaAplicadas?.Select(fint => findConjuntoFormula(formula, fint)).ToList());
            }
            StRetornoRegras? pbReturn = null;

            foreach (ConjuntoFormula fc in formulasCandidatas)
            {
                pbReturn = applyRuleBeta(fc, conjuntoFormulas);
                if (pbReturn == null) { continue; }

                // adiciona a fórmula à lista de fórmulas já aplicadas
                formulasJaAplicadas ??= new();
                formulasJaAplicadas.Add(fc.GetHashCode());
                break;
            }

            // não encontrou
            if (pbReturn == null || pbReturn.Value.Esquerda == null || pbReturn.Value.Direita == null) { return; }

            formula.addEsquerda(pbReturn.Value.Esquerda);
            formula.addDireita(pbReturn.Value.Direita);

            List<ConjuntoFormula> conjuntoFormulasEsquerda = new(), conjuntoFormulasDireita = new();
            conjuntoFormulas.ForEach(f => { conjuntoFormulasEsquerda.Add(f); conjuntoFormulasDireita.Add(f); });
            conjuntoFormulasEsquerda.Add(pbReturn.Value.Esquerda);
            conjuntoFormulasDireita.Add(pbReturn.Value.Direita);

            lts = trySolve(conjuntoFormulas, formula);
            if (lts != null && lts.Count >= 0)
            {
                lts.Where(
                    f => f != null &&
                    formula.LConjuntoFormula != null &&
                    !formula.LConjuntoFormula.Contains(f)
                )
                .ToList()
                .ForEach(formula.addConjuntoFormula);
            }
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
            _contradicoesHash.Add(new(cf1.GetHashCode(), cf2.GetHashCode()));
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
                cfAux = cfAux == null || !conjuntoFormulas.Contains(cfAux) ? cfAux : conjuntoFormulas.Where(f => f.Equals(cfAux)).FirstOrDefault(); // faz o replace por conta do hashcode
                p(string.Format("unaria: {0} ({1}): {2}", ru.RULE, cf1, cfAux));
                _regrasUnarias.Add(new(ru.RULE, cf1.GetHashCode(), cfAux == null ? -1 : cfAux.GetHashCode()));
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
                if (stED == null || stED.Value.Esquerda == null || stED.Value.Direita == null) { continue; }

                ConjuntoFormula? esquerda = stED.Value.Esquerda;
                ConjuntoFormula? direita = stED.Value.Direita;
                esquerda = esquerda == null || !conjuntoFormulas.Contains(esquerda) ? esquerda : conjuntoFormulas.Where(f => f.Equals(esquerda)).FirstOrDefault(); // faz o replace por conta do hashcode
                direita = direita == null || !conjuntoFormulas.Contains(direita) ? direita : conjuntoFormulas.Where(f => f.Equals(direita)).FirstOrDefault();

                // se tiver ambas as fórmulas na lista de conjuntoFormulas, deve continuar
                // caso contrário, adiciona ao rt as fórmulas que não estão na lista conjuntoFormulas
                if ((esquerda == null || conjuntoFormulas.Contains(esquerda)) && (direita == null || conjuntoFormulas.Contains(direita)))
                {
                    continue;
                }

                p(string.Format("unaria d: {0} ({1}): {2}, {3}", rudb.RULE, cf1, esquerda, direita));
                rt ??= new();
                _regrasRegraUnariaDouble.Add(new(rudb.RULE, cf1.GetHashCode(), esquerda == null ? -1 : esquerda.GetHashCode(), direita == null ? -1 : direita.GetHashCode()));
                if (esquerda != null && !conjuntoFormulas.Contains(esquerda)) { rt.Add(esquerda); }
                if (direita != null && !conjuntoFormulas.Contains(direita)) { rt.Add(direita); }
            }
        }

        private void aplicarRegrasBinarias(ConjuntoFormula? cf1, ConjuntoFormula? cf2, ref List<ConjuntoFormula> conjuntoFormulas, ref List<ConjuntoFormula>? rt)
        {
            if (cf1 == null || cf2 == null || _iListaRegras.RegrasBinarias == null || _iListaRegras.RegrasBinarias.Count <= 0) { return; }
            foreach (IRegraBinaria rb in _iListaRegras.RegrasBinarias)
            {
                if (!rb.isValid(cf1, cf2)) { continue; }
                ConjuntoFormula? cfAux = rb.apply(cf1, cf2);
                cfAux = cfAux == null || !conjuntoFormulas.Contains(cfAux) ? cfAux : conjuntoFormulas.Where(f => f.Equals(cfAux)).FirstOrDefault(); // faz o replace por conta do hashcode
                if (cfAux == null || conjuntoFormulas.Contains(cfAux) || (rt != null && rt.Contains(cfAux))) { continue; }
                rt ??= new();
                p(string.Format("binaria: {0} ({1}, {2}): {3}", rb.RULE, cf1, cf2, cfAux));
                _regrasBinarias.Add(new(rb.RULE, cf1.GetHashCode(), cf2.GetHashCode(), cfAux.GetHashCode()));
                rt.Add(cfAux);
            }
        }

        private StRetornoRegras? applyRuleBeta(ConjuntoFormula fc, List<ConjuntoFormula> conjuntoFormulas)
        {
            if (fc == null) { return null; }
            IRegraUnariaDouble? regraBeta = _iListaRegras.RegrasBeta?.FindAll(rb => rb != null && fc != null && rb.isValid(fc)).FirstOrDefault();
            if (regraBeta == null) { return null; }
            StRetornoRegras? rt = regraBeta.apply(fc);
            ConjuntoFormula? esquerda = rt?.Esquerda;
            ConjuntoFormula? direita = rt?.Direita;
            if (rt == null || esquerda == null || direita == null) { return null; }

            // se alguma regra já existe no conjunto de conjuntoFormulas, retorna null, pois não há variação do conjunto de fórmulas base
            if (conjuntoFormulas != null && (conjuntoFormulas.Contains(esquerda) || conjuntoFormulas.Contains(direita))) { return null; }
            // não precisa do replace (hashcode) porque nem a esquerda e direita estão no conjuntoFormulas
            // esquerda = esquerda == null || !conjuntoFormulas.Contains(esquerda) ? esquerda : conjuntoFormulas.Where(f => f.Equals(esquerda)).FirstOrDefault(); // faz o replace por conta do hashcode
            // direita = direita == null || !conjuntoFormulas.Contains(direita) ? direita : conjuntoFormulas.Where(f => f.Equals(direita)).FirstOrDefault();

            p(string.Format("beta: {0} ({1}): {2}", regraBeta.RULE, fc, rt.Value.ToString()));
            _regrasBeta.Add(new(regraBeta.RULE, fc.GetHashCode(), esquerda.GetHashCode(), direita.GetHashCode()));
            return rt;
        }
        #endregion


        public void Dispose()
        {
            //if (FormulasProp != null) { FormulasProp.Dispose(); } FormulasProp = null;
            _iListaRegras?.Dispose();
            _contradicoesHash?.Clear();
            _regrasUnarias?.Clear();
            _regrasBinarias?.Clear();
            _regrasRegraUnariaDouble?.Clear();
            _regrasBeta?.Clear();
        }

    }
}