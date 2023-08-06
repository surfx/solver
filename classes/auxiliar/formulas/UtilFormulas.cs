namespace classes.auxiliar.formulas
{
    public static class UtilFormulas
    {

        // Console.WriteLine(string.Join(" ", conectores)); ˄ ˅ →
        private static readonly string[] conectores = AuxiliarFormulas.getSimbolos(false);
        private const string simboloNegado = AuxiliarFormulas.SimboloNegado;

        public static string sanitizar(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return ""; }

            entrada = replaceAll(entrada, " ", "");
            entrada = replaceSimbols(entrada);

            return entrada;
        }

        public static string replaceSimbols(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return ""; }
            entrada = replaceAll(entrada, "!", AuxiliarFormulas.SimboloNegado);
            entrada = replaceAll(entrada, "^", AuxiliarFormulas.SimboloE);
            entrada = replaceAll(entrada, "&", AuxiliarFormulas.SimboloE);
            entrada = replaceAll(entrada, "|", AuxiliarFormulas.SimboloOu);
            entrada = replaceAll(entrada, "->", AuxiliarFormulas.SimboloImplica);
            entrada = replaceAll(entrada, ">", AuxiliarFormulas.SimboloImplica);
            return entrada;
        }

        public static string replaceAll(string entrada, string simbolo1, string simbolo2 = "")
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return ""; }
            while (entrada.Contains(simbolo1)) { entrada = entrada.Replace(simbolo1, simbolo2); }
            return entrada;
        }

        public static bool isParentesisOk(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return true; }
            if (!entrada.Contains('(') && !entrada.Contains(')')) { return true; }

            // remover tudo que não seja parêntesis '(' e ')'
            string aux = "";
            foreach (char ch in entrada.ToCharArray())
            {
                if (!ch.Equals('(') && !ch.Equals(')')) { continue; }
                aux += ch;
            }
            aux = replaceAll(aux, "()");
            return string.IsNullOrEmpty(aux == null ? "" : aux.Trim());
        }

        public static bool isNegativosOk(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return false; }
            if (!entrada.Contains(simboloNegado)) { return true; }
            if (string.IsNullOrEmpty(UtilFormulas.replaceAll(entrada, simboloNegado, ""))) { return false; }

            int max = entrada.Length - 1;
            int pos = entrada.IndexOf(simboloNegado);
            if (pos >= max) { return false; }
            char[] array = entrada.ToCharArray();
            while (pos <= max)
            {
                string next = array[++pos].ToString();
                if (next.Equals(simboloNegado)) { continue; }
                if (next.Equals("(")) { return true; }
                if (next.Equals(")")) { return false; }
                if (conectores.Contains(next))
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public static void p() { p("-----------------"); }
        public static void p(string str) { Console.WriteLine(str); }
        public static string toStr<T>(IEnumerable<T> values, String? separator = " ") { return values == null ? "" : string.Join(separator, values); }

    }
}