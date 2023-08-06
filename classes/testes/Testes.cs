using classes.formulas;

namespace classes.testes
{

    public class Testes
    {

        public void teste1()
        {

            AtomoConector esq = new(null, new Atomo("a1"));
            AtomoConector dir = new(null, new Atomo("a2", 2));

            Conector c1 = new(ESimbolo.E, esq, dir, 0);
            Console.WriteLine(c1);


            AtomoConector esq1 = new(c1);

            Conector c2 = new(ESimbolo.IMPLICA, esq1, dir, 0);
            Console.WriteLine(c2);

            AtomoConector dir2 = new(c2);
            Conector c3 = new(ESimbolo.IMPLICA, esq1, dir2, 0);
            Console.WriteLine(c3);


        }

        public void teste2()
        {

            Conector c1 = new(ESimbolo.E, new Atomo("a1"), new Atomo("a2"), 1); // a1 ^ a2
            Console.WriteLine(c1);

            Conector c2 = new(ESimbolo.IMPLICA, c1, new Atomo("a3", 1), 0); // (a1 ^ a2) → ¬a3
            Console.WriteLine(c2);

            Conector c3 = new(ESimbolo.OU, c1, c2, 0); // (a1 ^ a2) v ((a1 ^ a2) → ¬a3)
            Console.WriteLine(c3);

            c3.NumeroNegados = 1;
            Conector c4 = new(ESimbolo.IMPLICA, c3, c1, 1); // ¬((¬((a1 ^ a2) v ((a1 ^ a2) → ¬a3))) → (a1 ^ a2))
            Console.WriteLine(c4);

            ConjuntoFormula cf1 = new(true, null, c4); // T ¬((¬((a1 ^ a2) v ((a1 ^ a2) → ¬a3))) → (a1 ^ a2))
            Console.WriteLine(cf1);

            ConjuntoFormula cf2 = new(false, null, c1); // F a1 ^ a2
            Console.WriteLine(cf2);

        }

    }

}