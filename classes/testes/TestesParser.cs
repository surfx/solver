using clases.auxiliar;
using clases.formulas;

namespace clases.testes
{

    public class TestesParser
    {

        public void teste1()
        {

            string entrada = "A";
            entrada = "A! & B"; // T A¬ ˄ B
            entrada = "(!A & B) | C";
            entrada = "!(A & B) | C";
            entrada = "!!(A -> B) & C";

            entrada = "(B -> ( (G -> !(D ^ E)) ) -> ((!D | G) & C))"; // T
            entrada = sanitizar(entrada);
            entrada = sanitizar("(A -> H) | F");
            entrada = sanitizar("(A -> H) | () F | r");
            entrada = sanitizar("(A -> H) | ( F | r"); // inválida
            entrada = sanitizar("(A -> H) | ) F | r"); // inválida
            entrada = sanitizar("!(A -> !H) | ((!F) | !(r)) & (!!!g -> !!!!!!y)");

            p(entrada);

            //p(string.Format("{0} {1}", entrada, isParentesisOk(entrada)));

            ConjuntoFormula formulaAC = parserCF("t" + entrada);

            //p(); foreach (KeyValuePair<string, AtomoConector?> entry in mapFormulas) { p(string.Format("{0}: {1}", entry.Key, entry.Value)); } p();

            p(formulaAC == null ? "null/inválida" : formulaAC.ToString());


            //p(testeR(entrada));

            // Console.WriteLine(string.Format("{0}\n{1}", entrada, sanitizar(entrada)));

            // AtomoConector ac = parser(entrada);
            // if (ac != null) { p(); Console.WriteLine(ac); }

            //entrada = sanitizar("(!A)");
            //entrada = sanitizar("!C");
            //entrada = sanitizar(" !( !(!( D  !)) ) ");
            //p(string.Format("{0}, {1}, atomo: {2}", entrada, isAtomo(entrada)?"sim":"não", toAtomo(entrada)));

            // entrada = sanitizar("!(A & B) | D"); //  | (D & !V)
            // p(string.Format("{0}, {1}, conector: {2}", entrada, isConector(entrada)?"sim":"não", toAtomoConector(entrada)));

        }

        private bool isNegativosOk(string entrada){
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return false; }
            if (!entrada.Contains(simboloNegado)){return true;}
            if (string.IsNullOrEmpty(replaceAll(entrada, simboloNegado, ""))){return false;}

            int max = entrada.Length - 1;
            int pos = entrada.IndexOf(simboloNegado);
            if (pos >= max) {return false;}
            char[] array = entrada.ToCharArray();
            while(pos <= max) {
                string next = array[++pos].ToString();
                if (next.Equals(simboloNegado)){pos++; continue;}
                if (next.Equals("(")){return true;}
                if (next.Equals(")")){return false;}
                if (conectores.Contains(next)){
                    return false;
                }
                return true;
            }
            return true;
        }

        public void teste2()
        {
            string entrada = sanitizar("!!!!!A");
            Atomo atomo = toAtomo(entrada);
            p(atomo.ToString() + ", " + atomo.isNegado);
            atomo.NumeroNegados = 48;
            p(atomo.ToString() + ", " + atomo.isNegado);

            p();

            entrada = sanitizar("(!A->!!!B)");
            entrada = sanitizar("!!(A->B)");
            AtomoConector ac = toAtomoConector(entrada);
            p(ac.ToString() + ", " + ac.isNegado);

        }

        // (B→((G→!(D˄E)))→(¬D˅G)˄C)

        private ConjuntoFormula? parserCF(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }
            entrada = sanitizar(entrada);

            // OBS: se a entrada começa com T ou F, será o símbolo. Se não, considero T
            bool simbolo = !entrada.Substring(0, 1).ToUpper().Equals("F");
            if (entrada.Substring(0, 1).ToUpper().Equals("T") || entrada.Substring(0, 1).ToUpper().Equals("F"))
            {
                entrada = entrada.Substring(1);
            }

            AtomoConector? ac = parser(entrada);
            mapFormulas.Clear();

