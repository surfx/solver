using classes.formulas;

namespace classes.regras.unitarias.unidouble
{
    /* 
        F A → B
        ------- (F →)
        T A
        F B

        obs: A ou B podem ser conectores (!)
    */
    public class RegraFalseImplica : IRegraUnariaDouble
    {

        public string RULE { get => "F →"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            // ser F, ser conector
            if (cf == null || cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.isAtomo || !cf.AtomoConectorProp.isConector || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            // ter o implica como conector
            return cf.AtomoConectorProp.ConectorProp.Simbolo == ESimbolo.IMPLICA;
        }

        public StRetornoRegras? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            Conector? conector = cf == null || cf.AtomoConectorProp == null ? null : cf.AtomoConectorProp.ConectorProp;
            if (conector == null) { return null; }

            ConjuntoFormula cfEsquerda = new(true, conector.Esquerda?.copy());
            ConjuntoFormula cfDireita = new(false, conector.Direita?.copy());

            return new(cfEsquerda, cfDireita);
        }

    }
}