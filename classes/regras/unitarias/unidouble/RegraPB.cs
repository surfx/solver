using classes.formulas;

namespace classes.regras.unitarias.unidouble
{
    /* 
             A
        ---------- (PB)
        T A     FA
    */
    public class RegraPB : IRegraUnariaDouble
    {

        public string RULE { get => "PB"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            return cf != null &&
                cf.AtomoConectorProp != null &&
                ((cf.AtomoConectorProp.isAtomo && cf.AtomoConectorProp.AtomoProp != null) ||
                (cf.AtomoConectorProp.isConector && cf.AtomoConectorProp.ConectorProp != null));
        }

        public ConjuntoFormula[]? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            ConjuntoFormula cfEsquerda = new ConjuntoFormula(true, cf.AtomoConectorProp);
            ConjuntoFormula cfDireita = new ConjuntoFormula(false, cf.AtomoConectorProp);
            return new[] { cfEsquerda, cfDireita };
        }

    }
}