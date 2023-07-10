using classes.auxiliar.saidas.print;
using classes.formulas;

namespace classes.solverstage
{
    public class Formulas : IDisposable
    {
        private List<ConjuntoFormula>? _positivas, _negativas;
        public List<ConjuntoFormula>? Positivas { get => _positivas; }
        public List<ConjuntoFormula>? Negativas { get => _negativas; }

        private Formulas? _esquerda, _direita;
        public Formulas? Esquerda { get => _esquerda; }
        public Formulas? Direita { get => _direita; }

        public bool isClosed { get; set; }

        public Formulas()
        {

        }

        public void addConjuntoFormula(ConjuntoFormula cf)
        {
            if (cf.Simbolo)
            {
                if (_positivas == null) { _positivas = new List<ConjuntoFormula>(); }
                _positivas.Add(cf);
            }
            else
            {
                if (_negativas == null) { _negativas = new List<ConjuntoFormula>(); }
                _negativas.Add(cf);
            }
        }

        public void addEsquerda(ConjuntoFormula cf)
        {
            if (_esquerda == null) { _esquerda = new Formulas(); }
            _esquerda.addConjuntoFormula(cf);
        }

        public void addDireita(ConjuntoFormula cf)
        {
            if (_direita == null) { _direita = new Formulas(); }
            _direita.addConjuntoFormula(cf);
        }

        public override string ToString()
        {
            return new PrintFormulas().toString(this);
        }

        public void Dispose()
        {
            _positivas = null;
            _negativas = null;
            _esquerda = null;
            _direita = null;
        }

    }
}