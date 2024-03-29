using classes.formulas;

namespace classes.regras.unitarias.unidouble.beta
{
    /* 
        Obs: Não aplicar, mas sim as regras RegraFalsoE, RegraTrueOu e RegraTrueImplica

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

        public StRetornoRegras? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            ConjuntoFormula cfEsquerda = new(true, cf.AtomoConectorProp?.copy());
            ConjuntoFormula cfDireita = new(false, cf.AtomoConectorProp?.copy());
            return new(cfEsquerda, cfDireita);
        }

    }
}