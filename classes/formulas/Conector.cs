using clases.auxiliar;

namespace clases.formulas
{
    public class Conector : IConversor
    {
        public ESimbolo Simbolo { get; set; }
        public AtomoConector? Esquerda { get; set; }
        public AtomoConector? Direita { get; set; }
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

        #region util

        public AtomoConector toAtomoConector()
        {
            return new AtomoConector(this);
        }

        public Conector copy()
        {
            return new Conector(Simbolo, Esquerda, Direita, Negado);
        }

        #endregion

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Conector) { return false; }
            Conector o = (Conector)obj;
            return Simbolo == o.Simbolo &&
                    (Esquerda != null && Esquerda.Equals(o.Esquerda)) &&
                    (Direita != null && Direita.Equals(o.Direita)) &&
                    Negado == o.Negado;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            string rt = string.Format(
                "{0}{1}{2} {3} {4}{5}{6}", 
                (Esquerda != null && Esquerda.isConector) ? "(" : "", 
                Esquerda, 
                (Esquerda != null && Esquerda.isConector) ? ")" : "",
                Auxiliar.toStrSimbolo(Simbolo), 
                (Direita != null && Direita.isConector) ? "(" : "", 
                Direita, 
                (Direita != null && Direita.isConector) ? ")" : "");

            return Negado ? string.Format("{0}({1})", Auxiliar.SimboloNegado, rt) : rt;
        }

    }
}