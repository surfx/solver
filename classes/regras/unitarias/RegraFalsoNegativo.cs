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
            if (cf == null || cf.Simbolo || cf.AtomoConectorProp == null || (cf.AtomoConectorProp.ConectorProp == null && cf.AtomoConectorProp.AtomoProp == null)) { return false; }

            if ((cf.AtomoConectorProp.isAtomo && !cf.AtomoConectorProp.AtomoProp.isNegado) ||
                (cf.AtomoConectorProp.isConector && !cf.AtomoConectorProp.ConectorProp.isNegado)) { return false; }

            return true;
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return cf; }

            AtomoConector ac = null;
            if (cf.AtomoConectorProp.isAtomo)
            {
                ac = new AtomoConector(cf.AtomoConectorProp.AtomoProp.copy());
                ac.AtomoProp.NumeroNegados = 0;
            }
            else if (cf.AtomoConectorProp.isConector)
            {
                ac = new AtomoConector(cf.AtomoConectorProp.ConectorProp.copy());
                ac.ConectorProp.NumeroNegados = 0;
            }

            return new ConjuntoFormula(true, ac);
        }

    }
}