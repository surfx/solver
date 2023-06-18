using System.Text;
using classes.auxiliar;
using classes.formulas;
using classes.parser;
using classes.solverstage;

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
            //f.addConjuntoFormula(parser.parserCF("T (A | D) -> (C & D)"));
            f.addConjuntoFormula(parser.parserCF("T (A | D)"));

            p("-- Root");
            f.Negativas.ForEach(x => p(x.ToString()));
            f.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Negativas), sizeMax(f.Positivas))));
            p();

            f.addEsquerda(parser.parserCF("E"));
            f.addEsquerda(parser.parserCF("F Y -> (A | B)"));
            //f.addEsquerda(parser.parserCF("T A->B"));

            p("-- Esquerda");
            f.Esquerda.Negativas.ForEach(x => p(x.ToString()));
            f.Esquerda.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Esquerda.Negativas), sizeMax(f.Esquerda.Positivas))));
            p();

            f.addDireita(parser.parserCF("T H->G"));
            //f.addDireita(parser.parserCF("F (A|Z) & (C | D) -> J"));
            f.addDireita(parser.parserCF("F (A|Z)"));
            f.addDireita(parser.parserCF("T G|T&U"));

            p("-- Direita");
            f.Direita.Negativas.ForEach(x => p(x.ToString()));
            f.Direita.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Direita.Negativas), sizeMax(f.Direita.Positivas))));

            // TESTES
            f.Esquerda.addDireita(parser.parserCF("G & (Y -> B)"));

            p();

            p(string.Format("sizeMax: {0}", sizeMax(f)));

            //p();
            //pTree(f);

            // f = new Formulas();
            // f.addConjuntoFormula(parser.parserCF("A->B"));
            // f.addConjuntoFormula(parser.parserCF("F C->E"));
            // f.addConjuntoFormula(parser.parserCF("C"));
            // f.addConjuntoFormula(parser.parserCF("F A"));

            // #--------------------- ANÁLISE --------------------
            PrintFormulas pf = new PrintFormulas();

            int heighttree = heightTree(f);
            int maxElements = (int)Math.Pow(2, heighttree - 1);
            p(string.Format("heightTree: {0}, maxElements: {1}", heighttree, maxElements));

            Dictionary<int, Dictionary<int, PosElement<Formulas>>> dic = pf.toDict(f, 0, 0, maxElements, 0);
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

            // // int size = dic.Keys.Max();
            // // p(string.Format("size: {0}, maxElements: {1}", size, Math.Pow(2, size)));
            p();

            int minLStr = maxSizeString(f) + 1;
            int maxLStr = minLStr;

            p();

            

            string fullPath = @"C:\Users\zero_\OneDrive\Área de Trabalho\tree.txt";
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                string treeStre = pf.toString(f);
                p(treeStre);
                writer.WriteLine(treeStre);
            }

            // p();

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


        private int heightTree(Formulas? f)
        {
            return f == null ? 0 : 1 + Math.Max(heightTree(f.Esquerda), heightTree(f.Direita));
        }



        private int maxSizeString(Formulas? f)
        {
            return f == null ? 0 : Math.Max(masSizeString(f.Negativas, f.Positivas), Math.Max(maxSizeString(f.Esquerda), maxSizeString(f.Direita)));
        }

        private int masSizeString(List<ConjuntoFormula> listaNegativas, List<ConjuntoFormula> listaPositivas)
        {
            int maxNegativas = listaNegativas == null ? 0 : listaNegativas.Max(x => x.ToString().Length);
            int maxPositivas = listaPositivas == null ? 0 : listaPositivas.Max(x => x.ToString().Length);
            return Math.Max(maxNegativas, maxPositivas);
        }

        private int masSizeStringLevelOnly(Formulas? f)
        {
            if (f == null) { return 0; }
            return Math.Max(f.Negativas == null ? 0 : f.Negativas.Max(x => x == null ? 0 : x.ToString().Length), f.Positivas == null ? 0 : f.Positivas.Max(x => x == null ? 0 : x.ToString().Length));
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



    }

}