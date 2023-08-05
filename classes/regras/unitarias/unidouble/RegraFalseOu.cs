using classes.formulas;

namespace classes.regras.unitarias.unidouble
{
    /* 
        F A ˅ B
        ------- (F ˅)
        F A
        F B

        obs: A ou B podem ser conectores (!)
    */
    public class RegraFalseOu : IRegraUnariaDouble
    {

        public string RULE { get => "F ˅"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            // ser F, ser conector
            if (cf == null || cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.isAtomo || !cf.AtomoConectorProp.isConector || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            // conector: ˅ - OU
            return cf.AtomoConectorProp.ConectorProp.Simbolo == ESimbolo.OU;
        }

        public StRetornoRegras? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            Conector? conector = cf == null || cf.AtomoConectorProp == null ? null : cf.AtomoConectorProp.ConectorProp;
            if (conector == null || conector.Esquerda == null || conector.Direita == null) { return null; }

            ConjuntoFormula cfEsquerda = new(false, conector.Esquerda.copy());
            ConjuntoFormula cfDireita = new(false, conector.Direita.copy());

            return new(cfEsquerda, cfDireita);
        }

    }
}