using classes.auxiliar.diagnosticos;
using classes.formulas;
using static classes.auxiliar.valoracoes.Valoracoes;

namespace classes.solverstage.auxiliar
{
    public class DadosSolver : IDisposable
    {
        public StDadosConsumo? DadosConsumo { get; set; } = null;
        public bool isClosed { get; set; } = false;
        public int Complexidade { get; set; } = 0;
        public int Bifurcacoes { get; set; } = 0;
        public int NumeroAtomosLivres { get; set; } = 0;
        public int NumeroFormulas { get; set; } = 0;
        public StRamosAbertosFechados? RamosAbertosFechados { get; set; } = null;
        public StAlturas? Alturas { get; set; } = null;
        public int NumeroConectores { get; set; } = 0;

        public List<Contradicoes<ConjuntoFormula?>>? Contradicoes { get; set; } = null;

        public DadosSolver() { }

        public override string ToString()
        {
            return string.Format(
                "{0}\n" +
                "closed: {1}\n" +
                "Complexidade: {2}\n" +
                "Bifurcacoes: {3}\n" +
                "NumeroAtomosLivres: {4}\n" +
                "NumeroFormulas: {5}\n" +
                "RamosAbertosFechados: {6}\n" +
                "Alturas: {7}\n" +
                "NumeroConectores: {8}\n" +
                "Contradições:\n{9}"
                ,
                DadosConsumo == null ? "" : DadosConsumo.Value.ToString(),
                isClosed,
                Complexidade,
                Bifurcacoes,
                NumeroAtomosLivres,
                NumeroFormulas,
                RamosAbertosFechados == null ? "" : RamosAbertosFechados.Value.ToString(),
                Alturas == null ? "" : Alturas.Value.ToString(),
                NumeroConectores,
                string.Join("\n",
                    Contradicoes?.Select(c => string.Format("\t{0} {1} e {2} {3}", c.Formula1?.NumeroFormula, c.Formula1, c.Formula2?.NumeroFormula, c.Formula2))?.ToList()
                )
            );
        }

        public void Dispose()
        {
            DadosConsumo = null;
            isClosed = false;
            RamosAbertosFechados = null;
            Alturas = null;
            Complexidade = Bifurcacoes = NumeroAtomosLivres = NumeroFormulas = NumeroConectores = 0;
        }

    }
}