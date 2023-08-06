using classes.formulas;

namespace classes.regras.binarias
{

    /*
        T A → B
        F B
        ------- (T →2)
        F A

        obs: A ou B podem ser conectores (!)
    */
    public class RegraTrueImplica2 : IRegraBinaria
    {
        public string RULE { get => "T →2"; }

        // se a regra se aplica a estes objetos cf
        public bool isValid(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (cf1 == null || cf2 == null || cf1.AtomoConectorProp == null || cf2.AtomoConectorProp == null) { return false; }
            // se ambos forem T ou F
            if ((cf1.Simbolo && cf2.Simbolo) || (!cf1.Simbolo && !cf2.Simbolo)) { return false; }

            // conector ou atomo
            AtomoConector ac1 = cf1.Simbolo ? cf1.AtomoConectorProp : cf2.AtomoConectorProp; // T
            AtomoConector ac2 = !cf2.Simbolo ? cf2.AtomoConectorProp : cf1.AtomoConectorProp; // F

            // parte T deve ser um conector, e o símbolo deve ser →
            if (ac1 == null || ac1.isAtomo || ac1.ConectorProp == null || ac1.ConectorProp.Simbolo != ESimbolo.IMPLICA || ac1.ConectorProp.Direita == null) { return false; }
            return ac1.ConectorProp.Direita.Equals(ac2); // se a parte direita de ac1 é igual a ac2
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (!isValid(cf1, cf2)) { return null; }
            AtomoConector? ac1 = cf1.Simbolo ? cf1.AtomoConectorProp : cf2.AtomoConectorProp; // T
            return ac1 == null || ac1.ConectorProp == null || ac1.ConectorProp.Esquerda == null ? null :
                new(false, ac1.ConectorProp.Esquerda.copy());
        }

    }

}