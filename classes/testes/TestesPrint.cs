using classes.auxiliar;
using classes.formulas;
using classes.parser;

namespace classes.testes
{

    public class TestesPrint
    {

        public void teste1()
        {

            string entrada = "!!(FA -> B) & C";
            //entrada = "A -> B";
            p(entrada);

            Parser parser = new Parser();
            AtomoConector ac = parser.parserCF(entrada).AtomoConectorProp;
            p(string.Format("{0}, {1}", ac, ac.sizeStr()));

            //p(string.Format("{0}, {1}", ac.ConectorProp.Esquerda.AtomoProp, ac.ConectorProp.Esquerda.AtomoProp.sizeStr()));
            //p(string.Format("{0}, {1}", ac.ConectorProp.Direita.AtomoProp, ac.ConectorProp.Direita.AtomoProp.sizeStr()));

            print(ac);


        }

        private void print(AtomoConector ac)
        {

            //p(string.Format("{0}, {1}", ac, ac.sizeStr()));
            double size = ac.sizeStr();
            //p(string.Format("{0} '{1}'", size, getEspaco(size)));
            p(string.Format("{0}{1}", getEspaco(size), Auxiliar.toSimbolo(ac.ConectorProp.Simbolo)));

            string sizeE = getEspaco(ac.ConectorProp.Esquerda.sizeStr()*2-1);
            string sizeD = getEspaco(ac.ConectorProp.Direita.sizeStr()*2);
            string sE = ac.ConectorProp.Esquerda.isAtomo? ac.ConectorProp.Esquerda.AtomoProp.Simbolo : Auxiliar.toSimbolo(ac.ConectorProp.Esquerda.ConectorProp.Simbolo);
            string sD = ac.ConectorProp.Direita.isAtomo? ac.ConectorProp.Direita.AtomoProp.Simbolo : Auxiliar.toSimbolo(ac.ConectorProp.Direita.ConectorProp.Simbolo);
            p(string.Format("{0}{1}{2}{3}", sizeE, sE, sizeD, sD));

        }


        private string getEspaco(double size)
        {
            //if (size <= 1.0) { return " "; }
            //size /= 2.0;
            int n = (int)size;
            return n <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", n));
        }

        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }

    }

}