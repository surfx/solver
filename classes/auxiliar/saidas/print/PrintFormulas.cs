using System.Text;
using classes.formulas;
using classes.solverstage;

namespace classes.auxiliar.saidas.print
{
    public class PrintFormulas
    {

        public void printTree(PFormulasToString.PFormulasToStringBuilder paramBuilder)
        {
            string[,] matriz = getMatrix(paramBuilder);
            if (matriz == null) { return; }
            printMatrix(matriz);
        }

        public string toString(PFormulasToString.PFormulasToStringBuilder paramBuilder)
        {
            string[,] matriz = getMatrix(paramBuilder);
            return matriz == null ? "" : toStringMatrix(matriz);
        }

        private string[,] getMatrix(PFormulasToString.PFormulasToStringBuilder paramBuilder)
        {
            if (paramBuilder == null) { return null; }
            PFormulasToString param = paramBuilder.Build();
            if (param == null || param.Formulas == null) { return null; }

            // p(string.Format("treeHeight: {0}", treeHeight(paramBuilder)));
            // p(string.Format("numeroRamos: {0}", numeroRamos(f)));

            string[,] matriz = new string[treeHeight(paramBuilder), numeroRamos(param.Formulas)]; // treeHeight(f) x numeroRamos(f) colunas
            prencherMatriz(paramBuilder, matriz, 0, 0, false);
            ajustarMatrix(matriz);
            return matriz;
        }

        // ajusta a tree à matriz
        private void prencherMatriz(PFormulasToString.PFormulasToStringBuilder paramBuilder, string[,] matriz, int linha = 0, int coluna = 0, bool sum = false)
        {
            if (paramBuilder == null) { return; }
            PFormulasToString param = paramBuilder.Build();
            if (param == null || param.Formulas == null) { return; }

            // true	    coluna pai + esquerda + 1
            // false	coluna pai - direita - 1

            int resquerda = numeroRamos(param.Formulas.Esquerda);
            int rdireita = numeroRamos(param.Formulas.Direita);
            coluna = linha == 0 ? resquerda : (coluna + (sum ? resquerda + 1 : -rdireita - 1));
            coluna = coluna <= 0 ? 0 : coluna;

            int count = param.Formulas.LConjuntoFormula == null ? 0 : param.Formulas.LConjuntoFormula.Count;
            if (param.Formulas.LConjuntoFormula != null && count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    ConjuntoFormula cf = param.Formulas.LConjuntoFormula[i];
                    if (cf == null) { linha++; continue; }
                    //p(string.Format("{0} [{1},{2}]", cf, linha, coluna));
                    matriz[linha++, coluna] = cf.ToString();
                }
                if (param.PrintAllClosedOpen)
                {
                    matriz[linha++, coluna] = param.Formulas.isClosed ? "CLOSED" : "OPEN";
                }
            }

            if (param.Formulas.Direita == null && param.Formulas.Esquerda == null)
            {
                if (param.PrintLastClosedOpen)
                {
                    matriz[linha++, coluna] = param.Formulas.isClosed ? "CLOSED" : "OPEN";
                }
                return;
            }
            prencherMatriz(paramBuilder.copy(param.Formulas.Esquerda), matriz, linha, coluna, false);
            prencherMatriz(paramBuilder.copy(param.Formulas.Direita), matriz, linha, coluna, true);
        }

        // ajusta a largura das colunas da matriz
        private void ajustarMatrix(string[,] matriz)
        {
            int linhas = matriz.GetLength(0); // linhas
            int colunas = matriz.GetLength(1); // colunas
            for (int j = 0; j < colunas; j++)
            {
                int tamColuna = 0;
                for (int i = 0; i < linhas; i++)
                {
                    tamColuna = Math.Max(tamColuna, matriz[i, j] == null ? 0 : matriz[i, j].Length);
                }
                // ajustar as colunas
                for (int i = 0; i < linhas; i++)
                {
                    string valor = matriz[i, j] ?? "";
                    while (valor.Length < tamColuna) { valor += " "; }
                    matriz[i, j] = valor;
                }
            }
        }

        // imprime a matriz
        private void printMatrix(string[,] matriz)
        {
            if (matriz == null || matriz.Length <= 0) { return; }
            int length = matriz.Length;
            int linhas = matriz.GetLength(0); // linhas
            int colunas = matriz.GetLength(1); // colunas
            //p(string.Format("length: {0}, linhas: {1}, colunas: {2}", length, linhas, colunas));

            for (int i = 0; i < linhas; i++)
            {
                for (int j = 0; j < colunas; j++)
                {
                    //Console.Write(string.Format("| [{0},{1}]: {2} |", i, j, matriz[i, j]));
                    Console.Write(string.Format("{0}", matriz[i, j]));
                }
                Console.WriteLine();
            }
        }

        private string toStringMatrix(string[,] matriz)
        {
            if (matriz == null || matriz.Length <= 0) { return string.Empty; }
            int length = matriz.Length;
            int linhas = matriz.GetLength(0); // linhas
            int colunas = matriz.GetLength(1); // colunas
            //p(string.Format("length: {0}, linhas: {1}, colunas: {2}", length, linhas, colunas));

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < linhas; i++)
            {
                for (int j = 0; j < colunas; j++)
                {
                    //Console.Write(string.Format("| [{0},{1}]: {2} |", i, j, matriz[i, j]));
                    sb.Append(string.Format("{0}", matriz[i, j]));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private int treeHeight(PFormulasToString.PFormulasToStringBuilder paramBuilder)
        {
            if (paramBuilder == null) { return 0; }
            PFormulasToString param = paramBuilder.Build();
            if (param == null || param.Formulas == null) { return 0; }
            if (param.Formulas == null) { return 0; }
            //int rt = f.LConjuntoFormula == null ? 0 : (f.LConjuntoFormula.Count() + (f.LConjuntoFormula.Count() > 0 && addClosed ? 1 : 0));

            int rt = 0;
            int count = param.Formulas.LConjuntoFormula == null ? 0 : param.Formulas.LConjuntoFormula.Count;
            if (param.Formulas.LConjuntoFormula != null && count > 0)
            {
                rt += count + (param.PrintAllClosedOpen ? 1 : 0);

                for (int i = 0; i < count; i++)
                {
                    if (param.Formulas.LConjuntoFormula[i] == null) { rt++; continue; }
                }

            }

            if (param.Formulas.Direita == null && param.Formulas.Esquerda == null)
            {
                if (param.PrintLastClosedOpen)
                {
                    rt++;
                }
                return rt;
            }

            return rt + Math.Max(
                param.Formulas.Direita == null ? 0 : treeHeight(paramBuilder.copy(param.Formulas.Direita)),
                param.Formulas.Esquerda == null ? 0 : treeHeight(paramBuilder.copy(param.Formulas.Esquerda))
            );
        }

        private int numeroRamos(Formulas f)
        {
            if (f == null) { return 0; }
            if (f.Direita == null && f.Esquerda == null) { return 1; }
            int rt = 1;
            return rt + (f.Direita == null ? 0 : numeroRamos(f.Direita)) + (f.Esquerda == null ? 0 : numeroRamos(f.Esquerda));
        }

    }
}