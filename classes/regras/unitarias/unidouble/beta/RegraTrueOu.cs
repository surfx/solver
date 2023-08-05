using classes.formulas;

namespace classes.regras.unitarias.unidouble.beta
{
    /* 
           T A ˅ B
        ------------ (T ˅)
        T A      T B
    */
    public class RegraTrueOu : IRegraUnariaDouble
    {

        public string RULE { get => "T ˅"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            // ser T, ser conector ˅
            if (cf == null || !cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.isAtomo || !cf.AtomoConectorProp.isConector || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            // ter o ˅ (OU) como conector
            return cf.AtomoConectorProp.ConectorProp.Simbolo == ESimbolo.OU;

        }

        public StRetornoRegras? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            ConjuntoFormula cfEsquerda = new(true, cf.AtomoConectorProp?.ConectorProp?.Esquerda?.copy());
            ConjuntoFormula cfDireita = new(true, cf.AtomoConectorProp?.ConectorProp?.Direita?.copy());
            return new(cfEsquerda, cfDireita);
        }

    }
}