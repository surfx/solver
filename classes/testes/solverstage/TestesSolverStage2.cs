using classes.auxiliar;
using classes.auxiliar.formulas;
using classes.auxiliar.saidas.print;
using classes.parser;
using classes.solverstage;

namespace classes.testes.solverstage
{

    public class TestesSolverStage2
    {

        private Stage stage = new();

        public void teste1()
        {
            Formulas f;
            f = getFormulas1();
            //f = getFormulas2();
            // f = getFormulas3();
            //f = getFormulas4();
            //f = getFormulas5();
            //f = getFormulas6();

            //p(f.ToString()); p(); p("");

            System.Diagnostics.Stopwatch? sw = Util.CountTimer(() => stage.solve(f));
            Util.print(sw);
            saveImg(f);
            f.Dispose();
            stage.Dispose();
        }


        #region get formulas
        private Formulas getFormulas1()
        {
            Formulas f = new();

            Parser parser = new();
            // f.addConjuntoFormula(parser.parserCF("A->!B"));
            // f.addConjuntoFormula(parser.parserCF("F C->E"));
            // f.addConjuntoFormula(parser.parserCF("C"));
            // f.addConjuntoFormula(parser.parserCF("T !A"));
            // f.addConjuntoFormula(parser.parserCF("F A ˅ B"));
            // f.addConjuntoFormula(parser.parserCF("T (A | D) -> (C & D)"));
            f.addConjuntoFormula(parser.parserCF("T (A | C -> E) | (!C | B & E)"));
            //f.addConjuntoFormula(parser.parserCF("(A | C)"));
            f.addConjuntoFormula(parser.parserCF("T !E"));
            //f.addConjuntoFormula(parser.parserCF("F !B"));
            f.addConjuntoFormula(parser.parserCF("T B"));
            f.addConjuntoFormula(parser.parserCF("F C & D"));
            f.addConjuntoFormula(parser.parserCF("F !D"));
            f.addConjuntoFormula(parser.parserCF("F !A"));
            //f.addConjuntoFormula(parser.parserCF("F !!(B -> ( (G -> !(B -> E)) ) -> ((!D -> G) & A))"));

            //f.addConjuntoFormula(parser.parserCF("F A | C"));

            // f.addConjuntoFormula(parser.parserCF("F A ˅ B"));
            // f.addConjuntoFormula(parser.parserCF("F !B"));
            return f;
        }

        private Formulas getFormulaT()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A->B"));
            f.addConjuntoFormula(parser.parserCF("F C->E"));
            f.addConjuntoFormula(parser.parserCF("C"));
            f.addConjuntoFormula(parser.parserCF("F A"));
            //f.addConjuntoFormula(parser.parserCF("T (A | D) -> (C & D)"));
            f.addConjuntoFormula(parser.parserCF("T (A | D)"));


            //f.Negativas.ForEach(x => p(x.ToString()));
            //f.Positivas.ForEach(x => p(x.ToString()));

            f.addEsquerda(parser.parserCF("E"));
            f.addEsquerda(parser.parserCF("F Y -> (A | B)"));
            //f.addEsquerda(parser.parserCF("T A->B"));


            //f.Esquerda.Negativas.ForEach(x => p(x.ToString()));
            //f.Esquerda.Positivas.ForEach(x => p(x.ToString()));

            f.addDireita(parser.parserCF("T H->G"));
            //f.addDireita(parser.parserCF("F (A|Z) & (C | D) -> J"));
            f.addDireita(parser.parserCF("F (A|Z)"));
            f.addDireita(parser.parserCF("T G|T&U"));
            f.addDireita(parser.parserCF("T G|T&X"));
            f.Direita.addEsquerda(parser.parserCF("T G|T&U"));

            //f.Direita.Negativas.ForEach(x => p(x.ToString()));
            //f.Direita.Positivas.ForEach(x => p(x.ToString()));

            // TESTES
            f.Esquerda.addDireita(parser.parserCF("G & (Y -> B)"));
            f.Esquerda.addDireita(parser.parserCF("F G"));

            f.Esquerda.addEsquerda(parser.parserCF("G & (Y -> B)"));

            f.Esquerda.Direita.isClosed = true;

            return f;
        }

        private Formulas getFormulaABCDEFG()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            f.addEsquerda(parser.parserCF("B"));
            f.Esquerda.addConjuntoFormula(parser.parserCF("J->G"));

            f.addDireita(parser.parserCF("E"));

            f.Esquerda.addEsquerda(parser.parserCF("C"));
            f.Esquerda.addDireita(parser.parserCF("D"));

            f.Esquerda.Direita.isClosed = true;

            f.Direita.addEsquerda(parser.parserCF("F"));
            f.Direita.addDireita(parser.parserCF("G"));
            //f.Direita.Direita.addEsquerda(parser.parserCF("GT"));


            f.Direita.Direita.isClosed = true;

            return f;
        }

        private Formulas getFormulaAB()
        {
            Formulas f = new();
            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A"));
            f.addEsquerda(parser.parserCF("B"));

            f.Esquerda.isClosed = true;
            return f;
        }

        private Formulas getFormulaAC()
        {
            Formulas f = new();
            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A"));
            f.addDireita(parser.parserCF("C"));

            f.Direita.isClosed = true;
            return f;
        }

        private Formulas getFormulas2()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("! (A->B)"));
            f.addConjuntoFormula(parser.parserCF("T A ˅ B"));
            f.addConjuntoFormula(parser.parserCF("F A"));
            return f;
        }

        private Formulas getFormulas3()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("T (A | B)"));
            f.addConjuntoFormula(parser.parserCF("F (D & B) -> C"));
            return f;
        }

        private Formulas getFormulas4()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("T (A | B)"));
            f.addConjuntoFormula(parser.parserCF("T !A"));
            return f;
        }

        private Formulas getFormulas5()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("T A"));
            f.addConjuntoFormula(parser.parserCF("T A -> B"));
            f.addConjuntoFormula(parser.parserCF("F B"));
            return f;
        }

        private Formulas getFormulas6()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("F (p -> (q -> r) -> ( (p -> q) -> (p -> r) ))"));
            f.addConjuntoFormula(parser.parserCF("T p -> (q -> r)"));
            f.addConjuntoFormula(parser.parserCF("F (p -> q) -> (p -> r)"));
            return f;
        }
        #endregion

        #region img
        private void saveImg(Formulas formulas)
        {
            //p(formulas.ToString()); p(); p("");
            //new classes.solverstage.print.PrintFormulas().printTree(formulas);

            PFormulasToImage.PFormulasToImageBuilder pf2img = PFormulasToImage.PFormulasToImageBuilder.Init(formulas)
                    .SetPathImgSaida(string.Format(@"{0}\{1}", "imgformulas", "bmp_formula.png"))
                    .withDivisoriaArvore()
                    .withPrintAllClosedOpen()
                    .withPrintFormulaNumber()
                    ;
            new ImageFormulas(pf2img).formulasToImage();
        }
        #endregion

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion

    }

}