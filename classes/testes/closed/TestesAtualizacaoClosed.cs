
using classes.auxiliar;
using classes.auxiliar.saidas.print;
using classes.parser;
using classes.solverstage;

namespace classes.testes.closed
{
    public class TestesAtualizacaoClosed
    {

        public void testeClosed()
        {
            Formulas formulas = getFormulaT();
            //formulas = getFormulaABCDEFG();
            //formulas = getFormulaAB();
            //formulas = getFormulaAC();
            p(formulas.ToString()); p(); p("");

            PFormulasToImage.PFormulasToImageBuilder pf2imgBuilder = PFormulasToImage.PFormulasToImageBuilder.Init(formulas)
                    .SetPathImgSaida(string.Format(@"{0}\{1}", "imgformulas", "bmp_formula_1.png"))
                    .withDivisoriaArvore()
                    .withPrintAllClosedOpen();
            new ImageFormulas().formulasToImage(pf2imgBuilder.Build());

            updateClosed(formulas);

            //new classes.solverstage.print.PrintFormulas().printTree(formulas);

            pf2imgBuilder.SetPathImgSaida(string.Format(@"{0}\{1}", "imgformulas", "bmp_formula_2.png"));
            new ImageFormulas().formulasToImage(pf2imgBuilder.Build());
        }


        private void updateClosed(Formulas? formulas)
        {
            if (formulas == null) { return; }
            if (formulas.Esquerda != null) { updateClosed(formulas.Esquerda); }
            if (formulas.Direita != null) { updateClosed(formulas.Direita); }
            formulas.isClosed = isClosedFormula(formulas);

        }

        private bool isClosedFormula(Formulas? formulas)
        {
            // se é null, considero fechado
            if (formulas == null) { return true; }
            // não tem nem esquerda nem direita
            if (formulas.Esquerda == null && formulas.Direita == null) { return formulas.isClosed; }

            // para o ramo se considerado fechado, esquerda e direita, também devem estar
            // se um subramo estiver aberto, o ramo atual também está

            if (formulas.Esquerda != null && formulas.Direita != null)
            {
                return isClosedFormula(formulas.Esquerda) && isClosedFormula(formulas.Direita);
            }

            return formulas.Esquerda != null ? isClosedFormula(formulas.Esquerda) : isClosedFormula(formulas.Direita);

            // if (formulas.Esquerda != null) { formulas.Esquerda.isClosed = isClosedFormula(formulas.Esquerda.Esquerda) && isClosedFormula(formulas.Esquerda.Direita); }
            // if (formulas.Direita != null) { formulas.Direita.isClosed = isClosedFormula(formulas.Direita.Esquerda) && isClosedFormula(formulas.Direita.Direita); }
            // return isClosedFormula(formulas.Esquerda) && isClosedFormula(formulas.Direita);
        }

        #region fórmulas
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
            f.Esquerda.isClosed = false;
            //f.addEsquerda(parser.parserCF("T A->B"));


            //f.Esquerda.Negativas.ForEach(x => p(x.ToString()));
            //f.Esquerda.Positivas.ForEach(x => p(x.ToString()));

            f.addDireita(parser.parserCF("T H->G"));
            //f.addDireita(parser.parserCF("F (A|Z) & (C | D) -> J"));
            f.addDireita(parser.parserCF("F (A|Z)"));
            f.addDireita(parser.parserCF("T G|T&U"));
            f.addDireita(parser.parserCF("T G|T&X"));
            f.Direita.addEsquerda(parser.parserCF("T G|T&U"));
            f.Direita.Esquerda.isClosed = true;

            //f.Direita.Negativas.ForEach(x => p(x.ToString()));
            //f.Direita.Positivas.ForEach(x => p(x.ToString()));

            // TESTES
            f.Esquerda.addDireita(parser.parserCF("G & (Y -> B)"));
            f.Esquerda.addDireita(parser.parserCF("F G"));
            f.Esquerda.Direita.isClosed = false;

            f.Esquerda.addEsquerda(parser.parserCF("G & (Y -> B)"));
            f.Esquerda.Esquerda.isClosed = true;

            return f;
        }
        #endregion

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion

    }
}