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
            Formulas f = new Formulas();

            Parser parser = new Parser();
            f.addConjuntoFormula(parser.parserCF("A->B"));
            f.addConjuntoFormula(parser.parserCF("F C->E"));
            f.addConjuntoFormula(parser.parserCF("C"));
            f.addConjuntoFormula(parser.parserCF("F A"));
            f.addConjuntoFormula(parser.parserCF("T (A | D) -> (C & D)"));

            f.Negativas.ForEach(x => p(x.ToString()));
            f.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Negativas), sizeMax(f.Positivas))));
            p();

            f.addEsquerda(parser.parserCF("E"));
            f.addEsquerda(parser.parserCF("F Y"));
            //f.addEsquerda(parser.parserCF("T A->B"));
            f.Esquerda.Negativas.ForEach(x => p(x.ToString()));
            f.Esquerda.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Esquerda.Negativas), sizeMax(f.Esquerda.Positivas))));
            p();

            f.addDireita(parser.parserCF("T H->G"));
            f.addDireita(parser.parserCF("F (A|Z) & (C | D) -> J"));
            f.addDireita(parser.parserCF("T G|T&U"));
            f.Direita.Negativas.ForEach(x => p(x.ToString()));
            f.Direita.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Direita.Negativas), sizeMax(f.Direita.Positivas))));
            p();

            p(string.Format("sizeMax: {0}", sizeMax(f)));
            p();
            p("");
            p();
            p("");
            p();
            p("");

            toArrayStr(f.Negativas).ForEach(p);
            toArrayStr(f.Positivas).ForEach(p);

            p();
            //tryToPrint(f);

            // TODO
            //--- List< List<string>, List<string>, ... >

            //p(toArrayStr(f.Negativas));
            //p(toArrayStr(f.Positivas));
        }

        public void teste2()
        {
            string[] valores1 = new string[] { "um", "dois", "tres" };
            string[] valores2 = new string[] { "$", "V", "A" };
            string[] valores3 = new string[] { "6", "8", "9" };

            List<List<string>> linha = new List<List<string>>();
            linha.Add(valores1.ToList());
            linha.Add(valores2.ToList());
            linha.Add(valores3.ToList());

            //linha.ForEach( x => x.ForEach(p) );

            //teste(valores1, valores2);

            ListStrings ls = new ListStrings();
            ls.add(valores1);
            ls.add(valores2);
            ls.add(valores3);

            p();
            ls.Linha.ForEach(x => x.ToList().ForEach(p));
        }

        public void teste3()
        {
            //testes com shift
            int n = 2;
            p(string.Format("{0}<<1: {1}, ({0}<<1)+1: {2}", n, n << 1, (n << 1) + 1));
        }

        public void teste4()
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

            Tree treeB21 = new Tree("B21");
            Tree treeB22 = new Tree("B22");

            treeB2.Esquerda = treeB21;
            treeB2.Direita = treeB22;

            //pTree(treeR, 0);

            int maxElements = (int)Math.Pow(2, heightTree(treeR) - 1);
            p(string.Format("heightTree: {0}, maxElements: {1}", heightTree(treeR), maxElements));

            Dictionary<int, Dictionary<int, TreePos>> dic = toDict2(treeR, 0, 0, maxElements);

            // análise da estrutura de dicionário
            // foreach (KeyValuePair<int, Dictionary<int, TreePos>> kvp in dic)
            // {
            //     p(string.Format("--- {0} ---", kvp.Key));
            //     foreach (KeyValuePair<int, TreePos> kvp2 in kvp.Value)
            //     {
            //         Console.Write(string.Format("  {0} {1} | ", kvp2.Key, kvp2.Value.ToString()));
            //     }
            //     Console.WriteLine();
            // }

            // int size = dic.Keys.Max();
            // p(string.Format("size: {0}, maxElements: {1}", size, Math.Pow(2, size)));
            // p();

            string fullPath = @"C:\Users\zero_\OneDrive\Área de Trabalho\testes\tree.txt";
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (KeyValuePair<int, Dictionary<int, TreePos>> kvp in dic)
                {
                    string linha = toStrLineDic(kvp.Value, ((int)Math.Pow(2, dic.Keys.Max()) * 2));
                    p(linha);
                    writer.WriteLine(linha);
                }
            }

        }

        private int heightTree(Tree? t)
        {
            return t == null ? 0 : 1 + Math.Max(heightTree(t.Esquerda), heightTree(t.Direita));
        }

        private string toStrLineDic(Dictionary<int, TreePos> dic, int numeroMaximo, int sizeMin = 4)
        {
            if (dic == null || numeroMaximo <= 0) { return ""; }
            if (sizeMin <= 0) { sizeMin = 3; }

            string espaco = getEspaco(sizeMin);
            int numelementos = dic.Keys.Count();
            string chaves = string.Join(",", dic.Keys);

            string[] itens = new string[numeroMaximo];
            for (int i = 0; i < numeroMaximo; i++) { itens[i] = espaco; }

            for (int i = 0; i < numeroMaximo; i++)
            {
                if (!dic.ContainsKey(i)) { continue; }
                itens[dic[i].Posicao] = dic[i].TreeProp.ToString();
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

        private Dictionary<int, Dictionary<int, Tree>> toDict(Tree t, int level = 0, int pos = 0)
        {
            Dictionary<int, Dictionary<int, Tree>> rt = new Dictionary<int, Dictionary<int, Tree>>();
            Dictionary<int, Tree> aux = rt.ContainsKey(level) ? rt[level] : new Dictionary<int, Tree>();
            aux.Add(pos, t);
            rt.Add(level, aux);

            p(string.Format("{0}{1} ({2} {3}", level <= 0 ? "" : getEspaco(level), t.Item, level, pos));
            if (t.Esquerda != null)
            {
                unirDicts(rt, toDict(t.Esquerda, level + 1, pos << 1));
            }
            if (t.Direita != null)
            {
                unirDicts(rt, toDict(t.Direita, level + 1, (pos << 1) + 1));
            }
            return rt;
        }

        private Dictionary<int, Dictionary<int, TreePos>> toDict2(Tree t, int level, int pos, int maxElements)
        {

            int nAux = level <= 1 ? maxElements : maxElements / level;
            int posMap = level == 0 ? nAux : nAux / 2 + pos * nAux;

            Dictionary<int, Dictionary<int, TreePos>> rt = new Dictionary<int, Dictionary<int, TreePos>>();
            Dictionary<int, TreePos> aux = rt.ContainsKey(level) ? rt[level] : new Dictionary<int, TreePos>();
            aux.Add(pos, new TreePos(t, posMap));
            rt.Add(level, aux);

            //p(string.Format("{0}{1} ({2} {3} posMap: {4})", level <= 0 ? "" : getEspaco(level), t.Item, level, pos, posMap));
            if (t.Esquerda != null)
            {
                unirDicts2(rt, toDict2(t.Esquerda, level + 1, pos << 1, maxElements));
            }
            if (t.Direita != null)
            {
                unirDicts2(rt, toDict2(t.Direita, level + 1, (pos << 1) + 1, maxElements));
            }
            return rt;
        }

        class TreePos
        {
            public Tree TreeProp { get; set; }
            public int Posicao { get; set; }

            public TreePos(Tree treeProp, int posicao)
            {
                TreeProp = treeProp;
                Posicao = posicao;
            }
            public override string? ToString()
            {
                return string.Format("{0} {1}", TreeProp, Posicao);
            }
        }

        private void unirDicts(Dictionary<int, Dictionary<int, Tree>> rt, Dictionary<int, Dictionary<int, Tree>> auxRt)
        {
            if (auxRt == null) { return; }

            foreach (KeyValuePair<int, Dictionary<int, Tree>> kvp in auxRt)
            {
                Dictionary<int, Tree> aux = rt.ContainsKey(kvp.Key) ? rt[kvp.Key] : new Dictionary<int, Tree>();
                foreach (KeyValuePair<int, Tree> kvp2 in kvp.Value)
                {
                    aux.Add(kvp2.Key, kvp2.Value);
                }
                if (rt.ContainsKey(kvp.Key)) { rt.Remove(kvp.Key); }
                rt.Add(kvp.Key, aux);
            }

        }

        private void unirDicts2(Dictionary<int, Dictionary<int, TreePos>> rt, Dictionary<int, Dictionary<int, TreePos>> auxRt)
        {
            if (auxRt == null) { return; }

            foreach (KeyValuePair<int, Dictionary<int, TreePos>> kvp in auxRt)
            {
                Dictionary<int, TreePos> aux = rt.ContainsKey(kvp.Key) ? rt[kvp.Key] : new Dictionary<int, TreePos>();
                foreach (KeyValuePair<int, TreePos> kvp2 in kvp.Value)
                {
                    aux.Add(kvp2.Key, kvp2.Value);
                }
                if (rt.ContainsKey(kvp.Key)) { rt.Remove(kvp.Key); }
                rt.Add(kvp.Key, aux);
            }

        }

        // private ListStrings? pTree(Tree t){
        //     ListStrings rt = new ListStrings();

        //     return null;
        // }


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


        class ListStrings
        {
            private List<List<string>> _linha = new List<List<string>>();
            public List<List<string>> Linha { get => _linha; }

            public void add(params string[][] vetor)
            {
                vetor.ToList().ForEach(x => _linha.Add(x.ToList()));
            }

            public void add(params List<string>[] vetor)
            {
                vetor.ToList().ForEach(_linha.Add);
            }
        }










        class Formulas
        {

            private List<ConjuntoFormula> _positivas, _negativas;
            public List<ConjuntoFormula> Positivas { get => _positivas; }
            public List<ConjuntoFormula> Negativas { get => _negativas; }

            private Formulas _esquerda, _direita;
            public Formulas Esquerda { get => _esquerda; }
            public Formulas Direita { get => _direita; }

            public Formulas()
            {

            }

            public void addConjuntoFormula(ConjuntoFormula cf)
            {
                if (cf.Simbolo)
                {
                    if (_positivas == null) { _positivas = new List<ConjuntoFormula>(); }
                    _positivas.Add(cf);
                }
                else
                {
                    if (_negativas == null) { _negativas = new List<ConjuntoFormula>(); }
                    _negativas.Add(cf);
                }
            }

            public void addEsquerda(ConjuntoFormula cf)
            {
                if (_esquerda == null) { _esquerda = new Formulas(); }
                _esquerda.addConjuntoFormula(cf);
            }

            public void addDireita(ConjuntoFormula cf)
            {
                if (_direita == null) { _direita = new Formulas(); }
                _direita.addConjuntoFormula(cf);
            }

        }


        private void tryToPrint(Formulas f, int maxSizeFormula = 0)
        {
            if (maxSizeFormula <= 0) { maxSizeFormula = sizeMax(f); }
            toArrayStr(f.Negativas).ForEach(x => p(string.Format("{0}{1}", maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / 2), x)));
            toArrayStr(f.Positivas).ForEach(x => p(string.Format("{0}{1}", maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / 2), x)));

            DoubleLists? dl = toListEsquerdaDireita(f);
            maxSizeFormula = Math.Max(sizeMax(f.Esquerda), sizeMax(f.Direita));
            List<string> laux = toListString(dl, maxSizeFormula);
            if (laux == null || laux.Count <= 0) { return; }
            laux.ForEach(p);



        }

        private List<string> toListString(DoubleLists? dl, int maxSizeFormula = 0)
        {
            if (dl == null) { return null; }
            if (dl.Lesquerda == null && dl.Ldireita == null) { return null; }
            List<string> lesquerda = dl.Lesquerda;
            List<string> ldireita = dl.Ldireita;

            int lE = lesquerda == null ? 0 : lesquerda.Count;
            int lD = ldireita == null ? 0 : ldireita.Count;
            int length = Math.Max(lE, lD);
            if (length <= 0) { return null; }
            List<string> rt = new List<string>(length);
            for (int i = 0; i < length; i++)
            {
                string itemE = i < lE ? lesquerda[i] : "";
                string itemD = i < lD ? ldireita[i] : "";
                rt.Add(string.Format("{0}{1}{2}{3}",
                    maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / (string.IsNullOrEmpty(itemD) ? 2 : 4)),
                    itemE,
                    maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / (string.IsNullOrEmpty(itemE) ? 2 : 4)),
                    itemD));
            }
            return rt;
        }

        private DoubleLists? toListEsquerdaDireita(Formulas f)
        {
            List<ConjuntoFormula> aux1 = new List<ConjuntoFormula>();
            if (f.Esquerda.Positivas != null) aux1.AddRange(f.Esquerda.Positivas);
            if (f.Esquerda.Negativas != null) aux1.AddRange(f.Esquerda.Negativas);

            List<ConjuntoFormula> aux2 = new List<ConjuntoFormula>();
            if (f.Direita.Positivas != null) aux2.AddRange(f.Direita.Positivas);
            if (f.Direita.Negativas != null) aux2.AddRange(f.Direita.Negativas);

            return toListEsquerdaDireita(aux1, aux2);
        }

        private DoubleLists? toListEsquerdaDireita(List<ConjuntoFormula>? esquerda, List<ConjuntoFormula>? direita)
        {
            if (esquerda == null && direita == null) { return null; }
            List<string> lesquerda = toArrayStr(esquerda);
            List<string> ldireita = toArrayStr(direita);
            return new DoubleLists(toArrayStr(esquerda), toArrayStr(direita));
        }

        class DoubleLists
        {
            public List<string>? Lesquerda { get; set; }
            public List<string>? Ldireita { get; set; }

            public DoubleLists(List<string>? lesquerda, List<string>? ldireita)
            {
                Lesquerda = lesquerda;
                Ldireita = ldireita;
            }
        }

        private int sizeMax(Formulas f)
        {
            if (f == null || (f.Negativas == null && f.Positivas == null && f.Direita == null && f.Esquerda == null)) { return 0; }
            return Math.Max(Math.Max(sizeMax(f.Negativas), sizeMax(f.Positivas)), Math.Max(sizeMax(f.Direita), sizeMax(f.Esquerda)));
        }

        private int sizeMax(List<ConjuntoFormula> formulas)
        {
            if (formulas == null || formulas.Count <= 0) { return 0; }
            return formulas.Select(x => x.ToString().Length).Max();
        }

        private List<string>? toArrayStr(List<ConjuntoFormula> formulas)
        {
            if (formulas == null || formulas.Count <= 0) { return null; }
            return formulas.Select(x => x.ToString()).ToList();
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