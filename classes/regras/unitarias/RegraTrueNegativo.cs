using classes.formulas;

namespace classes.regras.unitarias
{
    /* 
        T ¬A
        -----   (T ¬)
        F  A
    */
    public class RegraTrueNegativo : IRegraUnaria
    {

        public string RULE { get => "T ¬"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula? cf)
        {
            return isCFValid(cf) || isValidLeft(cf);
        }

        public ConjuntoFormula? apply(ConjuntoFormula? cf)
        {
            if (!isValid(cf)) { return null; }

            AtomoConector? ac = null;
            if (cf?.AtomoConectorProp?.isAtomo ?? false)
            {
                ac = new(cf?.AtomoConectorProp?.AtomoProp?.copy());
                if (ac.AtomoProp != null) { ac.AtomoProp.NumeroNegados -= 1; }
            }
            else if (cf?.AtomoConectorProp?.isConector ?? false)
            {
                if (isCFValid(cf))
                {
                    ac = new(cf?.AtomoConectorProp?.ConectorProp?.copy());
                    if (ac.ConectorProp != null) { ac.ConectorProp.NumeroNegados -= 1; }
                }
                else
                {
                    // é a esquerda
                    AtomoConector? esquerda = cf?.AtomoConectorProp?.ConectorProp?.Esquerda?.copy();
                    if (esquerda != null && esquerda.isAtomo && esquerda.AtomoProp != null) { esquerda.AtomoProp.NumeroNegados -= 1; }
                    else if (esquerda != null && esquerda.isConector && esquerda.ConectorProp != null) { esquerda.ConectorProp.NumeroNegados -= 1; }
                    Conector? conector = cf?.AtomoConectorProp.ConectorProp?.copy();
                    if (conector != null) { conector.Esquerda = esquerda?.copy(); }

                    ac = new(conector?.copy());
                }

            }

            return new ConjuntoFormula(false, ac);
        }

        private bool isCFValid(ConjuntoFormula? cf)
        {
            // ser T; ter um conector ou um átomo
            if (cf == null || !cf.Simbolo || cf.AtomoConectorProp == null || (cf.AtomoConectorProp.ConectorProp == null && cf.AtomoConectorProp.AtomoProp == null)) { return false; }
            // ter 1 negação pelo menos
            return (cf.AtomoConectorProp.isAtomo && cf?.AtomoConectorProp?.AtomoProp?.NumeroNegados > 0) || (cf.AtomoConectorProp.isConector && cf.AtomoConectorProp?.ConectorProp?.NumeroNegados > 0);
        }

        private bool isValidLeft(ConjuntoFormula? cf)
        {
            // ser T, ser um conector
            if (cf == null || !cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            AtomoConector? ac = cf?.AtomoConectorProp.ConectorProp?.Esquerda;
            if (ac == null) { return false; }

            // ter 1 negação pelo menos
            return (ac.isAtomo && ac.AtomoProp?.NumeroNegados > 0) || (ac.isConector && ac?.ConectorProp?.NumeroNegados > 0);
        }

    }
}