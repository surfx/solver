using System.Text;
using classes.solverstage;

namespace classes.auxiliar.saidas.print_old
{
    [Obsolete]
    public class PrintFormulasOld
    {

        public void printTree(Formulas f, int level = 0, int pos = 0)
        {
            if (f == null || f.LConjuntoFormula == null) { return; }
            string espaco = level <= 0 ? "" : getEspaco(level);
            //p(string.Format("{0}{1} ({2} {3})", espaco, t, level, pos));

            if (f.LConjuntoFormula != null) f.LConjuntoFormula.ForEach(x => Console.WriteLine(string.Format("{0}{1}", espaco, x.ToString())));

            if (f.Esquerda != null) { printTree(f.Esquerda, level + 1, pos << 1); }
            if (f.Direita != null) { printTree(f.Direita, level + 1, (pos << 1) + 1); }
        }

        public string toString(Formulas f)
        {
            int maxElements = (int)Math.Pow(2, heightTree(f) - 1);
            Dictionary<int, Dictionary<int, PosElementOld<List<string>>>> dic = toDict(f, 0, 0, maxElements, 0);
            string rt = string.Empty;
            string[] aux = toString(dic, heightTreeFormulas(dic), ((int)Math.Pow(2, dic.Keys.Max()) * 2)).Split(Environment.NewLine);
            foreach (string st in aux)
            {
                if (string.IsNullOrEmpty(st.Trim())) { continue; }
                rt += st + Environment.NewLine;
            }
            return rt;
        }

        public string toString(Dictionary<int, Dictionary<int, PosElementOld<List<string>>>> dic, int heighttreeFormulas, int numeroMaximo)
        {
            if (dic == null || heighttreeFormulas <= 0 || numeroMaximo <= 0) { return ""; }

            // onde para cada item de heighttree existe uma lista de fórmulas positivas e negativas
            // p(string.Format("heighttreeFormulas: {0}, numeroMaximo: {1}", heighttreeFormulas, numeroMaximo));

            string[,] linhas = new string[heighttreeFormulas, numeroMaximo];
            for (int i = 0; i < heighttreeFormulas; i++) { for (int j = 0; j < numeroMaximo; j++) { linhas[i, j] = " "; } }


            Dictionary<int, PosElementOld<List<string>>>? dicFormulas = null;
            for (int i = 0; i < heighttreeFormulas; i++)
            {
                dicFormulas = dic.ContainsKey(i) ? dic[i] : null;
                for (int j = 0; j < numeroMaximo; j++)
                {
                    if (dicFormulas == null || !dicFormulas.ContainsKey(j)) { continue; }
                    PosElementOld<List<string>> posElement = dicFormulas[j];
                    List<string> lformulas = posElement.Elemento;

                    int pos = 0;
                    if (lformulas != null)
                    {
                        for (int k = 0; k < lformulas.Count(); k++)
                        {
                            linhas[i + (pos++) + posElement.Height, posElement.Posicao] = lformulas[k] == null ? "" : lformulas[k];
                        }
                    }
                }
            }

            Dictionary<int, int> dicMaxSizeColumn = new Dictionary<int, int>();
            List<int> skipJ = new List<int>();
            #region eliminar colunas vazias
            for (int j = 0; j < numeroMaximo; j++)
            {
                bool bSkipJ = true;
                for (int i = 0; i < heighttreeFormulas; i++)
                {
                    string valor = linhas[i, j].Trim();
                    if (!string.IsNullOrEmpty(valor))
                    {
                        bSkipJ = false;
                        if (!dicMaxSizeColumn.ContainsKey(j))
                        {
                            dicMaxSizeColumn.Add(j, valor.Length + 1);
                            continue;
                        }
                        else if (dicMaxSizeColumn[j] < valor.Length)
                        {
                            dicMaxSizeColumn.Remove(j);
                            dicMaxSizeColumn.Add(j, valor.Length + 1);
                        }
                        continue;
                    }
                }
                if (bSkipJ)
                {
                    skipJ.Add(j);
                }
            }
            #endregion

            StringBuilder rt = new StringBuilder();
            for (int i = 0; i < heighttreeFormulas; i++)
            {
                for (int j = 0; j < numeroMaximo; j++)
                {
                    if (skipJ.Contains(j) || !dicMaxSizeColumn.ContainsKey(j)) { continue; }
                    rt.Append(formatarStr(linhas[i, j], dicMaxSizeColumn[j])); //, !string.IsNullOrEmpty(linhas[i, j].Trim())
                }
                rt.AppendLine();
            }
            return rt.ToString();
        }

