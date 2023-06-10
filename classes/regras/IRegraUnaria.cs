using clases.formulas;

namespace clases.regras {

    public interface IRegraUnaria {
        
        public string RULE {get;}

        public bool isValid(ConjuntoFormula cf);

        public ConjuntoFormula? apply(ConjuntoFormula cf);

    }

}