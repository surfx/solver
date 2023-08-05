using classes.auxiliar.formulas;
using classes.formulas;
using classes.parser;
using classes.regras;
using classes.regras.binarias.closed;
using classes.regras.unitarias.unidouble.beta;

namespace classes.testes.regras
{
    public class TestesBetaRegras3
    {
        private Parser parser = new Parser();

        public void testeRegraPB()
        {
            IRegraUnariaDouble rpb = new RegraPB();

            ConjuntoFormula cf1 = parser.parserCF("F A | B");
            apply(rpb, cf1); p(); p("");

            ConjuntoFormula cf2 = parser.parserCF("F A & B");
            apply(rpb, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("F (A -> B) | B");
            apply(rpb, cf3); p(); p("");
        }

        public void testeRegraFalsoE()
        {
            IRegraUnariaDouble rud = new RegraFalsoE();

            ConjuntoFormula cf1 = parser.parserCF("F A ˄ B");
            apply(rud, cf1); p(); p("");

            ConjuntoFormula cf2 = parser.parserCF("F A | B");
            apply(rud, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("F (A & B) & (C -> D)");
            apply(rud, cf3); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("F (A & B) -> (C -> D)");
            apply(rud, cf4); p(); p("");

        }

        public void testeRegraTrueOu()
        {
            IRegraUnariaDouble rtou = new RegraTrueOu();

            ConjuntoFormula cf1 = parser.parserCF("T A | B");
            apply(rtou, cf1); p(); p("");

            ConjuntoFormula cf2 = parser.parserCF("T A & B");
            apply(rtou, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("T (A & B) | (C -> D)");
            apply(rtou, cf3); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("T (A & B) -> (C -> D)");
            apply(rtou, cf4); p(); p("");
        }

        public void testeRegraTrueImplica()
        {
            IRegraUnariaDouble rtimplica = new RegraTrueImplica();

            ConjuntoFormula cf1 = parser.parserCF("T A -> B");
            apply(rtimplica, cf1); p(); p("");

            ConjuntoFormula cf2 = parser.parserCF("T A & B");
            apply(rtimplica, cf2); p(); p("");

            ConjuntoFormula cf3 = parser.parserCF("T (A & B) | (C -> D)");
            apply(rtimplica, cf3); p(); p("");

            ConjuntoFormula cf4 = parser.parserCF("T (A & B) -> (C -> D)");
            apply(rtimplica, cf4); p(); p("");
        }

        #region apply rules

        private void apply(IRegraUnaria rUnaria, ConjuntoFormula cf1)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(string.Format("------ {0}", rUnaria.RULE));
            Console.WriteLine(rUnaria.apply(cf1));
        }

        private void apply(IRegraUnariaDouble rUnariaDouble, ConjuntoFormula cf1)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(string.Format("------ {0}", rUnariaDouble.RULE));
            StRetornoRegras? cfs = rUnariaDouble.apply(cf1);
            Console.WriteLine(cfs != null ? cfs.ToString() : "null");
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