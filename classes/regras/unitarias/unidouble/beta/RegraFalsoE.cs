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

        public ConjuntoFormula[]? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            ConjuntoFormula cfEsquerda = new ConjuntoFormula(false, cf.AtomoConectorProp.ConectorProp.Esquerda);
            ConjuntoFormula cfDireita = new ConjuntoFormula(false, cf.AtomoConectorProp.ConectorProp.Direita);
            return new[] { cfEsquerda, cfDireita };
        }

    }
}