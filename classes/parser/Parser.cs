using classes.auxiliar;
using classes.formulas;

namespace classes.parser
{
    // Parser para estrutura de dados ConjuntoFormula
    public class Parser
    {

        private string[] conectores = Auxiliar.getSimbolos(false);
        private const string simboloNegado = Auxiliar.SimboloNegado;

        public ConjuntoFormula? parserCF(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }
            entrada = UtilFormulas.sanitizar(entrada);

            // OBS: se a entrada começa com T ou F, será o símbolo. Se não, considero T

            bool simbolo = true;
            // Length: 1 e o átomo é 'F' ou 'T'
            if (entrada.Length == 1 && entrada.ToUpper().Equals("F") || entrada.ToUpper().Equals("T"))
            {
                simbolo = true; // como não sei o simbolo, seta True
            }
            else
            {
                simbolo = !entrada.Substring(0, 1).ToUpper().Equals("F");
                if (entrada.Substring(0, 1).ToUpper().Equals("T") || entrada.Substring(0, 1).ToUpper().Equals("F"))
                {
                    entrada = entrada.Substring(1);
                }
            }

            List<ItemList>? list = unirAtomoConector(entrada);
            if (list == null || list.Count <= 0 || list.Count > 1 || !isAtomoConectorProp(list[0]))
            {
                Console.WriteLine(string.Format("-- erro ao converter a entrada: {0}", entrada));
                return null;
            }

            AtomoConector? ac = list[0].AtomoConectorProp;
            return ac == null ? null : new ConjuntoFormula(simbolo, ac);
        }

        // converte cada item da entrada em uma estrutura (ItemList)
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
            if (list == null || list.Count <= 1) { return list; }

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
            if (list == null || list.Count <= 1) { return list; }
            list = removerParentesisDuplos(list);

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

        private List<ItemList> unirAtomoConector(string entrada)
        {
            if (entrada == null || string.IsNullOrEmpty(entrada)) { return null; }
            return unirAtomoConector(toList(entrada));
        }

        private List<ItemList> unirAtomoConector(List<ItemList> list)
        {
            if (list == null || list.Count <= 1) { return list; }
            list = removerParentesisDuplos(list);
            list = tratarNegacoes(list);

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

    }
}