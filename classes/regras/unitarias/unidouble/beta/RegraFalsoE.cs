using classes.formulas;

namespace classes.regras.unitarias.unidouble.beta
{
    /* 
           F A ˄ B
        ------------ (F ˄)
        F A      F B
    */
    public class RegraFalsoE : IRegraUnariaDouble
    {

        public string RULE { get => "F ˄"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            // ser F, ser conector ˄
            if (cf == null || cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.isAtomo || !cf.AtomoConectorProp.isConector || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            // ter o ˄ (E) como conector
            return cf.AtomoConectorProp.ConectorProp.Simbolo == ESimbolo.E;

        }

        public StRetornoRegras? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            ConjuntoFormula cfEsquerda = new(false, cf.AtomoConectorProp?.ConectorProp?.Esquerda?.copy());
            ConjuntoFormula cfDireita = new(false, cf.AtomoConectorProp?.ConectorProp?.Direita?.copy());
            return new(cfEsquerda, cfDireita);
        }

    }
}