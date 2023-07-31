using classes.auxiliar;
using classes.auxiliar.saidas.print;
using classes.formulas;
using classes.parser;
using classes.solverstage;

namespace classes.testes.print
{

    public class TestesPrint : IDisposable
    {

        private Parser? parser = new();

        public void teste1()
        {
            Formulas f = getFormulas1();
            saveImg(f);

            p(string.Format("treeHeight: {0}", treeHeight(f)));
            p(string.Format("numeroRamos: {0}", numeroRamos(f)));

            string[,] matriz = new string[treeHeight(f), numeroRamos(f)]; // treeHeight(f) x numeroRamos(f) colunas
            //printMatrix(matriz);

            prencherMatriz(f);

            //testesMatrizes();

        }

        int aux = 0;

        private void prencherMatriz(Formulas f, int linha = 0, int coluna = 0, bool sum = true)
        {
            //if (aux>=2){return;}
            if (f == null) { return; }

            int ramosEsquerda = f.Esquerda == null ? 0 : numeroRamos(f.Esquerda);

            // começa em zero
            int numeroRamosAux = numeroRamos(f.Esquerda); //: numeroRamos(f.Direita);
            p(string.Format("numeroRamos(f.Esquerda): {0}, numeroRamos(f.Direita): {1}", numeroRamos(f.Esquerda), numeroRamos(f.Direita)));
            coluna += numeroRamosAux;

            f.LConjuntoFormula.ForEach(cf =>
            {
                p(string.Format("numeroRamosAux: {0}, coluna: {1}, linha: {2} {3}", numeroRamosAux, coluna, linha, cf));
                linha++;
            });

            p(string.Format("numeroRamosAux: {0}, coluna: {1}, linha: {2}", numeroRamosAux, coluna, linha));
            p();

            aux++;
            //prencherMatriz(f.Esquerda, linha, coluna, false);
            prencherMatriz(f.Direita, linha, coluna, false);

        }

        private void testesMatrizes()
        {
            string[,] matriz = new string[2, 3]; // 2 x 3 - 2 linhas por 3 colunas
            matriz[0, 0] = "0,0";
            matriz[0, 1] = "0,1";
            matriz[0, 2] = "0,2";
            matriz[1, 0] = "1,0";
            matriz[1, 1] = "1,1";
            matriz[1, 2] = "1,2";

            printMatrix(matriz);
            p();

            matriz = new string[4, 3]; // 4 x 3 - 4 linhas por 3 colunas
            matriz[0, 0] = "0,0";
            matriz[0, 1] = "0,1";
            matriz[0, 2] = "0,2";
            matriz[1, 0] = "1,0";
            matriz[1, 1] = "1,1";
            matriz[1, 2] = "1,2";
            matriz[2, 0] = "2,0";
            matriz[2, 1] = "2,1";
            matriz[2, 2] = "2,2";
            matriz[3, 0] = "3,0";
            matriz[3, 1] = "3,1";
            matriz[3, 2] = "3,2";
            printMatrix(matriz);
            p();
        }

        private void printMatrix(string[,] matriz)
        {
            if (matriz == null || matriz.Length <= 0) { return; }
            int l = matriz.Length;
            int l1 = matriz.GetLength(0);
            int l2 = matriz.GetLength(1);
            p(string.Format("l: {0}, l1: {1}, l2: {2}", l, l1, l2));

            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    Console.Write(string.Format("| [{0},{1}]: {2} |", i, j, matriz[i, j]));
                }
                Console.WriteLine();
            }
        }

        private int treeHeight(Formulas f)
        {
            if (f == null) { return 0; }
            int rt = f.LConjuntoFormula == null ? 0 : f.LConjuntoFormula.Count();
            if (f.Direita == null && f.Esquerda == null)
            {
                return rt;
            }
            return rt + Math.Max(f.Direita == null ? 0 : treeHeight(f.Direita), f.Esquerda == null ? 0 : treeHeight(f.Esquerda));
        }

        private int numeroRamos(Formulas f)
        {
            if (f == null) { return 0; }
            if (f.Direita == null && f.Esquerda == null) { return 1; }
            int rt = 1;
            return rt + (f.Direita == null ? 0 : numeroRamos(f.Direita)) + (f.Esquerda == null ? 0 : numeroRamos(f.Esquerda));
        }

        private Formulas? getFormulas1()
        {
            if (parser == null) { return null; }
            Formulas f = new Formulas();

            f.addConjuntoFormula(parser.parserCF("T A"));
            f.addConjuntoFormula(parser.parserCF("T A -> B"));
            f.addConjuntoFormula(parser.parserCF("F B"));

            f.addEsquerda(parser.parserCF("F A -> B"));
            f.addEsquerda(parser.parserCF("T C"));

            f.Esquerda.addEsquerda(parser.parserCF("T C"));
            f.Esquerda.addEsquerda(parser.parserCF("F C"));
            f.Esquerda.addDireita(parser.parserCF("F C->B & C | D"));
            f.Esquerda.Direita.addEsquerda(parser.parserCF("T A"));
            f.Esquerda.Direita.addDireita(parser.parserCF("T B"));

            f.addDireita(parser.parserCF("F A | B"));
            f.addDireita(parser.parserCF("T C | A"));
            f.addDireita(parser.parserCF("T C | A -> B"));
            f.addDireita(parser.parserCF("T C"));

            f.Direita.addEsquerda(parser.parserCF("T C"));
            f.Direita.addEsquerda(parser.parserCF("F C"));
            f.Direita.addEsquerda(parser.parserCF("F C->B & C | D"));
            f.Direita.Esquerda.isClosed = true;

            return f;
        }

        #region aux
        public void Dispose()
        {
            parser = null;
        }

        #region img
        private void saveImg(Formulas formulas)
        {
            //p(formulas.ToString()); p(); p("");
            //new classes.solverstage.print.PrintFormulas().printTree(formulas);

            PFormulasToImage pf2img = PFormulasToImage.PFormulasToImageBuilder.Init(formulas)
                    .SetPathImgSaida(string.Format(@"{0}\{1}", "imgformulas", "bmp_formula_testes.png"))
                    .withDivisoriaArvore()
                    .withPrintAllClosedOpen()
                    .Build();
            new ImageFormulas().formulasToImage(pf2img);
        }
        #endregion

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion
        #endregion

    }

}