using classes.formulas;
using classes.regras;
using classes.regras.binarias;
using classes.regras.unitarias;

namespace classes.testes
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

            IRegraUnaria rfn = new RegraFalsoNegativo();

            ConjuntoFormula cf1 = new ConjuntoFormula(false, new Atomo("A", 1));

            apply(rfn, cf1);
            p();

            ConjuntoFormula cf2 = new ConjuntoFormula(true, new Atomo("C", 1));

            apply(rfn, cf2);
            p();

            ConjuntoFormula cf5 = new ConjuntoFormula(false, new Conector(ESimbolo.E, new Conector(ESimbolo.IMPLICA, new Atomo("B"), new Atomo("C")), new Atomo("D"), 1));
            apply(rfn, cf5);
            p();

        }

        /*
            T A → B
            T A
            ------- (T →1)
            T B
        */
        public void testeRegraTrueImplica1(){

            IRegraBinaria rti = new RegraTrueImplica();

            // ESimbolo simbolo, AtomoConector esquerda, AtomoConector direita, bool negado = false
            Conector c1 = new Conector(ESimbolo.IMPLICA, new Atomo("A", 0), new Atomo("B", 0));

            // bool simbolo, IConversor conversor
            ConjuntoFormula cf1 = new ConjuntoFormula(true, c1);
            ConjuntoFormula cf2 = new ConjuntoFormula(true, new Atomo("A", 0));

            apply(rti, cf1, cf2);
            p();

            
            Conector c2 = new Conector(ESimbolo.OU, new Atomo("D", 1), new Atomo("G", 0));
            Conector c3 = new Conector(ESimbolo.IMPLICA, new Atomo("A", 0), c2);
            ConjuntoFormula cf5 = new ConjuntoFormula(true, c3);                                    // T A → (¬D v G)
            
            apply(rti, cf5, cf2);
            p();

            // T G → D
            Conector G_impl_D = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D"));     //  G → D
            Conector nD_ou_G = new Conector(ESimbolo.OU, new Atomo("D", 1), new Atomo("G"));     // ¬D v G
            Conector G_impl_D_impl_nD_ou_G = new Conector(ESimbolo.IMPLICA, G_impl_D, nD_ou_G);     // (G → D) → (¬D v G)
            ConjuntoFormula cf7 = new ConjuntoFormula(true, G_impl_D_impl_nD_ou_G);                 // T (G → D) → (¬D v G)
            ConjuntoFormula cf8 = new ConjuntoFormula(true, G_impl_D);                              // T G → D

            apply(rti, cf7, cf8);
            p();

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
            
            IRegraBinaria rti = new RegraTrueImplica();

            //Conector c1 = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D", true));

            Conector c3 = new Conector(ESimbolo.IMPLICA, new Atomo("G"), new Atomo("D", 1));
            ConjuntoFormula cf1 = new ConjuntoFormula(true, c3.copy());
            Conector c2 = new Conector(ESimbolo.OU, new Atomo("D", 1), new Atomo("G"));
            ConjuntoFormula cf2 = new ConjuntoFormula(true, new Conector(ESimbolo.IMPLICA, c3.copy(), c2.copy()));

            apply(rti, cf1, cf2);
            p();

        }

        #region apply rules

        private void apply(IRegraUnaria rUnaria, ConjuntoFormula cf1){
            Console.WriteLine(cf1);
            Console.WriteLine(string.Format("------ {0}", rUnaria.RULE));
            Console.WriteLine(rUnaria.apply(cf1));
        }

        private void apply(IRegraBinaria rBinaria, ConjuntoFormula cf1, ConjuntoFormula cf2){
            Console.WriteLine(cf1);
            Console.WriteLine(cf2);
            Console.WriteLine(string.Format("------ {0}", rBinaria.RULE));
            Console.WriteLine(rBinaria.apply(cf1, cf2));
        }

        #endregion

        private void p(){Console.WriteLine("-----------------");}

    }

}