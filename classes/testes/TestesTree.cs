using classes.auxiliar;
using classes.formulas;
using classes.parser;

namespace classes.testes
{

    public class TestesTree
    {

        public void teste1()
        {
            Formulas f = new Formulas();

            Parser parser = new Parser();
            f.addConjuntoFormula(parser.parserCF("A->B"));
            f.addConjuntoFormula(parser.parserCF("F C->E"));
            f.addConjuntoFormula(parser.parserCF("C"));
            f.addConjuntoFormula(parser.parserCF("F A"));
            f.addConjuntoFormula(parser.parserCF("T (A | D) -> (C & D)"));

            f.Negativas.ForEach(x => p(x.ToString()));
            f.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Negativas), sizeMax(f.Positivas))));
            p();

            f.addEsquerda(parser.parserCF("E"));
            f.addEsquerda(parser.parserCF("F Y"));
            //f.addEsquerda(parser.parserCF("T A->B"));
            f.Esquerda.Negativas.ForEach(x => p(x.ToString()));
            f.Esquerda.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Esquerda.Negativas), sizeMax(f.Esquerda.Positivas))));
            p();

            f.addDireita(parser.parserCF("T H->G"));
            f.addDireita(parser.parserCF("F (A|Z) & (C | D) -> J"));
            f.addDireita(parser.parserCF("T G|T&U"));
            f.Direita.Negativas.ForEach(x => p(x.ToString()));
            f.Direita.Positivas.ForEach(x => p(x.ToString()));
            p(string.Format("{0}", Math.Max(sizeMax(f.Direita.Negativas), sizeMax(f.Direita.Positivas))));
            p();

            p(string.Format("sizeMax: {0}", sizeMax(f)));
            p();
            p("");
            p();
            p("");
            p();
            p("");

            toArrayStr(f.Negativas).ForEach(p);
            toArrayStr(f.Positivas).ForEach(p);

            p();
            tryToPrint(f);

            //p(toArrayStr(f.Negativas));
            //p(toArrayStr(f.Positivas));
        }

        class Formulas
        {

            private List<ConjuntoFormula> _positivas, _negativas;
            public List<ConjuntoFormula> Positivas { get => _positivas; }
            public List<ConjuntoFormula> Negativas { get => _negativas; }

            private Formulas _esquerda, _direita;
            public Formulas Esquerda { get => _esquerda; }
            public Formulas Direita { get => _direita; }

            public Formulas()
            {

            }

            public void addConjuntoFormula(ConjuntoFormula cf)
            {
                if (cf.Simbolo)
                {
                    if (_positivas == null) { _positivas = new List<ConjuntoFormula>(); }
                    _positivas.Add(cf);
                }
                else
                {
                    if (_negativas == null) { _negativas = new List<ConjuntoFormula>(); }
                    _negativas.Add(cf);
                }
            }

            public void addEsquerda(ConjuntoFormula cf)
            {
                if (_esquerda == null) { _esquerda = new Formulas(); }
                _esquerda.addConjuntoFormula(cf);
            }

            public void addDireita(ConjuntoFormula cf)
            {
                if (_direita == null) { _direita = new Formulas(); }
                _direita.addConjuntoFormula(cf);
            }

        }


        private void tryToPrint(Formulas f, int maxSizeFormula = 0)
        {
            if (maxSizeFormula <= 0) { maxSizeFormula = sizeMax(f); }
            toArrayStr(f.Negativas).ForEach(x => p(string.Format("{0}{1}", maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / 2), x)));
            toArrayStr(f.Positivas).ForEach(x => p(string.Format("{0}{1}", maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / 2), x)));

            DoubleLists? dl = toListEsquerdaDireita(f);
            maxSizeFormula = Math.Max(sizeMax(f.Esquerda), sizeMax(f.Direita));
            List<string> laux = toListString(dl, maxSizeFormula);
            if (laux == null || laux.Count <= 0) { return; }
            laux.ForEach(p);

            

        }

        private List<string> toListString(DoubleLists? dl, int maxSizeFormula = 0)
        {
            if (dl == null) { return null; }
            if (dl.Lesquerda == null && dl.Ldireita == null) { return null; }
            List<string> lesquerda = dl.Lesquerda;
            List<string> ldireita = dl.Ldireita;

            int lE = lesquerda == null ? 0 : lesquerda.Count;
            int lD = ldireita == null ? 0 : ldireita.Count;
            int length = Math.Max(lE, lD);
            if (length <= 0) { return null; }
            List<string> rt = new List<string>(length);
            for (int i = 0; i < length; i++)
            {
                string itemE = i < lE ? lesquerda[i] : "";
                string itemD = i < lD ? ldireita[i] : "";
                rt.Add(string.Format("{0}{1}{2}{3}",
                    maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / (string.IsNullOrEmpty(itemD) ? 2 : 4)),
                    itemE,
                    maxSizeFormula <= 0 ? "" : getEspaco(maxSizeFormula / (string.IsNullOrEmpty(itemE) ? 2 : 4)),
                    itemD));
            }
            return rt;
        }

        private DoubleLists? toListEsquerdaDireita(Formulas f)
        {
            List<ConjuntoFormula> aux1 = new List<ConjuntoFormula>();
            if (f.Esquerda.Positivas != null) aux1.AddRange(f.Esquerda.Positivas);
            if (f.Esquerda.Negativas != null) aux1.AddRange(f.Esquerda.Negativas);

            List<ConjuntoFormula> aux2 = new List<ConjuntoFormula>();
            if (f.Direita.Positivas != null) aux2.AddRange(f.Direita.Positivas);
            if (f.Direita.Negativas != null) aux2.AddRange(f.Direita.Negativas);

            return toListEsquerdaDireita(aux1, aux2);
        }

        private DoubleLists? toListEsquerdaDireita(List<ConjuntoFormula>? esquerda, List<ConjuntoFormula>? direita)
        {
            if (esquerda == null && direita == null) { return null; }
            List<string> lesquerda = toArrayStr(esquerda);
            List<string> ldireita = toArrayStr(direita);
            return new DoubleLists(toArrayStr(esquerda), toArrayStr(direita));
        }

        class DoubleLists
        {
            public List<string>? Lesquerda { get; set; }
            public List<string>? Ldireita { get; set; }

            public DoubleLists(List<string>? lesquerda, List<string>? ldireita)
            {
                Lesquerda = lesquerda;
                Ldireita = ldireita;
            }
        }

        private int sizeMax(Formulas f)
        {
            if (f == null || (f.Negativas == null && f.Positivas == null && f.Direita == null && f.Esquerda == null)) { return 0; }
            return Math.Max(Math.Max(sizeMax(f.Negativas), sizeMax(f.Positivas)), Math.Max(sizeMax(f.Direita), sizeMax(f.Esquerda)));
        }

        private int sizeMax(List<ConjuntoFormula> formulas)
        {
            if (formulas == null || formulas.Count <= 0) { return 0; }
            return formulas.Select(x => x.ToString().Length).Max();
        }

        private List<string>? toArrayStr(List<ConjuntoFormula> formulas)
        {
            if (formulas == null || formulas.Count <= 0) { return null; }
            return formulas.Select(x => x.ToString()).ToList();
        }

        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }

        private string getEspaco(int size)
        {
            return size <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", size));
        }

    }

}