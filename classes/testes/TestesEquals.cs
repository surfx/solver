using clases.formulas;

namespace clases.testes
{

    public class TestesEquals
    {

        public void teste1()
        {

            Atomo a1 = new Atomo("a1", false);      //  a1
            Atomo a1neg = new Atomo("a1", true);    // ¬a1

            comparar("a1", "a1", a1, a1);
            comparar("a1neg", "a1neg", a1neg, a1neg);
            comparar("a1neg", "a1", a1neg, a1);
            p();

            Conector c1 = new Conector(ESimbolo.E, a1.copy(), a1neg.copy(), true);          // ¬(a1 ˄ ¬a1)
            Conector c2 = new Conector(ESimbolo.OU, a1.copy(), a1neg.copy(), true);         // ¬(a1 v ¬a1)
            Conector c3 = new Conector(ESimbolo.IMPLICA, a1neg.copy(), a1.copy(), true);    // ¬(¬a1 → a1)
            Conector c4 = new Conector(ESimbolo.IMPLICA, a1neg.copy(), a1.copy(), true);    // ¬(¬a1 → a1)
            Conector c5 = new Conector(ESimbolo.OU, c1.copy(), c2.copy(), true);            // ¬((¬(a1 ˄ ¬a1)) v (¬(a1 v ¬a1)))
            Conector c6 = new Conector(ESimbolo.OU, c1.copy(), new Conector(ESimbolo.E, a1.copy(), a1neg.copy(), true), true);  // ¬((¬(a1 ˄ ¬a1)) v (¬(a1 ˄ ¬a1)))
            comparar("c1", "c2", c1, c2);
            comparar("c3", "c1", c3, c1);
            comparar("c3", "c4", c3, c4);
            comparar("c5", "c5", c5, c5);
            comparar("c5", "c6", c5, c6);
            comparar("c6", "c6", c6, c6);
            comparar("c1", "c6", c1, c6);
            p();

            AtomoConector ac1 = new AtomoConector(a1.copy());       // a1
            AtomoConector ac2 = new AtomoConector(a1neg.copy());    // ¬a1
            AtomoConector ac3 = new AtomoConector(c1.copy());       // ¬(a1 ˄ ¬a1)
            AtomoConector ac4 = new AtomoConector(c2.copy());       // ¬(a1 v ¬a1)
            AtomoConector ac5 = new AtomoConector(c3.copy());       // ¬(¬a1 → a1)
            AtomoConector ac6 = new AtomoConector(c5.copy());       // ¬((¬(a1 ˄ ¬a1)) v (¬(a1 v ¬a1)))
            AtomoConector ac7 = new AtomoConector(c6.copy());       // ¬((¬(a1 ˄ ¬a1)) v (¬(a1 ˄ ¬a1)))
            
            comparar("ac1", "ac1", ac1, ac1);
            comparar("ac2", "ac1", ac2, ac1);
            comparar("ac2", "ac3", ac2, ac3);
            comparar("ac3", "ac3", ac3, ac3);
            comparar("ac3", "ac4", ac3, ac4);
            comparar("ac3", "ac5", ac3, ac5);
            comparar("ac4", "ac5", ac4, ac5);
            comparar("ac6", "ac5", ac6, ac5);
            comparar("ac7", "ac5", ac7, ac5);
            comparar("ac7", "ac6", ac7, ac6);
            comparar("ac6", "ac6", ac6, ac6);
            comparar("ac7", "ac7", ac7, ac7);
            p();

            // AtomoConector
            // ConjuntoFormula

            ConjuntoFormula cf1 = new ConjuntoFormula(true, ac1.copy());
            ConjuntoFormula cf2 = new ConjuntoFormula(true, ac2.copy());
            ConjuntoFormula cf3 = new ConjuntoFormula(true, ac3.copy());
            ConjuntoFormula cf4 = new ConjuntoFormula(true, ac4.copy());
            ConjuntoFormula cf5 = new ConjuntoFormula(true, ac5.copy());
            ConjuntoFormula cf6 = new ConjuntoFormula(true, ac6.copy());
            ConjuntoFormula cf7 = new ConjuntoFormula(true, ac7.copy());
            comparar("cf1", "cf1", cf1, cf1);
            comparar("cf1", "cf2", cf1, cf2);
            comparar("cf1", "cf3", cf1, cf3);
            comparar("cf3", "cf3", cf3, cf3);
            comparar("cf2", "cf3", cf2, cf3);
            comparar("cf4", "cf3", cf4, cf3);
            comparar("cf5", "cf3", cf5, cf3);
            comparar("cf5", "cf5", cf5, cf5);
            comparar("cf4", "cf5", cf4, cf5);
            comparar("cf6", "cf5", cf6, cf5);
            comparar("cf6", "cf6", cf6, cf6);
            comparar("cf7", "cf5", cf7, cf5);
            comparar("cf7", "cf7", cf7, cf7);
            comparar("cf6", "cf7", cf6, cf7);
            p();

            // AtomoConector esq = new AtomoConector(null, new Atomo("a1"));
            // AtomoConector dir = new AtomoConector(null, new Atomo("a2", false));

            // Conector c1 = new Conector(ESimbolo.E, esq, dir, false);
            // Console.WriteLine(c1);


            // AtomoConector esq1 = new AtomoConector(c1);

            // Conector c2 = new Conector(ESimbolo.IMPLICA, esq1, dir, false);
            // Console.WriteLine(c2);

            // AtomoConector dir2 = new AtomoConector(c2);
            // Conector c3 = new Conector(ESimbolo.IMPLICA, esq1, dir2, false);
            // Console.WriteLine(c3);


        }

        private void comparar(string l1, string l2, Atomo p1, Atomo p2){
            Console.WriteLine(string.Format("[{0}, {1}] {2} = {3}: {4}", l1, l2, p1, p2, p1.Equals(p2)));
        }

        private void comparar(string l1, string l2,Conector p1, Conector p2){
            Console.WriteLine(string.Format("[{0}, {1}] {2} = {3}: {4}", l1, l2, p1, p2, p1.Equals(p2)));
        }

        private void comparar(string l1, string l2, AtomoConector p1, AtomoConector p2){
            Console.WriteLine(string.Format("[{0}, {1}] {2} = {3}: {4}", l1, l2, p1, p2, p1.Equals(p2)));
        }

        private void comparar(string l1, string l2, ConjuntoFormula p1, ConjuntoFormula p2){
            Console.WriteLine(string.Format("[{0}, {1}] {2} = {3}: {4}", l1, l2, p1, p2, p1.Equals(p2)));
        }

        private void p(){Console.WriteLine("-----------------");}

    }

}