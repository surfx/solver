using classes.formulas;

namespace classes.regras {

    public interface IRegraUnaria {
        
        public string RULE {get;}

        public bool isValid(ConjuntoFormula cf);

        public ConjuntoFormula? apply(ConjuntoFormula cf);

    }

}