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
    public class RegraTrueImplica1 : IRegraBinaria
    {
        public string RULE { get => "T →1"; }

        // se a regra se aplica a estes objetos cf
        public bool isValid(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (cf1 == null || cf2 == null || cf1.AtomoConectorProp == null || cf2.AtomoConectorProp == null) { return false; }
            if (!cf1.Simbolo || !cf2.Simbolo) { return false; } // ambos T
            if (cf1.AtomoConectorProp.isAtomo && cf2.AtomoConectorProp.isAtomo) { return false; } // ambos átomos

            // conector ou atomo
            AtomoConector ac1 = cf1.AtomoConectorProp;
            AtomoConector ac2 = cf2.AtomoConectorProp;

            // 1 dos dois precisa ser um conector (→)
            if ((ac1.isConector && ac1.ConectorProp?.Simbolo != ESimbolo.IMPLICA) || (ac2.isConector && ac2.ConectorProp?.Simbolo != ESimbolo.IMPLICA)) { return false; }
            if (ac1.isAtomo || ac2.isAtomo)
            {
                return ac1.isAtomo ?
                    ac2.ConectorProp != null && ac2.ConectorProp.Esquerda != null && ac2.ConectorProp.Esquerda.Equals(ac1) :
                    ac1.ConectorProp != null && ac1.ConectorProp.Esquerda != null && ac1.ConectorProp.Esquerda.Equals(ac2);
            }

            return ac1.isConector ?
                ac1.ConectorProp != null && ac1.ConectorProp.Esquerda != null && ac1.ConectorProp.Esquerda.Equals(ac2) :
                ac2.ConectorProp != null && ac2.ConectorProp.Esquerda != null && ac2.ConectorProp.Esquerda.Equals(ac1);
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (!isValid(cf1, cf2)) { return null; }

            AtomoConector? ac1 = cf1.AtomoConectorProp;
            AtomoConector? ac2 = cf2.AtomoConectorProp;

            if ((ac1 != null && ac1.isAtomo) || (ac2 != null && ac2.isAtomo))
            {
                return new(true, ac1 != null && ac1.isAtomo ? ac2?.ConectorProp?.Direita?.copy() : ac1?.ConectorProp?.Direita?.copy());
            }

            bool aux = ac1 != null && ac1.isConector && ac1.ConectorProp != null && ac1.ConectorProp.Esquerda != null && ac1.ConectorProp.Esquerda.Equals(ac2);
            return new(true, aux ? ac1?.ConectorProp?.Direita?.copy() : ac2?.ConectorProp?.Direita?.copy());
        }

    }

}