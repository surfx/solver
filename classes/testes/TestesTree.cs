using System.Text;
using classes.auxiliar;
using classes.formulas;
using classes.parser;

namespace classes.testes
{

    public class TestesTree
    {

        public void teste1()
        {
            //testes com shift
            int n = 2;
            p(string.Format("{0}<<1: {1}, ({0}<<1)+1: {2}", n, n << 1, (n << 1) + 1));
        }

        public void teste2()
        {
            Tree treeR = new Tree("R");
            Tree treeA = new Tree("A");
            Tree treeB = new Tree("B");

            treeR.Esquerda = treeA;
            treeR.Direita = treeB;

            //-- A
            Tree treeA1 = new Tree("A1");
            Tree treeA2 = new Tree("A2");

            treeA.Esquerda = treeA1;
            treeA.Direita = treeA2;

            Tree treeA11 = new Tree("A11");
            Tree treeA12 = new Tree("A12");

            treeA1.Esquerda = treeA11;
            treeA1.Direita = treeA12;

            Tree treeA21 = new Tree("A21");
            Tree treeA22 = new Tree("A22");

            treeA2.Esquerda = treeA21;
            treeA2.Direita = treeA22;

            //-- B
            Tree treeB1 = new Tree("B1");
            Tree treeB2 = new Tree("B2");

            treeB.Esquerda = treeB1;
            treeB.Direita = treeB2;

            Tree treeB11 = new Tree("B11");
            Tree treeB12 = new Tree("B12");

            treeB1.Esquerda = treeB11;
            treeB1.Direita = treeB12;

            Tree treeB21 = new Tree("B21sdfasdf");
            Tree treeB22 = new Tree("B22");

            treeB2.Esquerda = treeB21;
            treeB2.Direita = treeB22;

            //pTree(treeR, 0);

            int maxElements = (int)Math.Pow(2, heightTree(treeR) - 1);
            p(string.Format("heightTree: {0}, maxElements: {1}", heightTree(treeR), maxElements));

            Dictionary<int, Dictionary<int, PosElement<Tree>>> dic = toDict(treeR, 0, 0, maxElements);

            // análise da estrutura de dicionário
            foreach (KeyValuePair<int, Dictionary<int, PosElement<Tree>>> kvp in dic)
            {
                p(string.Format("--- {0} ---", kvp.Key));
                foreach (KeyValuePair<int, PosElement<Tree>> kvp2 in kvp.Value)
                {
                    Console.Write(string.Format("  {0} {1} | ", kvp2.Key, kvp2.Value.ToString()));
                }
                Console.WriteLine();
            }

            // int size = dic.Keys.Max();
            // p(string.Format("size: {0}, maxElements: {1}", size, Math.Pow(2, size)));
            p();

            int minLStr = maxSizeString(treeR) + 1;
            int maxLStr = 4;

            // string fullPath = @"C:\Users\zero_\OneDrive\Área de Trabalho\tree.txt";
            // using (StreamWriter writer = new StreamWriter(fullPath)) {
            foreach (KeyValuePair<int, Dictionary<int, PosElement<Tree>>> kvp in dic)
            {
                string linha = toStrLineDic(kvp.Value, ((int)Math.Pow(2, dic.Keys.Max()) * 2), minLStr, maxLStr);
                p(linha);
                //writer.WriteLine(linha);
            }
            //}

            p();
            // int maxLStr = maxSizeString(treeR);
            // p("maxLStr: " + maxLStr);
            // p(formatarStr("E", maxLStr));
            // p(formatarStr("B", maxLStr));
            // p(formatarStr("C1A", maxLStr));


        }

        private string formatarStr(string str, int sizeMin, int sizeMax = 0)
        {
            if (str == null || string.IsNullOrEmpty(str)) { return str; }
            if (sizeMax <= 0 && str.Length >= sizeMin) { return str; }
            bool left = true;
            while (str.Length < sizeMin)
            {
                str = left ? " " + str : str + " ";
                left = false;
            }
            if (sizeMax > 0 && str.Length > sizeMax) { str = str.Substring(0, sizeMax - 2) + ".."; }
            return str;
        }

        private int heightTree(Tree? t)
        {
            return t == null ? 0 : 1 + Math.Max(heightTree(t.Esquerda), heightTree(t.Direita));
        }

        private int maxSizeString(Tree? t)
        {
            return t == null ? 0 : Math.Max(t.Item.Length, Math.Max(maxSizeString(t.Esquerda), maxSizeString(t.Direita)));
        }

