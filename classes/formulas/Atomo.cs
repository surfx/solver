using classes.auxiliar;

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
            return new AtomoConector(this);
        }

        public Atomo copy()
        {
            return new Atomo(Simbolo, NumeroNegados);
        }

        // não considero negado número par de negativas...
        public bool isNegado { get => NumeroNegados == 1 || NumeroNegados % 2 == 1; }

        public int sizeStr()
        {
            //return this.Simbolo.Length + (NumeroNegados <= 0 ? 0 : NumeroNegados / 2);
            return 1;
        }

        public int heightTree(){
            return 1;
        }
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

        public override string? ToString()
        {
            return string.Format("{0}{1}", NumeroNegados > 0 ? string.Concat(Enumerable.Repeat(Auxiliar.SimboloNegado, NumeroNegados)) : "", Simbolo);
        }

    }
}