using classes.auxiliar.formulas;
using classes.formulas;
using classes.parser;
using classes.regras;
using classes.regras.binarias;
using classes.regras.binarias.closed;
using classes.regras.unitarias;
using classes.regras.unitarias.unidouble;

namespace classes.testes.regras
{

    public class TestesRegras1
    {

        /* 
            F ¬A
            -----   (F ¬)
            T  A
        */
        public void testeRegraFalsoNegativo()
        {

            IRegraUnaria rfn = new RegraFalseNegativo();

            ConjuntoFormula cf1 = new ConjuntoFormula(false, null, new Atomo("A", 1));
            apply(rfn, cf1);
            p();

            ConjuntoFormula cf2 = new ConjuntoFormula(true, null, new Atomo("C", 1));
            apply(rfn, cf2);
            p();

            ConjuntoFormula cf5 = new ConjuntoFormula(false, null, new Conector(ESimbolo.E, new Conector(ESimbolo.IMPLICA, new Atomo("B"), new Atomo("C")), new Atomo("D"), 1));
            apply(rfn, cf5);
            p();
            p("");

            Parser parser = new Parser();
            ConjuntoFormula cf6 = parser.parserCF("F !!!!!A");
            apply(rfn, cf6);
            p("");

            apply(rfn, parser.parserCF("T !(A | B) -> G"));
            p("");

            apply(rfn, parser.parserCF("F !(A | B) -> G"));
            p("");


        }

        public void testeRegraTrueNegativo()
        {
            IRegraUnaria rtn = new RegraTrueNegativo();
            IRegraUnaria rfn = new RegraFalseNegativo();

            Parser parser = new Parser();
            ConjuntoFormula cf1 = parser.parserCF("F !!!!!A");

            apply(rfn, cf1);
            p("");
            apply(rtn, rfn.apply(cf1));
            p("");

            apply(rtn, parser.parserCF("T A"));
            p("");

            apply(rtn, parser.parserCF("T !!A"));
            p("");

            apply(rtn, parser.parserCF("F !A"));
            p("");

            apply(rtn, parser.parserCF("T !(A | B)"));
            p("");

            apply(rtn, parser.parserCF("F !(A | B)"));
            p("");

            // TODO: verificar essa regra no KEMS
            // T (¬(A ˅ B)) → G
            // ------ T ¬
            // F (A ˅ B) → G


            apply(rtn, parser.parserCF("T !(A | B) -> G"));
            p("");


            apply(rfn, parser.parserCF("F (A | B) -> !G"));
            p("");
        }

        /*
            T A → B
            T A
            ------- (T →1)
            T B
        */
        public void testeRegraTrueImplica1()
        {

            IRegraBinaria rti = new RegraTrueImplica1();

            // ESimbolo simbolo, AtomoConector esquerda, AtomoConector direita, bool negado = false
            Conector c1 = new Conector(ESimbolo.IMPLICA, new Atomo("A", 0), new Atomo("B", 0));

            // bool simbolo, IConversor conversor
            ConjuntoFormula cf1 = new ConjuntoFormula(true, null, c1);
            ConjuntoFormula cf2 = new ConjuntoFormula(true, null, new Atomo("A", 0));

            apply(rti, cf1, cf2);
            p(); p("");


            Conector c2 = new Conector(ESimbolo.OU, new Atomo("D", 1), new Atomo("G", 0));
            Conector c3 = new Conector(ESimbolo.IMPLICA, new Atomo("A", 0), c2);
            ConjuntoFormula cf5 = new ConjuntoFormula(true, null, c3);                                    // T A → (¬D v G)

            apply(rti, cf5, cf2);
            p(); p("");

            // T G → D
            Conector G_impl_D = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D"));     //  G → D
            Conector nD_ou_G = new Conector(ESimbolo.OU, new Atomo("D", 1), new Atomo("G"));        // ¬D v G
            Conector G_impl_D_impl_nD_ou_G = new Conector(ESimbolo.IMPLICA, G_impl_D, nD_ou_G);     // (G → D) → (¬D v G)
            ConjuntoFormula cf7 = new ConjuntoFormula(true, null, G_impl_D_impl_nD_ou_G);                 // T (G → D) → (¬D v G)
            ConjuntoFormula cf8 = new ConjuntoFormula(true, null, G_impl_D);                              // T G → D

            apply(rti, cf7, cf8);
            p(); p("");

            /*
            T G → D
            T ( G → D ) → (¬D v G)
            ------------------
            T ¬D v G
            */

            //---------------------------------------------
            Parser parser = new Parser();
            ConjuntoFormula cf9 = parser.parserCF("(G → D) → (¬D v G)");
            ConjuntoFormula cf10 = parser.parserCF("A");

            apply(rti, cf9, cf10);
            p(); p("");
            /*
            T G → D
            T ( G → D ) → (¬D v G)
            ------------------
            T ¬D v G
            */


            //Conector c1 = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D", true));

            Conector c3n = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D", 1));
            ConjuntoFormula cf1n = new ConjuntoFormula(true, null, c3.copy());
            Conector c2n = new Conector(ESimbolo.OU, new Atomo("D", 1), new Atomo("G"));
            ConjuntoFormula cf2n = new ConjuntoFormula(true, null, new Conector(ESimbolo.IMPLICA, c3.copy(), c2.copy()));

            apply(rti, cf1, cf2);
            p();

        }

