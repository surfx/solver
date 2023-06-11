using classes.formulas;

namespace classes.regras.binarias
{

    /*
        T A → B
        T A
        ------- (T →1)
        T B

        obs: A ou B podem ser conectores (!)
    */
    public class RegraTrueImplica : IRegraBinaria
    {
        public string RULE { get => "T →1"; }

        // se a regra se aplica a estes objetos cf
        public bool isValid(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (cf1 == null || cf2 == null || cf1.AtomoConectorProp == null || cf2.AtomoConectorProp == null) { return false; }
            if (!cf1.Simbolo || !cf2.Simbolo) { return false; }

            // conector ou atomo
            AtomoConector ac1 = cf1.AtomoConectorProp;
            AtomoConector ac2 = cf2.AtomoConectorProp;

            // 1 dos dois precisa ser um conector (→)
            if ((ac1.isConector && ac1.ConectorProp.Simbolo != ESimbolo.IMPLICA) || (ac2.isConector && ac2.ConectorProp.Simbolo != ESimbolo.IMPLICA)) { return false; }
            if (ac1.isAtomo || ac2.isAtomo)
            {
                return ac1.isAtomo ? ac2.ConectorProp.Esquerda.Equals(ac1) : ac1.ConectorProp.Esquerda.Equals(ac2);
            }

            bool rt = ac1.isConector ? ac1.ConectorProp.Esquerda.Equals(ac2) : false;
            return rt ? rt : ac2.ConectorProp.Esquerda.Equals(ac1);
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (!isValid(cf1, cf2)) { return null; }

            AtomoConector ac1 = cf1.AtomoConectorProp;
            AtomoConector ac2 = cf2.AtomoConectorProp;

            if (ac1.isAtomo || ac2.isAtomo)
            {
                return new ConjuntoFormula(true, ac1.isAtomo ? ac2.ConectorProp.Direita.copy() : ac1.ConectorProp.Direita.copy());
            }

            bool aux = ac1.isConector ? ac1.ConectorProp.Esquerda.Equals(ac2) : false;
            return new ConjuntoFormula(true, aux ? ac1.ConectorProp.Direita.copy() : ac2.ConectorProp.Direita.copy());
        }

    }

}