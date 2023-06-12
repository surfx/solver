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
            entrada = "A -> B";
            entrada = "(A -> B) | (C & D)";
            entrada = "(A -> B) | C";
            entrada = "((E & F) | (A -> B)) | ((C & D) -> D)";
            p(UtilFormulas.sanitizar(entrada));

            Parser parser = new Parser();
            AtomoConector ac = parser.parserCF(entrada).AtomoConectorProp;
            p(string.Format("{0}, {1}, {2}", ac, ac.sizeStr(), ac.heightTree()));

            // p();

            // if (ac.isAtomo)
            // {
            //     p(string.Format("{0}, {1}, {2}", ac.AtomoProp, ac.AtomoProp.sizeStr(), ac.AtomoProp.heightTree()));
            // }
            // else
            // {
            //     p(string.Format("{0}, {1}, {2}", ac.ConectorProp.Esquerda, ac.ConectorProp.Esquerda.sizeStr(), ac.ConectorProp.Esquerda.heightTree()));
            //     p(string.Format("{0}, {1}, {2}", ac.ConectorProp.Direita, ac.ConectorProp.Esquerda.sizeStr(), ac.ConectorProp.Direita.heightTree()));
            // }


            //p(string.Format("{0}, {1}", ac.ConectorProp.Esquerda.AtomoProp, ac.ConectorProp.Esquerda.AtomoProp.sizeStr()));
            //p(string.Format("{0}, {1}", ac.ConectorProp.Direita.AtomoProp, ac.ConectorProp.Direita.AtomoProp.sizeStr()));

            //print(ac);

            p();
            teste(ac);

            // EspacosTree et = new EspacosTree(){
            //     Esquerda=7,
            //     Texto="A",
            //     Direita=8
            // };
            // p(et.ToString());

        }

        private void teste(AtomoConector ac)
        {
            //p(toStrAux(ac, 1, ac.sizeStr(), ac.heightTree()));
            //p(testeAux(ac.ConectorProp.Esquerda, ac.ConectorProp.Direita, 1, ac.sizeStr(), ac.heightTree()));

            p(toEspacosTree(ac, 0 + 1, ac.sizeStr(), ac.heightTree()).ToString());
            List<ItemEspacosTree> itens = testeEspacosTree(ac.ConectorProp.Esquerda, ac.ConectorProp.Direita, 1, ac.sizeStr(), ac.heightTree());
            //List<ItemEspacosTree> itens = testeEspacosTree2(ac, 1, ac.sizeStr(), ac.heightTree());
            //p(et.ToString());
            itens.ForEach(x =>
            {
                p(x.ToString());
            });
        }

        private string testeAux(AtomoConector? left, AtomoConector? right, int andar, int treeWidth, int treeHeight)
        {
            if (left == null && right == null) { return ""; }
            string rt = toStrAux(left, andar + 1, treeWidth, treeHeight) + " / " + toStrAux(right, andar + 1, treeWidth, treeHeight, true);
            if (left.ConectorProp == null && right.ConectorProp == null) { return rt; }
            string auxL = "", auxR = "";
            if (left != null && left.isConector)
            {
                auxL = testeAux(left.ConectorProp.Esquerda, left.ConectorProp.Direita, andar + 1, treeWidth, treeHeight);
            }
            if (right != null && right.isConector)
            {
                auxR = testeAux(right.ConectorProp.Esquerda, right.ConectorProp.Direita, andar + 1, treeWidth, treeHeight);
            }
            if (string.IsNullOrEmpty(auxL) && string.IsNullOrEmpty(auxR)) { return rt; }
            return rt + "\n" + (auxL + (string.IsNullOrEmpty(auxR) ? "" : " / " + auxR));
        }

        private string toStrAux(AtomoConector? ac, int andar, int treeWidth, int treeHeight, bool zerarEsquerda = false)
        {
            if (ac == null) { return ""; }
            int n = treeHeight - andar;
            int numeroEspacosEsquerda = zerarEsquerda ? 0 : (int)Math.Pow(2, n) - 1;
            int numeroEspacosDireita = (int)Math.Pow(2, n + 1) - 1;
            return string.Format("{0}, {1}, w: {2}, h: {3} [{4} {5},{6}] [{7}, {8}]",
                ac, ac.isAtomo ? ac.AtomoProp : Auxiliar.toSimbolo(ac.ConectorProp.Simbolo),
                ac.sizeStr(), ac.heightTree(), andar, treeWidth, treeHeight, numeroEspacosEsquerda, numeroEspacosDireita);
        }



        class EspacosTree
        {
            public string Texto { get; set; }
            public int Esquerda { get; set; }
            public int Direita { get; set; }

            public EspacosTree(string texto, int esquerda, int direita)
            {
                Texto = texto;
                Esquerda = esquerda;
                Direita = direita;
            }

            public override string? ToString()
            {
                string eE = Esquerda <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", Esquerda));
                string eD = Direita <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", Direita));
                return string.Format("{0}{1}{2}", eE, Texto, eD);
            }
        }

        class ItemEspacosTree
        {
            private List<EspacosTree> itens = new List<EspacosTree>(); // itens in line - na mesma linha
            public void addEspacosTree(EspacosTree? item)
            {
                if (item == null) { return; }
                itens.Add(item);
            }
            public List<EspacosTree> Itens { get => itens; }

            public override string ToString()
            {
                string rt = "";
                itens.ForEach(x => rt += x.ToString());
                return rt;
            }
        }

        private List<ItemEspacosTree>? testeEspacosTree2(AtomoConector? ac, int andar, int treeWidth, int treeHeight)
        {
            if (ac == null) { return null; }
            EspacosTree? et = toEspacosTree(ac, andar + 1, treeWidth, treeHeight);
            if (et == null) { return null; }
            List<ItemEspacosTree> rt = new List<ItemEspacosTree>();
            ItemEspacosTree item = new ItemEspacosTree();
            item.addEspacosTree(et);
            rt.Add(item);

            if (ac.isAtomo) { return rt; }
            Conector? c = ac.ConectorProp;

            List<ItemEspacosTree>? lesquerda = testeEspacosTree2(c.Esquerda, andar + 1, treeWidth, treeHeight);
            List<ItemEspacosTree>? ldireita = testeEspacosTree2(c.Direita, andar + 1, treeWidth, treeHeight);
            ItemEspacosTree item2 = new ItemEspacosTree();
            if (lesquerda != null)
            {
                lesquerda[0].Itens.ForEach(y => item2.addEspacosTree(y));
                lesquerda.RemoveAt(0);
                //lesquerda.ForEach(x => x.Itens.ForEach(y => item2.addEspacosTree(y)));

                lesquerda.ForEach(x => rt.Add(x));
            }
            if (ldireita != null)
            {
                ldireita[0].Itens.ForEach(y => item2.addEspacosTree(y));
                ldireita.RemoveAt(0);
                //ldireita.ForEach(x => x.Itens.ForEach(y => item2.addEspacosTree(y)));

                ldireita.ForEach(x => rt.Add(x));
            }
            rt.Add(item2);
            return rt;
        }

        private List<ItemEspacosTree> testeEspacosTree(AtomoConector? left, AtomoConector? right, int andar, int treeWidth, int treeHeight)
        {
            if (left == null && right == null) { return null; }

            EspacosTree eL = toEspacosTree(left, andar + 1, treeWidth, treeHeight);
            EspacosTree eD = toEspacosTree(right, andar + 1, treeWidth, treeHeight, true);

            List<ItemEspacosTree> rt = new List<ItemEspacosTree>();
            ItemEspacosTree item = new ItemEspacosTree();
            item.addEspacosTree(eL);
            item.addEspacosTree(eD);
            rt.Add(item);

            //p(string.Format("{0}{1}", eL.ToString(), eD.ToString()));
            //p(string.Format("{0}", eL.ToString()));

            //string rt = toEspacosTree(left, andar + 1, treeWidth, treeHeight) + " / " + toEspacosTree(right, andar + 1, treeWidth, treeHeight, true);
            if (left.ConectorProp == null && right.ConectorProp == null) { return rt; }

            List<ItemEspacosTree>? eaux1 = null;
            List<ItemEspacosTree>? eaux2 = null;
            if (left != null && left.isConector)
            {
                eaux1 = testeEspacosTree(left.ConectorProp.Esquerda, left.ConectorProp.Direita, andar + 1, treeWidth, treeHeight);
            }
            if (right != null && right.isConector)
            {
                eaux2 = testeEspacosTree(right.ConectorProp.Esquerda, right.ConectorProp.Direita, andar + 1, treeWidth, treeHeight);
            }

            ItemEspacosTree itemNew = new ItemEspacosTree();
            if (eaux1 != null)
            {
                eaux1.ForEach(x => x.Itens.ForEach(y => itemNew.addEspacosTree(y)));
            }
            if (eaux2 != null)
            {
                eaux2.ForEach(x => x.Itens.ForEach(y => itemNew.addEspacosTree(y)));
            }

            rt.Add(itemNew);

            // if (string.IsNullOrEmpty(auxL) && string.IsNullOrEmpty(auxR)) { return rt; }
            // return rt + "\n" + (auxL + (string.IsNullOrEmpty(auxR) ? "" : " / " + auxR));
            return rt;
        }

        private EspacosTree? toEspacosTree(AtomoConector? ac, int andar, int treeWidth, int treeHeight, bool zerarEsquerda = false)
        {
            if (ac == null) { return null; }
            int n = treeHeight - andar;
            int numeroEspacosEsquerda = zerarEsquerda ? 0 : (int)Math.Pow(2, n) - 1;
            int numeroEspacosDireita = (int)Math.Pow(2, n + 1) - 1;
            string texto = ac.isAtomo ? ac.AtomoProp.Simbolo : Auxiliar.toSimbolo(ac.ConectorProp.Simbolo);
            return new EspacosTree(
                ac.isAtomo ? ac.AtomoProp.Simbolo : Auxiliar.toSimbolo(ac.ConectorProp.Simbolo),
                numeroEspacosEsquerda,
                numeroEspacosDireita
            );
            // return string.Format("{0}, {1}, w: {2}, h: {3} [{4} {5},{6}] [{7}, {8}]",
            //     ac, ac.isAtomo ? ac.AtomoProp : Auxiliar.toSimbolo(ac.ConectorProp.Simbolo),
            //     ac.sizeStr(), ac.heightTree(), andar, treeWidth, treeHeight, numeroEspacosEsquerda, numeroEspacosDireita);
        }


        private void print(AtomoConector ac)
        {

            //p(string.Format("{0}, {1}", ac, ac.sizeStr()));
            double size = ac.sizeStr();
            //p(string.Format("{0} '{1}'", size, getEspaco(size)));
            p(string.Format("{0}{1}", getEspaco(size), Auxiliar.toSimbolo(ac.ConectorProp.Simbolo)));

            string sizeE = getEspaco(ac.ConectorProp.Esquerda.sizeStr() * 2 - 1);
            string sizeD = getEspaco(ac.ConectorProp.Direita.sizeStr() * 2);
            string sE = ac.ConectorProp.Esquerda.isAtomo ? ac.ConectorProp.Esquerda.AtomoProp.Simbolo : Auxiliar.toSimbolo(ac.ConectorProp.Esquerda.ConectorProp.Simbolo);
            string sD = ac.ConectorProp.Direita.isAtomo ? ac.ConectorProp.Direita.AtomoProp.Simbolo : Auxiliar.toSimbolo(ac.ConectorProp.Direita.ConectorProp.Simbolo);
            p(string.Format("{0}{1}{2}{3}", sizeE, sE, sizeD, sD));

        }


        private string getEspaco(double size)
        {
            //if (size <= 1.0) { return " "; }
            //size /= 2.0;
            int n = (int)size;
            return n <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", n));
        }

        private string getEspaco(int size)
        {
            return size <= 0 ? string.Empty : string.Concat(Enumerable.Repeat(" ", size));
        }

        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }

    }

}