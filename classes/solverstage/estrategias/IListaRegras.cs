using classes.regras;
using classes.regras.binarias.closed;

namespace classes.solverstage.estrategias
{
    public interface IListaRegras : IDisposable
    {
        public List<IRegraBinaria>? RegrasBinarias { get; set; }
        public List<IRegraUnaria>? RegrasUnarias { get; set; }
        public List<IRegraUnariaDouble>? RegrasUnariasDouble { get; set; }
        public List<IRegraUnariaDouble>? RegrasBeta { get; set; }
        //private IRegraUnariaDouble? RegraPBProp { get; set; }

        public RegraClosed? RegraClosedProp { get; set; }
    }
}