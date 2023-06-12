namespace classes.auxiliar
{
    public static class UtilFormulas
    {

        // Console.WriteLine(string.Join(" ", conectores)); ˄ ˅ →
        private static string[] conectores = Auxiliar.getSimbolos(false);
        private const string simboloNegado = Auxiliar.SimboloNegado;

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
            entrada = replaceAll(entrada, "!", Auxiliar.SimboloNegado);
            entrada = replaceAll(entrada, "^", Auxiliar.SimboloE);
            entrada = replaceAll(entrada, "&", Auxiliar.SimboloE);
            entrada = replaceAll(entrada, "|", Auxiliar.SimboloOu);
            entrada = replaceAll(entrada, "->", Auxiliar.SimboloImplica);
            entrada = replaceAll(entrada, ">", Auxiliar.SimboloImplica);
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
            if (!entrada.Contains("(") && !entrada.Contains(")")) { return true; }

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