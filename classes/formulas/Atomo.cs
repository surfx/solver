using classes.auxiliar.formulas;

namespace classes.formulas
{
    public class Atomo : IConversor
    {
        public string Simbolo { get; set; }
        //public bool Negado { get; set; }

        public int NumeroNegados { get; set; }

        public Atomo(string simbolo = "", int numeroNegados = 0)
        {
            this.Simbolo = simbolo;
            this.NumeroNegados = numeroNegados;
        }

        #region util

        public AtomoConector toAtomoConector()
        {
            return new(this);
        }

        public Atomo copy()
        {
            return new(Simbolo, NumeroNegados);
        }

        // não considero negado número par de negativas...
        public bool isNegado { get => NumeroNegados == 1 || NumeroNegados % 2 == 1; }
        #endregion

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Atomo atomo &&
                   Simbolo == atomo.Simbolo &&
                   NumeroNegados == atomo.NumeroNegados;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Simbolo);
        }

        public override string? ToString() => string.Format("{0}{1}", NumeroNegados > 0 ? string.Concat(Enumerable.Repeat(AuxiliarFormulas.SimboloNegado, NumeroNegados)) : "", Simbolo);

    }
}