using System.Text;
using classes.auxiliar;
using classes.formulas;
using classes.parser;

namespace classes.testes
{

    public class TestesTreeList
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

            p("-- Root");
            f.Negativas.ForEach(x => p(x.ToString()));
            f.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Negativas), sizeMax(f.Positivas))));
            p();

            f.addEsquerda(parser.parserCF("E"));
            f.addEsquerda(parser.parserCF("F Y"));
            //f.addEsquerda(parser.parserCF("T A->B"));

            p("-- Esquerda");
            f.Esquerda.Negativas.ForEach(x => p(x.ToString()));
            f.Esquerda.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Esquerda.Negativas), sizeMax(f.Esquerda.Positivas))));
            p();

            f.addDireita(parser.parserCF("T H->G"));
            f.addDireita(parser.parserCF("F (A|Z) & (C | D) -> J"));
            f.addDireita(parser.parserCF("T G|T&U"));

            p("-- Direita");
            f.Direita.Negativas.ForEach(x => p(x.ToString()));
            f.Direita.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Direita.Negativas), sizeMax(f.Direita.Positivas))));
            p();

            p(string.Format("sizeMax: {0}", sizeMax(f)));

            //p();
            //pTree(f);

            // #--------------------- ANÁLISE --------------------
            int maxElements = (int)Math.Pow(2, heightTree(f) - 1);
            p(string.Format("heightTree: {0}, maxElements: {1}", heightTree(f), maxElements));

            Dictionary<int, Dictionary<int, PosElement<Formulas>>> dic = toDict(f, 0, 0, maxElements);
            // análise da estrutura de dicionário
            foreach (KeyValuePair<int, Dictionary<int, PosElement<Formulas>>> kvp in dic)
            {
                p(string.Format("--- {0} ---", kvp.Key));
                foreach (KeyValuePair<int, PosElement<Formulas>> kvp2 in kvp.Value)
                {
                    Console.Write(string.Format("  {0} {1} | ", kvp2.Key, kvp2.Value.ToString()));
                }
                Console.WriteLine();
            }

            // int size = dic.Keys.Max();
            // p(string.Format("size: {0}, maxElements: {1}", size, Math.Pow(2, size)));
            p();

            int minLStr = maxSizeString(f) + 1;
            int maxLStr = 4;

            // string fullPath = @"C:\Users\zero_\OneDrive\Área de Trabalho\tree.txt";
            // using (StreamWriter writer = new StreamWriter(fullPath)) {
            foreach (KeyValuePair<int, Dictionary<int, PosElement<Formulas>>> kvp in dic)
            {
                string linha = toStrLineDic(kvp.Value, ((int)Math.Pow(2, dic.Keys.Max()) * 2), minLStr, maxLStr);
                p(linha);
                //writer.WriteLine(linha);
            }
            //}

            p();

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

        #region analise
        // private string formatarStr(string str, int sizeMin, int sizeMax = 0)
        // {
        //     if (str == null || string.IsNullOrEmpty(str)) { return str; }
        //     if (sizeMax <= 0 && str.Length >= sizeMin) { return str; }
        //     bool left = true;
        //     while (str.Length < sizeMin)
        //     {
        //         str = left ? " " + str : str + " ";
        //         left = false;
        //     }
        //     if (sizeMax > 0 && str.Length > sizeMax) { str = str.Substring(0, sizeMax - 2) + ".."; }
        //     return str;
        // }

        private int heightTree(Formulas? t)
        {
            return t == null ? 0 : 1 + Math.Max(heightTree(t.Esquerda), heightTree(t.Direita));
        }

        private int maxSizeString(Formulas? t)
        {
            return t == null ? 0 : Math.Max(masSizeString(t.Negativas, t.Positivas), Math.Max(maxSizeString(t.Esquerda), maxSizeString(t.Direita)));
        }

        private int masSizeString(List<ConjuntoFormula> listaNegativas, List<ConjuntoFormula> listaPositivas)
        {
            int maxNegativas = listaNegativas == null ? 0 : listaNegativas.Max(x => x.ToString().Length);
            int maxPositivas = listaPositivas == null ? 0 : listaPositivas.Max(x => x.ToString().Length);
            return Math.Max(maxNegativas, maxPositivas);
        }

        private string toStrFormulas_firstlevel(Formulas? t, string espaco = "", int iPos = 0)
        {
            if (t == null || (t.Negativas == null && t.Positivas == null)) { return string.Empty; }
            if (!string.IsNullOrEmpty(espaco) && iPos > 0)
            {
                espaco = string.Concat(Enumerable.Repeat(espaco, iPos));
            }
            // TODO: revisar
            StringBuilder rt = new StringBuilder();
            if (t.Negativas != null)
            {
                t.Negativas.ForEach(x => rt.Append(string.Format("{0}{1}", espaco, x.ToString())));
            }
            if (t.Positivas != null)
            {
                t.Positivas.ForEach(x => rt.Append(string.Format("{0}{1}", espaco, x.ToString())));
            }
            return rt.ToString();
        }

        private string toStrLineDic(Dictionary<int, PosElement<Formulas>> dic, int numeroMaximo, int minLStr, int sizeMax = 0)
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
                // TODO: revisar
                //itens[dic[i].Posicao] = formatarStr(dic[i].Elemento.ToString(), minLStr, sizeMax);
                itens[dic[i].Posicao] = toStrFormulas_firstlevel(dic[i].Elemento, espaco, i);
            }

            StringBuilder rt = new StringBuilder();
            for (int i = 0; i < numeroMaximo; i++)
            {
                rt.Append(itens[i]);
            }

            //return string.Format("{0}\t\t{1} {2} [{3}]", rt.ToString(), numeroMaximo, numelementos, chaves);
            return rt.ToString();
        }


        private void pTree(Formulas f, int level = 0, int pos = 0)
        {
            if (f == null || (f.Positivas == null && f.Negativas == null)) { return; }
            string espaco = level <= 0 ? "" : getEspaco(level);
            //p(string.Format("{0}{1} ({2} {3})", espaco, t, level, pos));

            if (f.Positivas != null) f.Positivas.ForEach(x => p(string.Format("{0}{1}", espaco, x.ToString())));
            if (f.Negativas != null) f.Negativas.ForEach(x => p(string.Format("{0}{1}", espaco, x.ToString())));

            if (f.Esquerda != null) { pTree(f.Esquerda, level + 1, pos << 1); }
            if (f.Direita != null) { pTree(f.Direita, level + 1, (pos << 1) + 1); }
        }

        private Dictionary<int, Dictionary<int, PosElement<Formulas>>> toDict(Formulas t, int level, int pos, int maxElements)
        {

            int nAux = level <= 1 ? maxElements : maxElements / level;
            int posMap = level == 0 ? nAux : nAux / 2 + pos * nAux;

            Dictionary<int, Dictionary<int, PosElement<Formulas>>> rt = new Dictionary<int, Dictionary<int, PosElement<Formulas>>>();
            Dictionary<int, PosElement<Formulas>> aux = rt.ContainsKey(level) ? rt[level] : new Dictionary<int, PosElement<Formulas>>();
            aux.Add(pos, new PosElement<Formulas>(t, posMap));
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
        #endregion

        // private List<string>? toArrayStr(List<ConjuntoFormula> formulas)
        // {
        //     if (formulas == null || formulas.Count <= 0) { return null; }
        //     return formulas.Select(x => x.ToString()).ToList();
        // }

        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }

        private string getEspaco(int size)
        {
            return size <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", size));
        }

    }

}