            return ac == null ? null : new ConjuntoFormula(simbolo, ac);
        }

        private Dictionary<string, AtomoConector?> mapFormulas = new Dictionary<string, AtomoConector?>();

        private AtomoConector? parser(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }
            entrada = sanitizar(entrada);
            if (!isParentesisOk(entrada) || !isNegativosOk(entrada)) { return null; }
            entrada = replaceAll(entrada, "()");
            Random rnd = new Random();

            if (entrada.Contains("("))
            {
                int posI = -1, posF = -1;
                char[] array = entrada.ToCharArray();
                for (int i = 0; i < array.Length; i++)
                {
                    char ch = array[i];
                    if (ch.Equals('('))
                    {
                        posI = i;
                        continue;
                    }
                    if (ch.Equals(')'))
                    {
                        posF = i;
                        break;
                    }
                }

                if (posI > 0)
                {
                    while (entrada.Substring(posI - 1, 1) == simboloNegado)
                    {
                        posI--;
                        if (posI <= 0) { break; }
                    }
                }

                string toParser = entrada.Substring(posI, posF - posI + 1);
                string key = "IDD" + rnd.Next(1, 100);
                while (mapFormulas.ContainsKey(key)) { key = "IDD" + rnd.Next(1, 100); }
                string novaEntrada = replaceAll(entrada, toParser, key); //entrada.Substring(0, posI) + "IDD" + entrada.Substring(posF);
                AtomoConector? ac = toAtomoConector(toParser);

                ac = tratarAC(ac);

                //p(string.Format("AUX {0}, esq: {1}, dir: {2}", ac, ac.isConector?ac.ConectorProp.Esquerda:"", ac.isConector?ac.ConectorProp.Direita:""));
                //p(string.Format("{0}, {1}, toParser: {2}, ac: {3}, novaEntrada: {4}", posI, posF, toParser, ac, novaEntrada));

                mapFormulas.Add(key, ac);


                return parser(novaEntrada);
            }


            return mapFormulas.ContainsKey(entrada) ? mapFormulas[entrada] : tratarAC(toAtomoConector(entrada));

        }

        private AtomoConector tratarAC(AtomoConector ac)
        {
            if (ac == null) { return ac; }
            string keyAux = "";
            if (ac.isConector)
            {
                keyAux = ac.ConectorProp.Esquerda.ToString();

                AtomoConector aux;
                if (mapFormulas.ContainsKey(keyAux))
                {
                    ac.ConectorProp.Esquerda = mapFormulas[keyAux];
                }
                else
                {
                    aux = toAtomoConector(keyAux);
                    if (aux.isConector)
                    {
                        ac.ConectorProp.Esquerda = tratarAC(aux);
                    }
                    else if (aux.isAtomo)
                    {
                        ac.AtomoProp = tratarAC(aux).AtomoProp;
                    }
                }

                keyAux = ac.ConectorProp.Direita.ToString();
                if (mapFormulas.ContainsKey(keyAux))
                {
                    ac.ConectorProp.Direita = mapFormulas[keyAux];
                }
                else
                {
                    aux = toAtomoConector(keyAux);
                    if (aux.isConector)
                    {
                        ac.ConectorProp.Direita = tratarAC(aux);
                    }
                    else if (aux.isAtomo)
                    {
                        ac.AtomoProp = tratarAC(aux).AtomoProp;
                    }
                }


            }
            else if (ac.isAtomo)
            {
                keyAux = ac.AtomoProp.ToString();
                if (mapFormulas.ContainsKey(keyAux))
                {
                    ac = mapFormulas[keyAux];
                }
            }
            return ac;
        }

        private bool isParentesisOk(string entrada)
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

        // Console.WriteLine(string.Join(" ", conectores)); ˄ ˅ →
        private string[] conectores = Auxiliar.getSimbolos(false);
        private const string simboloNegado = Auxiliar.SimboloNegado;


        public string sanitizar(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return ""; }

            entrada = replaceAll(entrada, " ", "");
            entrada = replaceSimbols(entrada);

            return entrada;
        }

        private string replaceSimbols(string entrada)
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

        private string replaceAll(string entrada, string simbolo1, string simbolo2 = "")
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return ""; }
            while (entrada.Contains(simbolo1)) { entrada = entrada.Replace(simbolo1, simbolo2); }
            return entrada;
        }

        private string removerParenteses(string entrada) { return replaceAll(replaceAll(entrada, "(", ""), ")", ""); }

        private int[] allIndexes(string entrada, string[] conectores)
        {
            int[] rt = conectores.Select(x => allIndexes(entrada, x)).Where(i => i != null).SelectMany(x => x).ToArray();
            if (rt == null) { return rt; }
            Array.Sort(rt);
            return rt;
        }

        private int[]? allIndexes(string entrada, string simbolo)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada) || simbolo == null || string.IsNullOrEmpty(simbolo)) { return null; }

            int pos = entrada.IndexOf(simbolo);
            if (pos < 0) { return null; }
            List<int> posicoes = new List<int>();
            posicoes.Add(pos);

            while (pos >= 0)
            {
                pos = entrada.IndexOf(simbolo, pos + 1);
                if (pos < 0) { break; }
                posicoes.Add(pos);
            }
            return posicoes.ToArray();
        }


        private bool isAtomo(string entrada)
        {
            // não pode ter um conector
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return false; }
            return !conectores.Any(x => entrada.IndexOf(x) >= 0);
            //return !conectores.Select(x => entrada.IndexOf(x) >= 0).Any(x=>x);
        }

        private bool isConector(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return false; }
            return conectores.Any(x => entrada.IndexOf(x) >= 0);
        }

        private Atomo? toAtomo(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }
            entrada = sanitizar(entrada);
            if (!isAtomo(entrada)) { return null; }
            entrada = removerParenteses(entrada);
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }

            //entrada = replaceAll(entrada, simboloNegado + simboloNegado, "");

            // int sizeRem = simboloNegado.Length;
            // while (entrada.EndsWith(simboloNegado))
            // {
            //     entrada = entrada.Substring(0, entrada.Length - sizeRem);
            // }

            int numeroNegados = 0;
            while (entrada.StartsWith(simboloNegado))
            {
                numeroNegados++;
                entrada = entrada.Substring(1);
            }
            return new Atomo(entrada, numeroNegados);
        }

        private AtomoConector? toAtomoConector(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }
            entrada = sanitizar(entrada);
            if (!isParentesisOk(entrada) || entrada.Equals("()")) { return null; }
            if (isAtomo(entrada)) { return toAtomo(entrada).toAtomoConector(); }
            if (!isConector(entrada)) { return null; }

            Conector rt = null;
            // string[] conectores
            int[] indices = allIndexes(entrada, conectores);
            foreach (int num in indices)
            {
                //Console.WriteLine(num);

                string left, right, conectorStr;

                left = entrada.Substring(0, num);
                right = entrada.Substring(num + 1);
                conectorStr = entrada.Substring(num, 1);
                ESimbolo? conector = Auxiliar.toSimbolo(conectorStr);

                int numeroNegados = 0;
                while (left.StartsWith(simboloNegado))
                {
                    numeroNegados++;
                    left = left.Substring(1);
                }

                while (left.Contains("("))
                {
                    left = left.Substring(left.IndexOf("(") + 1);
                }
                while (right.Contains(")"))
                {
                    right = right.Substring(0, right.IndexOf(")"));
                }

                AtomoConector esquerda = toAtomoConector(left);
                AtomoConector direita = toAtomoConector(right);

                //p(string.Format("'{0}' '{1}' '{2}' : '{3}'", left, conectorStr, right, conector == null ? "" : Auxiliar.toSimbolo(conector)));

                rt = new Conector((ESimbolo)conector, esquerda, direita, numeroNegados);

            }

            return rt == null ? null : new AtomoConector(rt);
        }

        private void p() { p("-----------------"); }
        private void p(string str) { Console.WriteLine(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return values == null ? "" : string.Join(separator, values); }

    }

}
/*
T G → ¬D
T (G → ¬D) → (¬D v G)

¬, ˄, v, →

*/