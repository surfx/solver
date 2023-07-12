using classes.formulas;

namespace classes.regras.unitarias.unidouble
{
    /* 
        T A ˄ B
        ------- (T ˄)
        T A
        T B

        obs: A ou B podem ser conectores (!)
    */
    public class RegraTrueE : IRegraUnariaDouble
    {

        public string RULE { get => "T ˄"; }

        // se a regra se aplica a este objeto cf
        public bool isValid(ConjuntoFormula cf)
        {
            // ser T, ser conector
            if (cf == null || !cf.Simbolo || cf.AtomoConectorProp == null || cf.AtomoConectorProp.isAtomo || !cf.AtomoConectorProp.isConector || cf.AtomoConectorProp.ConectorProp == null) { return false; }
            // conector: ˄ - E
            return cf.AtomoConectorProp.ConectorProp.Simbolo == ESimbolo.E;
        }

        public ConjuntoFormula[]? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return null; }
            Conector? conector = cf == null || cf.AtomoConectorProp == null ? null : cf.AtomoConectorProp.ConectorProp;
            if (conector == null) { return null; }

            ConjuntoFormula cfEsquerda = new ConjuntoFormula(true, conector.Esquerda);
            ConjuntoFormula cfDireita = new ConjuntoFormula(true, conector.Direita);

            return new[] { cfEsquerda, cfDireita };
        }

    }
}