using classes.auxiliar;
using classes.formulas;
using classes.regras;
using classes.regras.binarias;
using classes.regras.binarias.closed;
using classes.regras.unitarias;
using classes.regras.unitarias.unidouble;

namespace classes.solverstage
{
    public class Stage : IDisposable
    {
        public Formulas FormulasProp { get; set; }

        // TODO: strategy
        private List<IRegraBinaria> RegrasBinarias { get; set; }
        private List<IRegraUnaria> RegrasUnarias { get; set; }
        private List<IRegraUnariaDouble> RegrasUnariasDouble { get; set; }
        private IRegraUnariaDouble RegraPBProp { get; set; }
        private RegraClosed RegraClosedProp { get; set; }

        public Stage(Formulas? formulas = null)
        {
            preencherFormulas();
            if (formulas != null) { FormulasProp = formulas; }
        }

        public void solve(Formulas? formulas = null)
        {
            if (formulas != null) { FormulasProp = formulas; }
            if (FormulasProp == null) { p("Informe o conjunto de fórmulas para o Solver"); return; }

            p(FormulasProp.ToString()); p(); p("");

            List<ConjuntoFormula> conjuntoFormulas = new();
            if (formulas.LConjuntoFormula != null) { conjuntoFormulas.AddRange(formulas.LConjuntoFormula); }

            // TODO: verificar os ramos: Direita e Esquerda

            trySolve(conjuntoFormulas, formulas);

            FormulasProp.updateFormulas(conjuntoFormulas);

            updateClosed(formulas);

            if (!formulas.isClosed)
            {
                p(); p("-- verificar a regra PB");
            } else {
                p(); p("-- Ramo fechado");
            }

            p(FormulasProp.ToString()); p(); p("");
        }

        private List<ConjuntoFormula>? trySolve(List<ConjuntoFormula> conjuntoFormulas, Formulas? formulas)
        {
            if (conjuntoFormulas == null || conjuntoFormulas.Count <= 0 || formulas == null) { return null; }
            List<ConjuntoFormula>? rt = null;

            int count = conjuntoFormulas.Count;
            for (int i = 0; i < count; i++)
            {
                ConjuntoFormula cf1 = conjuntoFormulas[i];

                // TODO: abstrair estas funções de apply
                RegrasUnarias.ForEach(ru =>
                {
                    if (!ru.isValid(cf1)) { return; }
                    ConjuntoFormula? cfAux = ru.apply(cf1);
                    if (cfAux == null || conjuntoFormulas.Contains(cfAux) || (rt != null && rt.Contains(cfAux))) { return; }
                    if (rt == null) { rt = new(); }
                    rt.Add(cfAux);
                });

                // encontrou uma contradição
                if (isClosed(conjuntoFormulas))
                {
                    formulas.isClosed = true;
                    return rt;
                }

                RegrasUnariasDouble.ForEach(rudb =>
                {
                    if (!rudb.isValid(cf1)) { return; }
                    ConjuntoFormula[]? lcfAux = rudb.apply(cf1);
                    if (lcfAux == null) { return; }
                    for (int k = 0; k < lcfAux.Length; k++) { if (conjuntoFormulas.Contains(lcfAux[k]) || (rt != null && rt.Contains(lcfAux[k]))) { return; } }

                    if (rt == null) { rt = new(); }
                    rt.AddRange(lcfAux);
                });

                // encontrou uma contradição
                if (isClosed(conjuntoFormulas))
                {
                    formulas.isClosed = true;
                    return rt;
                }

                for (int j = 0; j < count; j++)
                {
                    if (i == j) { continue; } //same
                    ConjuntoFormula cf2 = conjuntoFormulas[j];

                    // encontrou uma contradição
                    if (RegraClosedProp.apply(cf1, cf2))
                    {
                        formulas.isClosed = true;
                        return rt;
                    }

                    RegrasBinarias.ForEach(rb =>
                    {
                        if (!rb.isValid(cf1, cf2)) { return; }
                        ConjuntoFormula? cfAux = rb.apply(cf1, cf2);
                        if (cfAux == null || conjuntoFormulas.Contains(cfAux) || (rt != null && rt.Contains(cfAux))) { return; }
                        if (rt == null) { rt = new(); }
                        rt.Add(cfAux);
                    });

                    // encontrou uma contradição
                    if (RegraClosedProp.apply(cf1, cf2))
                    {
                        formulas.isClosed = true;
                        return rt;
                    }

                }

            }

            // TODO: verificar se rt!= null e chamar novamente a função trySolve
            if (rt != null && rt.Count > 0)
            {
                conjuntoFormulas.AddRange(rt);
                List<ConjuntoFormula>? laux = null;
                laux = trySolve(conjuntoFormulas, formulas);
                if (laux != null && laux.Count > 0) { rt.AddRange(laux); }
                while (laux != null && laux.Count >= 0)
                {
                    laux = trySolve(conjuntoFormulas, formulas);
                    if (laux != null && laux.Count > 0) { rt.AddRange(laux); }
                }
            }

            return rt;
        }

        #region closed
        private bool isClosed(List<ConjuntoFormula> conjuntoFormulas)
        {
            if (conjuntoFormulas == null || conjuntoFormulas.Count <= 0) { return false; }
            // procura por uma contradição
            int count = conjuntoFormulas.Count;
            for (int i = 0; i < count; i++)
            {
                ConjuntoFormula cf1 = conjuntoFormulas[i];
                for (int j = 0; j < count; j++)
                {
                    if (i == j) { continue; } //same
                    ConjuntoFormula cf2 = conjuntoFormulas[j];
                    if (RegraClosedProp.apply(cf1, cf2)) { return true; }
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

        private void preencherFormulas()
        {
            RegrasBinarias = new() {
                new RegraFalseE1(),
                new RegraFalseE2(),
                new RegraTrueImplica1(),
                new RegraTrueImplica2(),
                new RegraTrueOu1(),
                new RegraTrueOu2()
            };

            //RegraRemoverNegativos //não usar
            RegrasUnarias = new(){
                new RegraFalseNegativo(),
                new RegraTrueNegativo()
            };

            RegrasUnariasDouble = new() {
                new RegraFalseImplica(),
                new RegraFalseOu(),
                new RegraTrueE()
            };

            // regra PB fica separada
            RegraPBProp = new RegraPB();

            RegraClosedProp = new RegraClosed();
        }


        public void Dispose()
        {
            if (FormulasProp != null) { FormulasProp.Dispose(); }
            FormulasProp = null;
            if (RegrasBinarias != null) { RegrasBinarias.Clear(); }
            RegrasBinarias = null;
            if (RegrasUnarias != null) { RegrasUnarias.Clear(); }
            RegrasUnarias = null;
            if (RegrasBinarias != null) { RegrasUnariasDouble.Clear(); }
            RegrasUnariasDouble = null;
            RegraPBProp = null;
            RegraClosedProp = null;
        }

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion

    }
}