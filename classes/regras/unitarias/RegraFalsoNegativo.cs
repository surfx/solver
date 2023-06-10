using clases.formulas;

namespace clases.regras.unitarias
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

            if ((cf.AtomoConectorProp.isAtomo && !cf.AtomoConectorProp.AtomoProp.Negado) ||
                (cf.AtomoConectorProp.isConector && !cf.AtomoConectorProp.ConectorProp.Negado)) { return false; }

            return true;
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return cf; }

            AtomoConector ac = null;
            if (cf.AtomoConectorProp.isAtomo) {
                ac = new AtomoConector(cf.AtomoConectorProp.AtomoProp.copy());
                ac.AtomoProp.Negado = false;
            } else if (cf.AtomoConectorProp.isConector){
                ac = new AtomoConector(cf.AtomoConectorProp.ConectorProp.copy());
                ac.ConectorProp.Negado = false;
            }

            return new ConjuntoFormula(true, ac);
        }

    }
}