        private string toStrLineDic(Dictionary<int, PosElement<Tree>> dic, int numeroMaximo, int minLStr, int sizeMax = 0)
        {
            if (dic == null || numeroMaximo <= 0) { return ""; }
            //if (minEspacos <= 0) { minEspacos = 3; }
            minLStr = minLStr <= sizeMax ? minLStr : sizeMax;
            int minEspacos = minLStr;

            string espaco = getEspaco(minEspacos);
            int numelementos = dic.Keys.Count();
            string chaves = string.Join(",", dic.Keys);

            string[] itens = new string[numeroMaximo];
            for (int i = 0; i < numeroMaximo; i++) { itens[i] = espaco; }

            for (int i = 0; i < numeroMaximo; i++)
            {
                if (!dic.ContainsKey(i)) { continue; }
                itens[dic[i].Posicao] = formatarStr(dic[i].Elemento.ToString(), minLStr, sizeMax);
            }

            StringBuilder rt = new StringBuilder();
            for (int i = 0; i < numeroMaximo; i++)
            {
                rt.Append(itens[i]);
            }

            //return string.Format("{0}\t\t{1} {2} [{3}]", rt.ToString(), numeroMaximo, numelementos, chaves);
            return rt.ToString();
        }


        private void pTree(Tree t, int level = 0, int pos = 0)
        {
            p(string.Format("{0}{1} ({2} {3})", level <= 0 ? "" : getEspaco(level), t.Item, level, pos));
            if (t.Esquerda != null) { pTree(t.Esquerda, level + 1, pos << 1); }
            if (t.Direita != null) { pTree(t.Direita, level + 1, (pos << 1) + 1); }
        }

        private Dictionary<int, Dictionary<int, PosElement<Tree>>> toDict(Tree t, int level, int pos, int maxElements)
        {

            int nAux = level <= 1 ? maxElements : maxElements / level;
            int posMap = level == 0 ? nAux : nAux / 2 + pos * nAux;

            Dictionary<int, Dictionary<int, PosElement<Tree>>> rt = new Dictionary<int, Dictionary<int, PosElement<Tree>>>();
            Dictionary<int, PosElement<Tree>> aux = rt.ContainsKey(level) ? rt[level] : new Dictionary<int, PosElement<Tree>>();
            aux.Add(pos, new PosElement<Tree>(t, posMap));
            rt.Add(level, aux);

            //p(string.Format("{0}{1} ({2} {3} posMap: {4})", level <= 0 ? "" : getEspaco(level), t.Item, level, pos, posMap));
            if (t.Esquerda != null)
            {
                unirDicts(rt, toDict(t.Esquerda, level + 1, pos << 1, maxElements));
            }
            if (t.Direita != null)
            {
                unirDicts(rt, toDict(t.Direita, level + 1, (pos << 1) + 1, maxElements));
            }
            return rt;
        }


        private void unirDicts<T>(Dictionary<int, Dictionary<int, T>> rt, Dictionary<int, Dictionary<int, T>> auxRt)
        {
            if (auxRt == null) { return; }

            foreach (KeyValuePair<int, Dictionary<int, T>> kvp in auxRt)
            {
                Dictionary<int, T> aux = rt.ContainsKey(kvp.Key) ? rt[kvp.Key] : new Dictionary<int, T>();
                foreach (KeyValuePair<int, T> kvp2 in kvp.Value)
                {
                    aux.Add(kvp2.Key, kvp2.Value);
                }
                if (rt.ContainsKey(kvp.Key)) { rt.Remove(kvp.Key); }
                rt.Add(kvp.Key, aux);
            }
        }


        class PosElement<T>
        {
            public T Elemento { get; set; }
            public int Posicao { get; set; }

            public PosElement(T treeProp, int posicao)
            {
                Elemento = treeProp;
                Posicao = posicao;
            }
            public override string? ToString()
            {
                return string.Format("{0} {1}", Elemento, Posicao);
            }
        }

        class Tree
        {
            public string Item { get; set; }
            public Tree? Esquerda { get; set; }
            public Tree? Direita { get; set; }

            public Tree(string item = "", Tree? esquerda = null, Tree? direita = null)
            {
                Item = item;
                Esquerda = esquerda;
                Direita = direita;
            }

            public override string? ToString()
            {
                return Item;
            }
        }



        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }

        private string getEspaco(int size)
        {
            return size <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", size));
        }

    }

}