using classes.auxiliar.saidas.print;
using classes.formulas;

namespace classes.solverstage
{
    public class Formulas : IDisposable
    {
        private List<ConjuntoFormula>? _lconjuntoFormula;
        public List<ConjuntoFormula>? LConjuntoFormula { get => _lconjuntoFormula; }

        private Formulas? _esquerda, _direita;
        public Formulas? Esquerda { get => _esquerda; }
        public Formulas? Direita { get => _direita; }

        public bool isClosed { get; set; }

        public Formulas()
        {

        }

        public void addConjuntoFormula(ConjuntoFormula cf)
        {
            if (_lconjuntoFormula == null) { _lconjuntoFormula = new(); }
            _lconjuntoFormula.Add(cf);
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

        public void updateFormulas(List<ConjuntoFormula> lformulas)
        {
            if (lformulas == null || lformulas.Count <= 0) { return; }
            this._lconjuntoFormula = lformulas;
        }

        public override string ToString()
        {
            return new PrintFormulas().toString(this);
        }

        public void Dispose()
        {
            _lconjuntoFormula = null;
            _esquerda = null;
            _direita = null;
        }

    }
}