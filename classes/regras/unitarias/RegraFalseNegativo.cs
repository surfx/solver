using classes.formulas;

namespace classes.regras.unitarias
{
    /* 
        F ¬A
        -----   (F ¬)
        T  A
    */
    public class RegraFalsoNegativo : IRegraUnaria
    {

        public string RULE { get => "F ¬"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            return isCFValid(cf) || isValidLeft(cf);
        }


        public ConjuntoFormula? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }

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

            return new ConjuntoFormula(true, ac);
        }

        private bool isCFValid(ConjuntoFormula cf)
        {
            // ser F; ter um conector ou um átomo
            if (cf == null || cf.Simbolo || cf.AtomoConectorProp == null || (cf.AtomoConectorProp.ConectorProp == null && cf.AtomoConectorProp.AtomoProp == null)) { return false; }
            // ter 1 negação pelo menos
            return (cf.AtomoConectorProp.isAtomo && cf.AtomoConectorProp.AtomoProp.NumeroNegados > 0) || (cf.AtomoConectorProp.isConector && cf.AtomoConectorProp.ConectorProp.NumeroNegados > 0);
        }

        private bool isValidLeft(ConjuntoFormula cf)
        {
            // ser F, ser um conector
            if (cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            AtomoConector? ac = cf.AtomoConectorProp.ConectorProp.Esquerda;
            if (ac == null) { return false; }

            // ter 1 negação pelo menos
            return (ac.isAtomo && ac.AtomoProp.NumeroNegados > 0) || (ac.isConector && ac.ConectorProp.NumeroNegados > 0);
        }

    }
}