        public void testeRegraTrueImplica2()
        {
            IRegraBinaria rti = new RegraTrueImplica2();
            Parser parser = new Parser();

            /*
                T A → B
                F B
                ------- (T →2)
                F A
            */
            ConjuntoFormula cf1 = parser.parserCF("A → B");
            ConjuntoFormula cf2 = parser.parserCF("B"); cf2.Simbolo = false;
            apply(rti, cf1, cf2); p(); p("");

            apply(rti, cf2, cf1); p(); p("");


            ConjuntoFormula cf3 = parser.parserCF("(G → D) → (¬D v G)");
            ConjuntoFormula cf4 = parser.parserCF("¬D v G"); cf4.Simbolo = false;
            apply(rti, cf3, cf4); p(); p("");

        }

        public void testeRegraFalseImplica()
        {
            IRegraUnariaDouble rfi = new RegraFalseImplica();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("F (G → D) → (¬D v G)");
            apply(rfi, cf1); p(); p("");

            ConjuntoFormula cf2 = parser.parserCF("F A → B");
            apply(rfi, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("A → B");
            apply(rfi, cf3); p(); p("");
        }

        public void testeRegraFalseE1()
        {
            IRegraBinaria rfe = new RegraFalseE1();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("F A & B");
            ConjuntoFormula cf2 = parser.parserCF("T A");
            apply(rfe, cf1, cf2); p(); p("");
            apply(rfe, cf2, cf1); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("F A → B & (G | Z)");
            ConjuntoFormula cf3 = parser.parserCF("A → B");
            apply(rfe, cf4, cf3); p(); p("");

        }

        public void testeRegraFalseE2()
        {
            IRegraBinaria rfe = new RegraFalseE2();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("F A & B");
            ConjuntoFormula cf2 = parser.parserCF("T B");
            apply(rfe, cf1, cf2); p(); p("");
            apply(rfe, cf2, cf1); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("F A → B & (G | Z)");
            ConjuntoFormula cf3 = parser.parserCF("T G | Z");
            apply(rfe, cf4, cf3); p(); p("");

        }

        public void testeRegraTrueOu1()
        {
            IRegraBinaria rto1 = new RegraTrueOu1();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("T A | B");
            ConjuntoFormula cf2 = parser.parserCF("F A");
            apply(rto1, cf1, cf2); p(); p("");
            apply(rto1, cf2, cf1); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("T A → B | (G | Z)");
            ConjuntoFormula cf3 = parser.parserCF("F A → B");
            apply(rto1, cf4, cf3); p(); p("");
        }

        public void testeRegraTrueOu2()
        {
            IRegraBinaria rto2 = new RegraTrueOu2();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("T A | B");
            ConjuntoFormula cf2 = parser.parserCF("F B");
            apply(rto2, cf1, cf2); p(); p("");
            apply(rto2, cf2, cf1); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("T A → B | (G | Z)");
            ConjuntoFormula cf3 = parser.parserCF("F G | Z");
            apply(rto2, cf4, cf3); p(); p("");
        }

        public void testeRegraTrueE()
        {
            IRegraUnariaDouble rte = new RegraTrueE();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("T A & B");
            apply(rte, cf1); p(); p("");

            ConjuntoFormula cf2 = parser.parserCF("F A & B");
            apply(rte, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("T (A -> B) & B");
            apply(rte, cf3); p(); p("");
        }

        public void testeRegraFalseOu()
        {
            IRegraUnariaDouble rfo = new RegraFalseOu();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("F A | B");
            apply(rfo, cf1); p(); p("");

            ConjuntoFormula cf2 = parser.parserCF("F A & B");
            apply(rfo, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("F (A -> B) | B");
            apply(rfo, cf3); p(); p("");
        }

        public void testeRegraClosed()
        {
            RegraClosed rc = new RegraClosed();
            Parser parser = new Parser();

            ConjuntoFormula cf1 = parser.parserCF("F A & B");
            ConjuntoFormula cf2 = parser.parserCF("T B");
            apply(rc, cf1, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("T A & B");
            apply(rc, cf1, cf3); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("F B");
            apply(rc, cf2, cf4); p(); p("");

            ConjuntoFormula cf5 = parser.parserCF("F (A -> B) | B & A → B | (G | Z)");
            apply(rc, cf2, cf5); p(); p("");

            ConjuntoFormula cf6 = parser.parserCF("T (A -> B) | B & A → B | (G | Z)");
            apply(rc, cf6, cf5); p(); p("");
            apply(rc, cf5, cf6); p(); p("");
            apply(rc, cf5, cf5); p(); p("");
            apply(rc, cf6, cf6); p(); p("");
        }

        #region apply rules

        private void apply(IRegraUnaria rUnaria, ConjuntoFormula cf1)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(string.Format("------ {0}", rUnaria.RULE));
            ConjuntoFormula? cfApply = rUnaria.apply(cf1);
            Console.WriteLine(cfApply == null ? "null" : cfApply);
        }

        private void apply(IRegraUnariaDouble rUnariaDouble, ConjuntoFormula cf1)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(string.Format("------ {0}", rUnariaDouble.RULE));
            ConjuntoFormula[]? cfs = rUnariaDouble.apply(cf1);
            Console.WriteLine(cfs != null ? string.Join(", ", cfs.Select(x => x.ToString())) : "null");
        }

        private void apply(IRegraBinaria rBinaria, ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(cf2);
            Console.WriteLine(string.Format("------ {0}", rBinaria.RULE));
            Console.WriteLine(rBinaria.apply(cf1, cf2));
        }

        private void apply(RegraClosed rc, ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(cf2);
            Console.WriteLine(string.Format("------ {0}", rc.RULE));
            Console.WriteLine(rc.apply(cf1, cf2));
        }

        #endregion

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion

    }

}