using clases.auxiliar;

namespace clases.formulas
{
    public class Conector : IConversor
    {
        private ESimbolo Simbolo { get; set; }
        private AtomoConector? Esquerda { get; set; }
        private AtomoConector? Direita { get; set; }
        public bool Negado { get; set; }

        public Conector(ESimbolo simbolo, AtomoConector esquerda, AtomoConector direita, bool negado = false)
        {
            Simbolo = simbolo;
            Esquerda = esquerda;
            Direita = direita;
            Negado = negado;
        }

        public Conector(ESimbolo simbolo, IConversor esquerda, IConversor direita, bool negado = false)
        {
            Simbolo = simbolo;
            Esquerda = esquerda == null ? null : esquerda.toAtomoConector();
            Direita = direita == null ? null : direita.toAtomoConector();
            Negado = negado;
        }

        public AtomoConector toAtomoConector()
        {
            return new AtomoConector(this);
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            string rt = string.Format("{0}{1}{2} {3} {4}{5}{6}", (Esquerda != null && Esquerda.isConector) ? "(" : "", Esquerda, (Esquerda != null && Esquerda.isConector) ? ")" : "",
            Auxiliar.toStrSimbolo(Simbolo), (Direita != null && Direita.isConector) ? "(" : "", Direita, (Direita != null && Direita.isConector) ? ")" : "");

            return Negado ? string.Format("{0}({1})", Auxiliar.SimboloNegado, rt) : rt;
        }

    }
}