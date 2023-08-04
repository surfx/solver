using classes.regras;
using classes.regras.binarias;
using classes.regras.binarias.closed;
using classes.regras.unitarias;
using classes.regras.unitarias.unidouble;
using classes.regras.unitarias.unidouble.beta;

namespace classes.solverstage.estrategias.listaregras
{
    // regras da lógica clássica proposicional (LCP) - Tableaux KE
    public class ListaRegrasLCP : IListaRegras
    {

        public List<IRegraBinaria>? RegrasBinarias { get; set; }
        public List<IRegraUnaria>? RegrasUnarias { get; set; }
        public List<IRegraUnariaDouble>? RegrasUnariasDouble { get; set; }
        public List<IRegraUnariaDouble>? RegrasBeta { get; set; }
        public RegraClosed? RegraClosedProp { get; set; }

        public ListaRegrasLCP()
        {
            preencherRegras();
        }

        private void preencherRegras()
        {
            RegrasBinarias = new() {
                new RegraFalseE1(),
                new RegraFalseE2(),
                new RegraTrueImplica1(),
                new RegraTrueImplica2(),
                new RegraTrueOu1(),
                new RegraTrueOu2()
            };

            //RegraRemoverNegativos //não usar
            RegrasUnarias = new(){
                new RegraFalseNegativo(),
                new RegraTrueNegativo()
            };

            RegrasUnariasDouble = new() {
                new RegraFalseImplica(),
                new RegraFalseOu(),
                new RegraTrueE()
            };

            // regra PB fica separada
            //RegraPBProp = new RegraPB();

            RegrasBeta = new() {
                new RegraFalsoE(),
                new RegraTrueOu(),
                new RegraTrueImplica()
            };

            RegraClosedProp = new RegraClosed();
        }

        public void Dispose()
        {
            RegrasBinarias?.Clear();
            RegrasBinarias = null;
            RegrasUnarias?.Clear();
            RegrasUnarias = null;
            RegrasUnariasDouble?.Clear();
            RegrasUnariasDouble = null;
            //RegraPBProp = null;
            RegrasBeta?.Clear();
            RegrasBeta = null;
            RegraClosedProp = null;
        }

    }
}