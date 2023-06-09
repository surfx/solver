using clases.auxiliar;

namespace clases.formulas
{
    public class Atomo : IConversor
    {
        public string Simbolo { get; set; }
        public bool Negado { get; set; }

        public Atomo(string simbolo = "", bool negado = false)
        {
            this.Simbolo = simbolo;
            this.Negado = negado;
        }

        #region util

        public AtomoConector toAtomoConector()
        {
            return new AtomoConector(this);
        }

        public Atomo copy()
        {
            return new Atomo(Simbolo, Negado);
        }

        #endregion

        public override bool Equals(object? obj)
        {
            return obj != null && obj is Atomo atomo &&
                   Simbolo == atomo.Simbolo &&
                   Negado == atomo.Negado;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Simbolo);
        }

        public override string? ToString()
        {
            return (Negado ? Auxiliar.SimboloNegado : "") + Simbolo;
        }

    }
}