using classes.auxiliar.formulas;
using classes.formulas;
using classes.parser;
using classes.regras;
using classes.regras.unitarias;
using static classes.auxiliar.formulas.UtilFormulas;

namespace classes.testes.regras
{
    public class TestesRegras2
    {
        [Obsolete]
        public void testeRegraRemoverFalsos()
        {
            Parser parser = new Parser();
            ConjuntoFormula cf1 = parser.parserCF("T !!!(A->!!!B) & !(C | !!F) ");
            ConjuntoFormula cf2 = parser.parserCF("T !A->!!!B & !C | !!!!!F ");


            apply(new RegraRemoverNegativos(), cf1);
            p("");
            apply(new RegraRemoverNegativos(), cf2);
        }

        #region apply rules

        private void apply(IRegraUnaria rUnaria, ConjuntoFormula cf1)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(string.Format("------ {0}", rUnaria.RULE));
            Console.WriteLine(rUnaria.apply(cf1));
        }

        private void apply(IRegraBinaria rBinaria, ConjuntoFormula cf1, ConjuntoFormula cf2)
        {
            Console.WriteLine(cf1);
            Console.WriteLine(cf2);
            Console.WriteLine(string.Format("------ {0}", rBinaria.RULE));
            Console.WriteLine(rBinaria.apply(cf1, cf2));
        }

        #endregion


    }
}