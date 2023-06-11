using clases.auxiliar;
using clases.formulas;

namespace clases.testes
{

    public class TestesParser2
    {

        private static string[] conectores = Auxiliar.getSimbolos(false);
        private const string simboloNegado = Auxiliar.SimboloNegado;

        public void teste1()
        {

            string entrada = UtilFormulas.sanitizar("(!(A)->!!!B)->C");
            entrada = UtilFormulas.sanitizar("(B -> ( (G -> !(D ^ E)) ) -> ((!D | G) & C))");
            p(entrada);

            List<ItemList>? list = toList(entrada);
            if (list == null || list.Count <= 0) { return; }

            p(toStr(list, ","));

            tratarNegacoes(list);
            unirAtomoConector(list);
            p(toStr(list, ","));
        }

        private List<ItemList>? toList(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }
            entrada = UtilFormulas.sanitizar(entrada);
            if (!UtilFormulas.isParentesisOk(entrada) || !UtilFormulas.isNegativosOk(entrada)) { return null; }
            entrada = UtilFormulas.replaceAll(entrada, "()");

            char[] array = entrada.ToCharArray();
            if (array == null) { return null; }

            List<ItemList> rt = new List<ItemList>();
            int size = array.Length;
            for (int i = 0; i < size; i++)
            {
                char ch = array[i];
                if (ch == null || string.IsNullOrEmpty(ch.ToString())) { continue; }

                if (ch.Equals('(') || ch.Equals(')'))
                {
                    rt.Add(new ItemList() { Parentesis = ch.Equals('(') });
                    continue;
                }
                if (ch.ToString().Equals(simboloNegado))
                {
                    rt.Add(new ItemList() { Negacao = simboloNegado });
                    continue;
                }
                if (conectores.Contains(ch.ToString()))
                {
                    ESimbolo? simbolo = Auxiliar.toSimbolo(ch.ToString());
                    if (simbolo == null) { continue; }
                    rt.Add(new ItemList() { Simbolo = (ESimbolo)simbolo });
                    continue;
                }

                string simboloAtual = ch.ToString();
                while ((i + 1) < size && !isAnySimbol(array[i + 1]))
                {
                    simboloAtual += array[++i];
                }

                rt.Add(new ItemList() { AtomoConectorProp = new AtomoConector(new Atomo(simboloAtual)) });
            }

            return removerParentesisDuplos(rt);
        }

        private bool isAnySimbol(char ch)
        {
            if (ch == null || string.IsNullOrEmpty(ch.ToString())) { return false; }
            return (ch.Equals('(') || ch.Equals(')')) || (ch.ToString().Equals(simboloNegado)) || (conectores.Contains(ch.ToString()));
        }

        private List<ItemList> removerParentesisDuplos(List<ItemList> list)
        {
            if (list == null || list.Count <= 0) { return list; }

            List<int> remover = new List<int>();
            int size = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                if (i + 2 >= size) { break; }
                if (list[i].Parentesis == null || list[i + 2].Parentesis == null) { continue; }
                bool p1 = (bool)list[i].Parentesis;
                bool p2 = (bool)list[i + 2].Parentesis;

                if (p1 && !p2) // ( e )
                {
                    remover.Add(i);
                    remover.Add(i + 2);
                }
            }

            if (remover.Count > 0)
            {
                for (int i = remover.Count - 1; i >= 0; i--)
                {
                    list.RemoveAt(remover[i]);
                }
            }

            return remover.Count > 0 ? removerParentesisDuplos(list) : list;
        }


        private List<ItemList> tratarNegacoes(List<ItemList> list)
        {
            if (list == null || list.Count <= 0) { return list; }

            List<int> remover = new List<int>();
            int size = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null || !isNegacao(list[i])) { continue; }

                int numeroNegacoes = 1;
                List<int> removerAux = new List<int>();
                removerAux.Add(i);
                while ((i + 1) < size && isNegacao(list[i + 1]))
                {
                    numeroNegacoes++;
                    i++;
                    removerAux.Add(i);
                }

                if (i + 1 > size) { continue; }
                if (isAtomoConectorProp(list[i + 1]))
                {
                    if (list[i + 1].AtomoConectorProp.isAtomo)
                    {
                        list[i + 1].AtomoConectorProp.AtomoProp.NumeroNegados = numeroNegacoes;
                        remover.AddRange(removerAux);
                    }
                    else if (list[i + 1].AtomoConectorProp.isConector)
                    {
                        list[i + 1].AtomoConectorProp.ConectorProp.NumeroNegados = numeroNegacoes;
                        remover.AddRange(removerAux);
                    }
                }

            }

            if (remover.Count > 0)
            {
                for (int i = remover.Count - 1; i >= 0; i--)
                {
                    list.RemoveAt(remover[i]);
                }
            }

            list = removerParentesisDuplos(list);
            return remover.Count > 0 ? tratarNegacoes(list) : list;
        }

        // TODO: fazer (!)
        private List<ItemList> unirAtomoConector(List<ItemList> list)
        {
            if (list == null || list.Count <= 0) { return list; }

            List<int> remover = new List<int>();
            int size = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null || !isAtomoConectorProp(list[i])) { continue; }
                if (i + 2 > size) { continue; }
                if (!isSimbolo(list[i + 1]) || !isAtomoConectorProp(list[i + 2])) { continue; }

                remover.Add(i);
                remover.Add(i + 1);

                AtomoConector item1 = list[i].AtomoConectorProp;
                ESimbolo simbolo = (ESimbolo)list[i + 1].Simbolo;
                AtomoConector item3 = list[i + 2].AtomoConectorProp.copy();

                Conector conector = new Conector(simbolo, item1, item3);
                list[i + 2].AtomoConectorProp = new AtomoConector(conector);

            }

            if (remover.Count > 0)
            {
                for (int i = remover.Count - 1; i >= 0; i--)
                {
                    list.RemoveAt(remover[i]);
                }
            }

            list = removerParentesisDuplos(list);
            list = tratarNegacoes(list);
            return remover.Count > 0 ? unirAtomoConector(list) : list;
        }

        private bool isNegacao(ItemList item) { return item != null && item.Negacao != null && !string.IsNullOrEmpty(item.Negacao); }
        private bool isAtomoConectorProp(ItemList item) { return item != null && item.AtomoConectorProp != null; }
        private bool isSimbolo(ItemList item) { return item != null && item.Simbolo != null; }

        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }

    }

    class ItemList
    {
        public AtomoConector? AtomoConectorProp { get; set; }
        public bool? Parentesis { get; set; } // true: (, false: )
        public ESimbolo? Simbolo { get; set; } // E, OU, IMPLICA
        public string? Negacao { get; set; }

        public ItemList()
        {
            AtomoConectorProp = null; Parentesis = null; Simbolo = null; Negacao = null;
        }

        public override string? ToString()
        {
            if (AtomoConectorProp != null) { return AtomoConectorProp.ToString(); }
            if (Simbolo != null) { return Auxiliar.toSimbolo(Simbolo); }
            if (Negacao != null && !string.IsNullOrEmpty(Negacao)) { return Negacao; }
            if (Parentesis != null) { return (bool)Parentesis ? "(" : ")"; }
            return "";
        }
    }

}