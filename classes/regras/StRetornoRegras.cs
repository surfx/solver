
using classes.formulas;

namespace classes.regras
{
    public struct StRetornoRegras
    {

        public ConjuntoFormula? Esquerda { get; set; }
        public ConjuntoFormula? Direita { get; set; }

        public StRetornoRegras(ConjuntoFormula? esquerda = null, ConjuntoFormula? direita = null)
        {
            this.Esquerda = esquerda;
            this.Direita = direita;
        }

        public override readonly string ToString()
        {
            return string.Format("{0}, {1}", Esquerda, Direita);
        }

    }
}