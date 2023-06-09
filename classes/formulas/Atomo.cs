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

        public AtomoConector toAtomoConector()
        {
            return new AtomoConector(this);
        }

        public override bool Equals(object? obj)
        {
            return obj is Atomo atomo &&
                   Simbolo == atomo.Simbolo;
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