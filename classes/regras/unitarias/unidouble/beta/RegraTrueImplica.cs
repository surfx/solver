using classes.formulas;

namespace classes.regras.unitarias.unidouble.beta
{
    /* 
           T A → B
        ------------ (T →)
        F A      T B
    */
    public class RegraTrueImplica : IRegraUnariaDouble
    {

        public string RULE { get => "T →"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            // ser T, ser conector →
            if (cf == null || !cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.isAtomo || !cf.AtomoConectorProp.isConector || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            // ter o → (Implica) como conector
            return cf.AtomoConectorProp.ConectorProp.Simbolo == ESimbolo.IMPLICA;

        }

        public StRetornoRegras? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            ConjuntoFormula cfEsquerda = new(false, cf.AtomoConectorProp?.ConectorProp?.Esquerda?.copy());
            ConjuntoFormula cfDireita = new(true, cf.AtomoConectorProp?.ConectorProp?.Direita?.copy());
            return new(cfEsquerda, cfDireita);
        }

    }
}