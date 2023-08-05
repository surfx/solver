using classes.formulas;

namespace classes.regras
{

    public interface IRegraUnariaDouble
    {

        public string RULE { get; }

        public bool isValid(ConjuntoFormula cf);

        public StRetornoRegras? apply(ConjuntoFormula cf);

    }

}