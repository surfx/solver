﻿using classes.auxiliar.inputs;
using classes.testes;
using classes.testes.closed;
using classes.testes.formulas;
using classes.testes.imagens;
using classes.testes.print;
using classes.testes.print.numerosformulas;
using classes.testes.print.valoracoes;
using classes.testes.regras;
using classes.testes.solverstage;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            ParametrosEntrada? pe = AvaliarParametrosEntrada.avaliar(args);
            if (!AvaliarParametrosEntrada.isParametrosOk(pe))
            {
                Console.WriteLine("Parâmetros informados inválidos");
                helpUso();
                return;
            }

            ExecucaoSolver.executarSolver(pe);
        }

        private static void helpUso()
        {
            Console.WriteLine("-- Parâmetros aceitos");
            Console.WriteLine("file_formulas (obrigatório | entrada): arquivo onde estão as fórmulas lógicas");
            Console.WriteLine("file_img (opcional | saída): arquivo onde a árvore de prova será salva");
            Console.WriteLine("file_estatisticas (opcional | saída): arquivo onde as estatíticas serão salvas");

            string exemplo = @"$ dotnet run file_formulas=C:\Users\...\parser\formulas.txt file_img=C:\Users\...\parser\prova.png file_estatisticas=C:\...\Desktop\parser\estatisticas.txt";
            Console.WriteLine("\nExemplo de uso:\n{0}", exemplo);
        }

        private static void testesAntigos()
        {
            //new Testes().teste2();
            //new TestesEquals().teste1();

            //new TestesRegras().testeRegraFalsoNegativo();
            //new TestesRegras().testeRegraTrueImplica1();
            //new TestesRegras().testeRegraTrueImplica2();

            //new TestesParser().teste1();
            //new TestesParser().teste2();

            //new TestesParser2().teste1();
            //new TestesParser2().teste2();

            //new TestesTree().teste1();
            //new TestesTree().teste2();

            //new TestesTreeList().teste1();
            //new TestesTreeList().teste2();

            //new TestesRegras2().testeRegraRemoverFalsos();

            //new TestesRegras1().testeRegraFalsoNegativo();
            //new TestesRegras1().testeRegraTrueNegativo();
            //new TestesRegras1().testeRegraTrueImplica1();
            //new TestesRegras1().testeRegraTrueImplica2();
            //new TestesRegras1().testeRegraFalseImplica();
            //new TestesRegras1().testeRegraFalseE1();
            //new TestesRegras1().testeRegraFalseE2();
            //new TestesRegras1().testeRegraTrueOu1();
            //new TestesRegras1().testeRegraTrueOu2();
            //new TestesRegras1().testeRegraTrueE();
            //new TestesRegras1().testeRegraFalseOu();
            //new TestesRegras1().testeRegraPB();
            //new TestesRegras1().testeRegraClosed();

            //new TestesBetaRegras3().testeRegraPB();
            //new TestesBetaRegras3().testeRegraFalsoE();
            //new TestesBetaRegras3().testeRegraTrueOu();
            //new TestesBetaRegras3().testeRegraTrueImplica();

            //new TestesSolverStage().teste1();

            //new TestesFormulas().teste1();

            //new TestesImagens().teste1();
            //new TestesImagens().teste2();
            // new TestesImagens().teste3();
            //new TestesImagens().teste4();
            //new TestesImagens().teste5();

            //new TestesImagens2().teste1();

            //new TestesAtualizacaoClosed().testeClosed();

            //new TestesPrint().teste1();
            //new TestesNumerosFormulas().teste1();

            //new TestesValoracoes().teste1();
            //new TestesValoracoes().teste2();
            //new TestesValoracoes().teste3();
            //new TestesValoracoes().teste4();
            //new TestesValoracoes().teste5();

            //new TestesSelecaoFormulas().teste1();
            //new TestesSelecaoFormulas().teste2();

            new TestesSolverStage2().teste1();
        }

    }
}