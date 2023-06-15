using classes.auxiliar;
using classes.formulas;
using classes.parser;

namespace classes.testes
{

    public class TestesParser
    {

        private static string[] conectores = Auxiliar.getSimbolos(false);
        private const string simboloNegado = Auxiliar.SimboloNegado;

        // public void teste1()
        // {

        //     string entrada = UtilFormulas.sanitizar("(!(A)->!!!B)->C");
        //     entrada = UtilFormulas.sanitizar("!!(B -> ( (G -> !(D ^ E)) ) -> ((!D | G) & C))");
        //     p(entrada);

        //     List<ItemList>? list = unirAtomoConector(toList(entrada));
        //     if (list == null || list.Count <= 0) { return; }

        //     p(toStr(list, ","));

        //     tratarNegacoes(list);
        //     unirAtomoConector(list);
        //     p(toStr(list, ","));

        //     p(string.Format("{0}", list.Count()));
        //  }

        public void teste2()
        {
            Parser parser = new Parser();
            p(string.Format("{0}", parser.parserCF(UtilFormulas.sanitizar("A->B"))));
            p(string.Format("{0}", parser.parserCF(UtilFormulas.sanitizar("B->(B & V)"))));
            p(string.Format("{0}", parser.parserCF(UtilFormulas.sanitizar("!A->!(T | !Y)"))));
            p(string.Format("{0}", parser.parserCF(UtilFormulas.sanitizar("F !!(B -> ( (G -> !(D ^ E)) ) -> ((!D | G) & C))"))));
        }


        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }

    }

}