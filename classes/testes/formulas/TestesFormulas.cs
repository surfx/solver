using classes.auxiliar.formulas;
using classes.parser;
using classes.solverstage;
using static classes.auxiliar.formulas.UtilFormulas;

namespace classes.testes.formulas
{
    public class TestesFormulas
    {
        private Parser parser = new();

        public void teste1()
        {
            Formulas f1 = getFormulaABCDEFG();

            p(f1.ToString());
            p(); p("");

            Formulas f2 = new();
            f2.addConjuntoFormula(parser.parserCF("F R"));
            f2.addDireita(parser.parserCF("A"));
            f2.addEsquerda(parser.parserCF("B"));
            p(f2.ToString());
            p(); p("");


            // addFormula(f1, f2);
            // addFormula(f1, f2);
            p(f1.ToString());

        }


        // private void addFormula(Formulas f1, Formulas f2, bool esquerda = true)
        // {
        //     if (f1 == null || f2 == null || (f2.Positivas == null && f2.Negativas == null))
        //     {
        //         return;
        //     }

        //     f2?.Positivas?.ForEach(x =>
        //     {
        //         if (x == null) { return; }
        //         if (f1.Esquerda == null) { f1.inicializarEsquerda(); }
        //         if (f1.Direita == null) { f1.inicializarDireita(); }
        //         if (esquerda) { f1.addEsquerda(x); } else { f1.addDireita(x); }
        //     });
        //     f2?.Negativas?.ForEach(x =>
        //     {
        //         if (x == null) { return; }
        //         if (f1.Esquerda == null) { f1.inicializarEsquerda(); }
        //         if (f1.Direita == null) { f1.inicializarDireita(); }
        //         if (esquerda) { f1.addEsquerda(x); } else { f1.addDireita(x); }
        //     });

        //     if (f2.Esquerda != null)
        //     {
        //         if (f1.Esquerda == null) { f1.inicializarEsquerda(); }
        //         f1.Esquerda.inicializarEsquerda();
        //         addFormula(f1.Esquerda, f2.Esquerda, true);
        //     }

        //     if (f2.Direita != null)
        //     {
        //         if (f1.Direita == null) { f1.inicializarDireita(); }
        //         f1.Direita.inicializarDireita();
        //         addFormula(f1.Direita, f2.Direita, true);
        //     }
        // }


        private Formulas getFormulaABCDEFG()
        {
            Formulas f = new();

            f.addConjuntoFormula(parser.parserCF("A"));
            f.addEsquerda(parser.parserCF("B"));
            f.addDireita(parser.parserCF("E"));

            f.Esquerda.addEsquerda(parser.parserCF("C"));
            f.Esquerda.addDireita(parser.parserCF("D"));

            f.Direita.addEsquerda(parser.parserCF("F"));
            f.Direita.addDireita(parser.parserCF("G"));

            return f;
        }


    }
}