using classes.formulas;

namespace classes.auxiliar.formulas
{
    public static class AuxiliarFormulas
    {

        public const string SimboloNegado = "¬";

        public const string SimboloE = "˄";
        public const string SimboloOu = "˅";
        public const string SimboloImplica = "→";


        public static string toSimbolo(ESimbolo? simbolo)
        {
            if (simbolo == null) { return string.Empty; }
            return simbolo switch
            {
                ESimbolo.E => SimboloE,
                ESimbolo.OU => SimboloOu,
                ESimbolo.IMPLICA => SimboloImplica,
                _ => string.Empty,
            };
        }

        public static ESimbolo? toSimbolo(string simbolo)
        {
            if (simbolo == null || string.IsNullOrEmpty(simbolo)) { return null; }
            switch (simbolo)
            {
                case SimboloE: return ESimbolo.E;
                case SimboloOu: return ESimbolo.OU;
                case SimboloImplica: return ESimbolo.IMPLICA;
            }
            return null;
        }

        public static string[] getSimbolos(bool addSimboloNegado = true)
        {
            return addSimboloNegado ? new string[] { SimboloNegado, SimboloE, SimboloOu, SimboloImplica } : new string[] { SimboloE, SimboloOu, SimboloImplica };
        }

    }
}