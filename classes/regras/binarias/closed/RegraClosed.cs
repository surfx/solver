using classes.formulas;

namespace classes.regras.binarias.closed
{

    /*
        F A
        T A
        ------- (Closed)
        true

        obs: A ou B podem ser conectores (!)
    */
    public class RegraClosed
    {
        public string RULE { get => "Closed"; }

        public bool apply(ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
             if (cf1 == null || cf2 == null || cf1.AtomoConectorProp == null || cf2.AtomoConectorProp == null) { return false; }
            // se ambos forem T ou F
            if ((cf1.Simbolo && cf2.Simbolo) || (!cf1.Simbolo && !cf2.Simbolo)) { return false; }
            return cf1.AtomoConectorProp.Equals(cf2.AtomoConectorProp);
        }

    }

}