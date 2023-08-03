using System.Drawing;
using classes.auxiliar;
using classes.auxiliar.saidas.print;
using classes.auxiliar.valoracoes;
using classes.formulas;
using classes.parser;
using classes.solverstage;

namespace classes.testes.print.valoracoes
{

    public class TestesValoracoes : IDisposable
    {

        private Parser? parser = new();
        private Valoracoes val = new();

        public void teste1()
        {

            Formulas f;
            f = getFormulas1();

            //f = getFormulas2();
            //f = getFormulas3();
            f = getFormulas4();

            valorar(parser.parserCF("T A"));
            valorar(parser.parserCF("F C->B"));
            valorar(parser.parserCF("F AFG | FTG"));
            valorar(parser.parserCF("F C->B & C | D & (C->B & C | D) -> (C->B & C | D)"));
            p(); p("");

            valorar(f);
            //p(f.ToString());

        }

        public void teste2()
        {
            Formulas f;
            f = getFormulas1();

            //f = getFormulas2();
            //f = getFormulas3();
            //f = getFormulas4();

            p(f.ToString());


            // testes bifurcacoes
            p(string.Format("bifurcacoes: {0}", val.bifurcacoes(f)));
            p(string.Format("numeroAtomosLivres: {0}", val.numeroAtomosLivres(f)));

            Dictionary<string, int>? dicFAL = val.frequenciaAtomosConectores(f, false, false);
            foreach (KeyValuePair<string, int> entry in dicFAL)
            {
                p(string.Format("{0}: {1}", entry.Key, entry.Value));
            }
        }

        public void teste3()
        {
            Formulas f;
            f = getFormulas1();

            p(f.ToString());
            
            p(string.Format("numeroFormulas: {0}", val.numeroFormulas(f)));
            
            
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

        private void valorar(ConjuntoFormula? cf1)
        {
            p(string.Format("{0}: {1}", cf1, val.complexidade(cf1)));
        }

        private void valorar(Formulas? f)
        {
            p(f.ToString());
            p("" + val.complexidade(f));
        }

        public void Dispose()
        {
            parser = null;
            val = null;
        }

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion
        #endregion

    }

}