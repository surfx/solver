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

            // 1 dos dois precisa ser um conector (→)
            if ((ac1.isConector && ac1.ConectorProp.Simbolo != ESimbolo.IMPLICA) || (ac2.isConector && ac2.ConectorProp.Simbolo != ESimbolo.IMPLICA)) { return false; }
            // ac1 é a parte True, não pode ser um átomo
            if (ac1.isAtomo) { return false; }

            // se a parte esquerda de ac1 é igual a ac2
            return ac1.ConectorProp.Direita.Equals(ac2);
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            if (!isValid(cf1, cf2)) { return null; }

            AtomoConector ac1 = cf1.Simbolo ? cf1.AtomoConectorProp : cf2.AtomoConectorProp; // T
            return new ConjuntoFormula(false, ac1.ConectorProp.Esquerda.copy());
        }

    }

}