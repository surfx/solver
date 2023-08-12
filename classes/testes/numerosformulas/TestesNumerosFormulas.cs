using classes.auxiliar.formulas;
using classes.auxiliar.saidas.print;
using classes.parser;
using classes.solverstage;
using static classes.auxiliar.formulas.UtilFormulas;

namespace classes.testes.print.numerosformulas
{

    public class TestesNumerosFormulas : IDisposable
    {

        private Parser? parser = new();

        public void teste1()
        {

            Formulas f;
            f = getFormulas1();

            PFormulasToString.PFormulasToStringBuilder paramBuilder = PFormulasToString.PFormulasToStringBuilder.Init(f).withPrintLastClosedOpen().withPrintFormulaNumber();
            p(new PrintFormulas().toString(paramBuilder)); // p(f.ToString()); 
            p(); p("");

            f.updateNumeroFormulas();
            printFormulas(f);
        }


        private void printFormulas(Formulas f, string space = "")
        {
            if (f == null) { return; }
            f.LConjuntoFormula.ForEach(cf => p(string.Format("{0}{1} {2}", space, cf.NumeroFormula, cf)));
            if (f.Esquerda != null) { printFormulas(f.Esquerda, space + " "); }
            if (f.Direita != null) { printFormulas(f.Direita, space + " "); }
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

        private Formulas? getFormulas2()
        {
            if (parser == null) { return null; }
            Formulas f = new Formulas();

            f.addConjuntoFormula(parser.parserCF("T A"));
            f.addEsquerda(parser.parserCF("B"));
            f.addDireita(parser.parserCF("C"));

            f.Esquerda.addEsquerda(parser.parserCF("D"));
            f.Esquerda.Esquerda.addEsquerda(parser.parserCF("J"));
            f.Esquerda.Esquerda.addDireita(parser.parserCF("K"));

            f.Esquerda.addDireita(parser.parserCF("E"));

            f.Esquerda.Direita.addEsquerda(parser.parserCF("H"));
            f.Esquerda.Direita.addDireita(parser.parserCF("I"));

            f.Direita.addEsquerda(parser.parserCF("F"));
            f.Direita.Esquerda.addEsquerda(parser.parserCF("L"));
            f.Direita.Esquerda.addDireita(parser.parserCF("M"));

            f.Direita.addDireita(parser.parserCF("G"));
            f.Direita.Direita.addEsquerda(parser.parserCF("N"));
            f.Direita.Direita.addDireita(parser.parserCF("O"));

            return f;
        }

        private Formulas? getFormulas3()
        {
            if (parser == null) { return null; }
            Formulas f = new Formulas();

            f.addConjuntoFormula(parser.parserCF("T A"));
            f.addDireita(parser.parserCF("B"));

            f.Direita.addEsquerda(parser.parserCF("C"));
            f.Direita.Esquerda.addEsquerda(parser.parserCF("D"));

            f.Direita.addDireita(parser.parserCF("E"));
            return f;
        }

        private Formulas? getFormulas4()
        {
            if (parser == null) { return null; }
            Formulas f = new Formulas();

            f.addConjuntoFormula(parser.parserCF("T A"));
            f.addEsquerda(parser.parserCF("G"));
            f.addDireita(parser.parserCF("B"));

            f.Direita.addEsquerda(parser.parserCF("C"));
            f.Direita.Esquerda.addEsquerda(parser.parserCF("D"));

            f.Direita.addDireita(parser.parserCF("E"));
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

            PFormulasToImage.PFormulasToImageBuilder pf2img = PFormulasToImage.PFormulasToImageBuilder.Init(formulas)
                    .SetPathImgSaida(string.Format(@"{0}\{1}", "imgformulas", "bmp_formula_testes.png"))

                    .withDivisoriaArvore()
                    //.withDivisoria() // difere da divisória de árvore
                    .withPrintAllClosedOpen()
                    .withPrintFormulaNumber()

                    //.modoDotTreeMode()
                    ;
            new ImageFormulas(pf2img).formulasToImage();
        }
        #endregion


        #endregion

    }

}