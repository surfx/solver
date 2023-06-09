using clases.formulas;

namespace clases.regras.binarias
{

    /*
        T A → B
        T A
        ------- (T →1)
        T B

        obs: Se B for um Conector, o retorna
    */
    public class RegraTrueImplica
    {

        // se a regra se aplica a estes objetos cf
        public static bool isValid(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (cf1 == null || cf2 == null || cf1.AtomoConectorProp == null || cf2.AtomoConectorProp == null) { return false; }
            if (!cf1.Simbolo || !cf2.Simbolo) { return false; }

            // conector ou atomo
            AtomoConector ac1 = cf1.AtomoConectorProp;
            AtomoConector ac2 = cf1.AtomoConectorProp;
            
            // 1 dos dois precisa ser um conector (→)
            if ((ac1.isConector && ac1.ConectorProp.Simbolo != ESimbolo.IMPLICA) || (ac2.isConector && ac2.ConectorProp.Simbolo != ESimbolo.IMPLICA)){ return false; }

            return ac1.Equals(ac2);

            // if (ac1.isConector){
            //     Conector c1 = ac1.ConectorProp;
            //     if (c1.Esquerda != null) {
            //         AtomoConector esquerda = c1.Esquerda;
            //         if (ac2.isAtomo && esquerda.isConector) {return false;}
            //         return esquerda.isConector ? esquerda.ConectorProp.Equals(ac2.ConectorProp) : esquerda.AtomoProp.Equals(ac2.AtomoProp);
            //     }
            // }

            


            // // 1 dos dois precisa ser um conector (→) e o outro um atomo
            // if ((!cf1.AtomoConectorProp.isConector && !cf2.AtomoConectorProp.isConector) || (!cf1.AtomoConectorProp.isAtomo && !cf2.AtomoConectorProp.isAtomo)) { return false; }

            // // o que for conector, precisa ser um →
            // Conector? c1 = cf1.AtomoConectorProp.isConector ? cf1.AtomoConectorProp.ConectorProp : cf2.AtomoConectorProp.ConectorProp;
            // if (c1 == null || c1.Esquerda == null || c1.Esquerda.AtomoProp == null || c1.Esquerda.AtomoProp.Simbolo == null || c1.Negado || c1.Simbolo != ESimbolo.IMPLICA) { return false; }
            // if (c1.Direita == null) {return false;}
            // //if (c1.Direita == null || c1.Direita.AtomoProp == null || c1.Direita.AtomoProp.Simbolo == null) { return false; }
            // Atomo? a1 = cf1.AtomoConectorProp.isAtomo ? cf1.AtomoConectorProp.AtomoProp : cf2.AtomoConectorProp.AtomoProp;
            // if (a1 == null || a1.Simbolo == null || a1.Negado) { return false; }

            // // os símbolos precisam ser iguais
            // if (!c1.Esquerda.AtomoProp.Simbolo.Equals(a1.Simbolo)) { return false; }
            //return true;
        }

        public static ConjuntoFormula? apply(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (!isValid(cf1, cf2)) { return null; }

            //TODO: terminar lógica

            AtomoConector ac1 = cf1.AtomoConectorProp;
            AtomoConector ac2 = cf1.AtomoConectorProp;

            //ac1.ConectorProp.

            //Conector? c1 = cf1.AtomoConectorProp.isConector ? cf1.AtomoConectorProp.ConectorProp : cf2.AtomoConectorProp.ConectorProp;
            //Atomo? a1 = cf1.AtomoConectorProp.isAtomo ? cf1.AtomoConectorProp.AtomoProp : cf2.AtomoConectorProp.AtomoProp;
            return new ConjuntoFormula(true, c1.Direita);
        }



    }

}