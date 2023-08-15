using classes.formulas;
using classes.parser;
using classes.solverstage;
using classes.solverstage.estrategias.selecao;
using static classes.auxiliar.formulas.UtilFormulas;
using static classes.auxiliar.valoracoes.Valoracoes;

namespace classes.testes.print.valoracoes
{
    public class TestesSelecaoFormulas
    {

        public void teste1()
        {

            List<int> listaInteiros = new();
            for (int i = 0; i < 100; i++) { listaInteiros.Add(i); }

            //words.Sort((a, b) => a.Length.CompareTo(b.Length));


            listaInteiros.Sort((v1, v2) =>
            {
                // primeiro os primos, depois os pares
                // sendo os ímpares de forma decrescente, e os pares, de forma crescente
                if (v1 == v2 || v1 - v2 == 0) { return 0; }

                // ambos ímpares
                if (v1 % 2 != 0 && v2 % 2 != 0)
                {
                    return v1 == v2 ? 0 : v1 > v2 ? -1 : 1; // ímpar maior tem prioridade (-1) - ordem decrescente
                }
                if (v1 % 2 != 0)
                {
                    return v1 == v2 ? 0 : v1 > v2 ? -1 : 1; // ímpar (v1) tem prioridade sobre par (v2): (-1) - ordem decrescente
                }
                if (v2 % 2 != 0)
                {
                    return v1 == v2 ? 0 : v2 > v1 ? -1 : 1; // ímpar (v2) tem prioridade sobre par (v1): (-1) - ordem decrescente
                }
                return v1 == v2 ? 0 : v1 > v2 ? 1 : -1; // o menor par tem menor prioridade (1) - ordem crescente
            });

            p(string.Join(",", listaInteiros));

        }

        public void teste2()
        {
            Formulas f1 = getFormulas1();

            p(f1.ToString());
            p();

            SelecaoFormulas.ordernarMaiorTaxa(f1);

            frequenciaAtomosRelativoAux(f1.LConjuntoFormula[1], f1);
            p();
            frequenciaAtomosRelativoAux(f1.LConjuntoFormula[2], f1);
            p();

            f1.LConjuntoFormula.ForEach(f => { if (f != null) { p(f.ToString()); } });
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

        #region get formulas
        private Parser parser = new();
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

            //f.addConjuntoFormula(parser.parserCF("(A | C)"));
            f.addConjuntoFormula(parser.parserCF("T !E"));
            //f.addConjuntoFormula(parser.parserCF("F !B"));
            f.addConjuntoFormula(parser.parserCF("T B"));
            f.addConjuntoFormula(parser.parserCF("F C & D"));
            f.addConjuntoFormula(parser.parserCF("F !D"));
            f.addConjuntoFormula(parser.parserCF("F !A"));
            f.addConjuntoFormula(parser.parserCF("T (A | C -> E) | (!C | B & E)"));
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

    }
}