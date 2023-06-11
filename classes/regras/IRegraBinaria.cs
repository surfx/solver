using classes.formulas;

namespace classes.regras {

    public interface IRegraBinaria {

        public string RULE {get;}

        public bool isValid(ConjuntoFormula cf1, ConjuntoFormula cf2);

        public ConjuntoFormula? apply(ConjuntoFormula cf1, ConjuntoFormula cf2);

    }

}