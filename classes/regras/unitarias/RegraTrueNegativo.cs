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
        public bool isValid(ConjuntoFormula cf)
        {
            return isCFValid(cf) || isValidLeft(cf);
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return cf; }

            AtomoConector ac = null;
            if (cf.AtomoConectorProp.isAtomo)
            {
                ac = new AtomoConector(cf.AtomoConectorProp.AtomoProp.copy());
                ac.AtomoProp.NumeroNegados -= 1;
            }
            else if (cf.AtomoConectorProp.isConector)
            {
                if (isCFValid(cf))
                {
                    ac = new AtomoConector(cf.AtomoConectorProp.ConectorProp.copy());
                    ac.ConectorProp.NumeroNegados -= 1;
                }
                else
                {
                    // é a esquerda
                    AtomoConector? esquerda = cf.AtomoConectorProp.ConectorProp.Esquerda;
                    if (esquerda.isAtomo) { esquerda.AtomoProp.NumeroNegados -= 1; }
                    else if (esquerda.isConector) { esquerda.ConectorProp.NumeroNegados -= 1; }
                    cf.AtomoConectorProp.ConectorProp.Esquerda = esquerda;

                    ac = new AtomoConector(cf.AtomoConectorProp.ConectorProp.copy());
                }

            }

            return new ConjuntoFormula(false, ac);
        }

        private bool isCFValid(ConjuntoFormula cf)
        {
            if (cf == null || !cf.Simbolo || cf.AtomoConectorProp == null || (cf.AtomoConectorProp.ConectorProp == null && cf.AtomoConectorProp.AtomoProp == null)) { return false; }

            if (cf.AtomoConectorProp.isAtomo && cf.AtomoConectorProp.AtomoProp.NumeroNegados > 0) { return true; }
            if (cf.AtomoConectorProp.isConector && cf.AtomoConectorProp.ConectorProp.NumeroNegados > 0) { return true; }

            return false;
        }

        private bool isValidLeft(ConjuntoFormula cf)
        {
            if (!cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            AtomoConector? ac = cf.AtomoConectorProp.ConectorProp.Esquerda;
            if (ac == null) { return false; }
            if (ac.isAtomo && ac.AtomoProp.NumeroNegados <= 0) { return false; }
            if (ac.isConector && ac.ConectorProp.NumeroNegados > 0) { return true; }

            return false;
        }

    }
}