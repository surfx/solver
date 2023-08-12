using classes.auxiliar.formulas;
using classes.parser;
using static classes.auxiliar.formulas.UtilFormulas;

namespace classes.testes
{

    public class TestesParser
    {

        private static string[] conectores = AuxiliarFormulas.getSimbolos(false);
        private const string simboloNegado = AuxiliarFormulas.SimboloNegado;

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
            p(string.Format("{0}", parser.parserCF("T (A | C -> E) | (!C | B & E)")));
        }

    }

}