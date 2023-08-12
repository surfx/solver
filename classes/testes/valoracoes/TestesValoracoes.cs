using classes.auxiliar.formulas;
using classes.formulas;
using classes.parser;
using classes.solverstage;
using static classes.auxiliar.valoracoes.Valoracoes;
using static classes.auxiliar.formulas.UtilFormulas;

namespace classes.testes.print.valoracoes
{

    public class TestesValoracoes : IDisposable
    {

        private Parser? parser = new();

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
            p(string.Format("bifurcacoes: {0}", bifurcacoes(f)));
            p(string.Format("numeroAtomosLivres: {0}", numeroAtomosLivres(f)));

            Dictionary<string, int>? dicFAL = frequenciaAtomosConectores(f, false, false);
            foreach (KeyValuePair<string, int> entry in dicFAL) { p(string.Format("{0}: {1}", entry.Key, entry.Value)); }
        }

        public void teste3()
        {
            Formulas f;
            f = getFormulas1();
            //f = getFormulas2();
            //f = getFormulas3();
            //f = getFormulas4();

            p(f.ToString());

            p(string.Format("numeroFormulas: {0}", numeroFormulas(f)));
            p(string.Format("ramosAbertosEFechados: {0}", ramosAbertosEFechados(f)));
            p(string.Format("altura: {0}", altura(f)));

            p(string.Format("numeroConectores: {0}", numeroConectores(parser.parserCF("T A"))));
            p(string.Format("numeroConectores: {0}", numeroConectores(parser.parserCF("T A -> B"))));
            p(string.Format("numeroConectores: {0}", numeroConectores(parser.parserCF("F C->B & C | D"))));
            p(string.Format("numeroConectores: {0}", numeroConectores(f)));
            p(); p("");

            frequenciaAtomosAux(parser.parserCF("T A"));
            frequenciaAtomosAux(parser.parserCF("T A -> B"));
            frequenciaAtomosAux(parser.parserCF("F C->B & C | D"));

        }

        public void teste4()
        {
            Formulas f;
            f = getFormulas1();
            //f = getFormulas2();
            //f = getFormulas3();
            //f = getFormulas4();

            p(f.ToString());

            Dictionary<string, int>? dicFAL = frequenciaAtomosConectores(f, false, true);
            foreach (KeyValuePair<string, int> entry in dicFAL) { p(string.Format("{0}: {1}", entry.Key, entry.Value)); }
            p();

            frequenciaAtomosAux(parser.parserCF("T A"));
            frequenciaAtomosAux(parser.parserCF("T A -> B"));
            frequenciaAtomosAux(parser.parserCF("F C->B & C | D"));
            p(); p("");

            frequenciaAtomosAux2(f.Esquerda.LConjuntoFormula);
            p(); p("");

            frequenciaAtomosAux3(f);

        }

        public void teste5()
        {
            Formulas f;
            f = getFormulas1();
            //f = getFormulas2();
            //f = getFormulas3();
            //f = getFormulas4();

            p(f.ToString());

            frequenciaAtomosRelativoAux(f.LConjuntoFormula[1], f);
            p();
            frequenciaAtomosRelativoAux(f.Esquerda.Direita.LConjuntoFormula[0], f);

        }

        #region formulas
        private Formulas? getFormulas1()
        {
            if (parser == null) { return null; }
            Formulas f = new();

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
            Formulas f = new();

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
            Formulas f = new();

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
            Formulas f = new();

            f.addConjuntoFormula(parser.parserCF("T A"));
            f.addEsquerda(parser.parserCF("G"));
            f.addDireita(parser.parserCF("B"));

            f.Direita.addEsquerda(parser.parserCF("C"));
            f.Direita.Esquerda.addEsquerda(parser.parserCF("D"));

            f.Direita.addDireita(parser.parserCF("E"));
            return f;
        }
        #endregion

        #region aux

        private void valorar(ConjuntoFormula? cf)
        {
            p(string.Format("{0}: {1}", cf, complexidade(cf)));
        }

        private void valorar(Formulas? f)
        {
            p(f.ToString());
            p("" + complexidade(f));
        }

        private void frequenciaAtomosAux(ConjuntoFormula? cf)
        {
            if (cf == null) { return; }
            Dictionary<string, int>? aux = frequenciaAtomos(cf);
            if (aux == null || aux.Count <= 0) { return; }
            p(cf.ToString());
            foreach (KeyValuePair<string, int> entry in aux)
            {
                p(string.Format("{0}: {1}", entry.Key, entry.Value));
            }
            p();
        }

        private void frequenciaAtomosAux2(List<ConjuntoFormula> listaFormulas)
        {
            if (listaFormulas == null || listaFormulas.Count <= 0) { return; }
            Dictionary<string, int>? aux = frequenciaAtomos(listaFormulas);
            foreach (KeyValuePair<string, int> entry in aux)
            {
                p(string.Format("{0}: {1}", entry.Key, entry.Value));
            }
        }

        private void frequenciaAtomosAux3(Formulas f)
        {
            if (f == null) { return; }
            Dictionary<string, int>? aux = frequenciaAtomos(f);
            foreach (KeyValuePair<string, int> entry in aux)
            {
                p(string.Format("{0}: {1}", entry.Key, entry.Value));
            }
        }

        private void frequenciaAtomosRelativoAux(ConjuntoFormula cf, Formulas f)
        {
            if (cf == null || f == null) { return; }
            p(string.Format("Analisando: {0}", cf));
            Dictionary<string, StFrequenciaAtomosRelativos>? aux = frequenciaAtomosRelativo(cf, f);
            foreach (KeyValuePair<string, StFrequenciaAtomosRelativos> entry in aux)
            {
                p(string.Format("{0} | {1}", entry.Key, entry.Value.ToString()));
            }
        }

        public void Dispose()
        {
            parser = null;
        }
        #endregion

    }

}