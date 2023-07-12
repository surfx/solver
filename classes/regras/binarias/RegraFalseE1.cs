using classes.formulas;

namespace classes.regras.binarias
{

    /*
        F A ˄ B
        T A
        ------- (F ˄1)
        F B

        obs: A ou B podem ser conectores (!)
    */
    public class RegraFalseE1 : IRegraBinaria
    {
        public string RULE { get => "F ˄1"; }

        // se a regra se aplica a estes objetos cf
        public bool isValid(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (cf1 == null || cf2 == null || cf1.AtomoConectorProp == null || cf2.AtomoConectorProp == null) { return false; }
            // se ambos forem T ou F
            if ((cf1.Simbolo && cf2.Simbolo) || (!cf1.Simbolo && !cf2.Simbolo)) { return false; }

            // conector ou átomo
            AtomoConector ac1 = !cf1.Simbolo ? cf1.AtomoConectorProp : cf2.AtomoConectorProp; // parte F
            AtomoConector ac2 = cf2.Simbolo ? cf2.AtomoConectorProp : cf1.AtomoConectorProp;  // parte T
            // parte F deve ser um conector, e o símbolo deve ser E
            if (ac1 == null || ac1.isAtomo || ac1.ConectorProp == null || ac1.ConectorProp.Simbolo != ESimbolo.E || ac1.ConectorProp.Esquerda == null) { return false; }
            return ac1.ConectorProp.Esquerda.Equals(ac2);
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (!isValid(cf1, cf2)) { return null; }
            AtomoConector? ac1 = !cf1.Simbolo ? cf1.AtomoConectorProp : cf2.AtomoConectorProp; // parte F
            return ac1 == null || ac1.ConectorProp == null || ac1.ConectorProp.Direita == null ? null :
                new ConjuntoFormula(false, ac1.ConectorProp.Direita.copy());
        }

    }

}