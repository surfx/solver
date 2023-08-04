using classes.auxiliar;
using classes.auxiliar.formulas;

namespace classes.formulas
{
    public class Conector : IConversor
    {
        public ESimbolo Simbolo { get; set; }
        public AtomoConector? Esquerda { get; set; }
        public AtomoConector? Direita { get; set; }
        //public bool Negado { get; set; }
        public int NumeroNegados { get; set; }

        public Conector(ESimbolo simbolo, AtomoConector esquerda, AtomoConector direita, int numeroNegados = 0)
        {
            Simbolo = simbolo;
            Esquerda = esquerda;
            Direita = direita;
            this.NumeroNegados = numeroNegados;
        }

        public Conector(ESimbolo simbolo, IConversor esquerda, IConversor direita, int numeroNegados = 0)
        {
            Simbolo = simbolo;
            Esquerda = esquerda == null ? null : esquerda.toAtomoConector();
            Direita = direita == null ? null : direita.toAtomoConector();
            this.NumeroNegados = numeroNegados;
        }

        #region util

        public AtomoConector toAtomoConector()
        {
            return new AtomoConector(this);
        }

        public Conector copy()
        {
            return new Conector(Simbolo, Esquerda, Direita, NumeroNegados);
        }

        // não considero negado número par de negativas...
        public bool isNegado { get => NumeroNegados == 1 || NumeroNegados % 2 == 1; }

        #endregion

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Conector) { return false; }
            Conector o = (Conector)obj;
            return Simbolo == o.Simbolo &&
                    (Esquerda != null && Esquerda.Equals(o.Esquerda)) &&
                    (Direita != null && Direita.Equals(o.Direita)) &&
                    NumeroNegados == o.NumeroNegados;
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
                AuxiliarFormulas.toSimbolo(Simbolo),
                (Direita != null && Direita.isConector) ? "(" : "",
                Direita,
                (Direita != null && Direita.isConector) ? ")" : "");

            string pattern = NumeroNegados > 0 ? "{0}({1})" : "{0}{1}";
            return string.Format(pattern, NumeroNegados > 0 ? string.Concat(Enumerable.Repeat(AuxiliarFormulas.SimboloNegado, NumeroNegados)) : "", rt);
        }

    }
}