using clases.formulas;

namespace clases.auxiliar
{
    public static class Auxiliar
    {

        public const string SimboloNegado = "¬";

        public static string toStrSimbolo(ESimbolo simbolo)
        {
            switch (simbolo)
            {
                case ESimbolo.E: return "^";
                case ESimbolo.OU: return "v";
                case ESimbolo.IMPLICA: return "→";
            }
            return string.Empty;
        }

    }
}