        #region Dictionary
        public Dictionary<int, Dictionary<int, PosElementOld<List<string>>>> toDict(Formulas f, int level, int pos, int maxElements, int height)
        {

            int nAux = level <= 1 ? maxElements : maxElements / level;
            int posMap = (level == 0 ? nAux : nAux / 2 + pos * nAux) - 1;

            Dictionary<int, Dictionary<int, PosElementOld<List<string>>>> rt = new Dictionary<int, Dictionary<int, PosElementOld<List<string>>>>();
            Dictionary<int, PosElementOld<List<string>>> aux = rt.ContainsKey(level) ? rt[level] : new Dictionary<int, PosElementOld<List<string>>>();

            List<string> lformulas = new();
            if (f.LConjuntoFormula != null)
            {
                lformulas.AddRange(f.LConjuntoFormula.Select(f => f.ToString()));
            }

            if (f.Esquerda == null && f.Direita == null)
            {
                lformulas.Add(f.isClosed ? "CLOSED" : "OPEN");
            }

            aux.Add(pos, new PosElementOld<List<string>>(lformulas, posMap, height));
            rt.Add(level, aux);

            height += lformulas.Count();
            if (height > 0) { height--; }

            //p(string.Format("{0}{1} ({2} {3} posMap: {4})", level <= 0 ? "" : getEspaco(level), t.Item, level, pos, posMap));
            if (f.Esquerda != null)
            {
                unirDicts(rt, toDict(f.Esquerda, level + 1, pos << 1, maxElements, height));
            }
            if (f.Direita != null)
            {
                unirDicts(rt, toDict(f.Direita, level + 1, (pos << 1) + 1, maxElements, height));
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
        #endregion

        #region privado
        private int heightTree(Formulas? f)
        {
            return f == null ? 0 : 1 + Math.Max(heightTree(f.Esquerda), heightTree(f.Direita));
        }

        private int heightTreeFormulas(Dictionary<int, Dictionary<int, PosElementOld<List<string>>>> dic)
        {
            int rt = 0;
            foreach (KeyValuePair<int, Dictionary<int, PosElementOld<List<string>>>> kvp in dic)
            {
                foreach (KeyValuePair<int, PosElementOld<List<string>>> kvp2 in kvp.Value)
                {
                    rt += kvp2.Value.Elemento.Count(); // .Where(x => !string.IsNullOrEmpty(x))
                }
            }

            return rt;
        }
        // private int heightTreeFormulas(Formulas? f)
        // {
        //     if (f == null) { return 0; }
        //     int aux = f.Negativas == null ? 0 : f.Negativas.Count;
        //     aux += f.Positivas == null ? 0 : f.Positivas.Count;
        //     return aux + Math.Max(heightTreeFormulas(f.Esquerda), heightTreeFormulas(f.Direita));
        // }

        private string getEspaco(int size)
        {
            return size <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", size));
        }

        private string formatarStr(string str, int sizeString = 0)
        {
            if (str == null || string.IsNullOrEmpty(str) || sizeString <= 0) { return str ?? ""; }
            str = str.Trim();
            if (sizeString <= 0 && str.Length >= sizeString) { return str; }
            bool left = true;
            while (str.Length < sizeString)
            {
                str = left ? " " + str : str + " ";
                left = false;
            }
            if (sizeString > 0 && str.Length > sizeString) { str = str.Substring(0, sizeString - 2) + (!string.IsNullOrEmpty(str) ? ".." : ""); }
            return str;
        }
        #endregion

    }
}