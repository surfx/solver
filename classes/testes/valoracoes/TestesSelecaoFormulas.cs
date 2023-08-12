using static classes.auxiliar.formulas.UtilFormulas;

namespace classes.testes.print.valoracoes
{
    public class TestesSelecaoFormulas
    {

        public void teste1()
        {

            List<int> listaInteiros = new();
            for (int i = 0; i < 100; i++) { listaInteiros.Add(i); }

            //words.Sort((a, b) => a.Length.CompareTo(b.Length));


            listaInteiros.Sort((v1, v2) =>
            {
                // primeiro os primos, depois os pares
                // sendo os ímpares de forma decrescente, e os pares, de forma crescente
                if (v1 == v2 || v1 - v2 == 0) { return 0; }

                // ambos ímpares
                if (v1 % 2 != 0 && v2 % 2 != 0)
                {
                    return v1 == v2 ? 0 : v1 > v2 ? -1 : 1; // ímpar maior tem prioridade (-1) - ordem decrescente
                }
                if (v1 % 2 != 0)
                {
                    return v1 == v2 ? 0 : v1 > v2 ? -1 : 1; // ímpar (v1) tem prioridade sobre par (v2): (-1) - ordem decrescente
                }
                if (v2 % 2 != 0)
                {
                    return v1 == v2 ? 0 : v2 > v1 ? -1 : 1; // ímpar (v2) tem prioridade sobre par (v1): (-1) - ordem decrescente
                }
                return v1 == v2 ? 0 : v1 > v2 ? 1 : -1; // o menor par tem menor prioridade (1) - ordem crescente
            });

            p(string.Join(",", listaInteiros));

        }

    }
}