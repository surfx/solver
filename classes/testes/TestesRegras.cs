using clases.formulas;
using clases.regras.binarias;
using clases.regras.unitarias;

namespace clases.testes
{

    public class TestesRegras
    {

        /* 
            F ¬A
            -----   (F ¬)
            T  A
        */
        public void testeRegraFalsoNegativo()
        {

            ConjuntoFormula cf1 = new ConjuntoFormula(false, new Atomo("A", true));
            ConjuntoFormula? cf2 = RegraFalsoNegativo.apply(cf1);

            Console.WriteLine(cf1);
            Console.WriteLine(cf2);
            Console.WriteLine("----------------------------------");

            ConjuntoFormula cf3 = new ConjuntoFormula(true, new Atomo("C", true));
            ConjuntoFormula? cf4 = RegraFalsoNegativo.apply(cf3);

            Console.WriteLine(cf3);
            Console.WriteLine(cf4);

            Console.WriteLine("----------------------------------");

            ConjuntoFormula cf5 = new ConjuntoFormula(false, new Conector(ESimbolo.E, new Conector(ESimbolo.IMPLICA, new Atomo("B"), new Atomo("C")), new Atomo("D"), true));
            Console.WriteLine(cf5);
            Console.WriteLine(RegraFalsoNegativo.apply(cf5));
            Console.WriteLine("----------------------------------");

        }

        /*
            T A → B
            T A
            ------- (T →1)
            T B
        */
        public void testeRegraTrueImplica(){

            // ESimbolo simbolo, AtomoConector esquerda, AtomoConector direita, bool negado = false
            Conector c1 = new Conector(ESimbolo.IMPLICA, new Atomo("A", false), new Atomo("B", false));

            // bool simbolo, IConversor conversor
            ConjuntoFormula cf1 = new ConjuntoFormula(true, c1);
            ConjuntoFormula cf2 = new ConjuntoFormula(true, new Atomo("A", false));
            ConjuntoFormula? cf3 = RegraTrueImplica.apply(cf1, cf2);

            Console.WriteLine(cf1);
            Console.WriteLine(cf2);
            Console.WriteLine(cf3);
            Console.WriteLine("----------------------------------");

            
            Conector c2 = new Conector(ESimbolo.OU, new Atomo("D", true), new Atomo("G", false));
            Conector c3 = new Conector(ESimbolo.IMPLICA, new Atomo("A", false), c2);
            ConjuntoFormula cf5 = new ConjuntoFormula(true, c3);
            ConjuntoFormula? cf6 = RegraTrueImplica.apply(cf5, cf2);
            
            Console.WriteLine(cf2);
            Console.WriteLine(cf5);
            Console.WriteLine(cf6);
            Console.WriteLine("----------------------------------");

            Conector c4 = new Conector(ESimbolo.E, new Atomo("D", true), new Atomo("G", false));
            Conector c5 = new Conector(ESimbolo.IMPLICA, new Atomo("A", false), c2);
            ConjuntoFormula cf7 = new ConjuntoFormula(true, c3);
            ConjuntoFormula? cf8 = RegraTrueImplica.apply(cf5, cf2);
            
            Console.WriteLine(cf2);
            Console.WriteLine(cf5);
            Console.WriteLine(cf6);
            Console.WriteLine("----------------------------------");

            /*
            T G → D
            T ( G → D ) → (¬D v G)
            ------------------
            T ¬D v G
            */

        }

        public void testeRegraTrueImplica2(){
            /*
            T G → D
            T ( G → D ) → (¬D v G)
            ------------------
            T ¬D v G
            */


            Conector c1 = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D"));
            ConjuntoFormula cf1 = new ConjuntoFormula(true, c1.copy());
            Conector c2 = new Conector(ESimbolo.OU, new Atomo("D", true), new Atomo("G"));
            ConjuntoFormula cf2 = new ConjuntoFormula(true, new Conector(ESimbolo.IMPLICA, c1.copy(), c2.copy()));
            Console.WriteLine(cf1);
            Console.WriteLine(cf2);
            Console.WriteLine("----------------------------------");
            Console.WriteLine(RegraTrueImplica.apply(cf1, cf2));

            Console.WriteLine("\n----------------------------------\n");

            Conector c3 = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D"));
            Console.WriteLine(c3);
            Console.WriteLine(string.Format("{0} == {1}: {2}", c3, c1, c3.Equals(c1)));
            Console.WriteLine(string.Format("{0} == {1}: {2}", c3, c2, c3.Equals(c2)));

            Console.WriteLine(string.Format("{0} nc: {1}", cf1, cf1.numeroConectores()));
            Console.WriteLine(string.Format("{0} nc: {1}", cf2, cf2.numeroConectores()));

        }


    }

}