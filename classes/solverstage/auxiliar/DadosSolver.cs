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
        public List<ApplyRegraUnaria<ConjuntoFormula?>>? ApplyRegraUnarias { get; set; } = null;
        public List<ApplyRegraBinaria<ConjuntoFormula?>>? ApplyRegraBinarias { get; set; } = null;
        public List<ApplyRegraUnariaDouble<ConjuntoFormula?>>? ApplyRegraUnariaDoubleProp { get; set; } = null;
        public List<ApplyRegraUnariaDouble<ConjuntoFormula?>>? ApplyRegraBeta { get; set; } = null;

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
                "Contradições: {9}\n" +
                "RegraUnarias: {10}\n" +
                "RegraBinarias: {11}\n" +
                "RegraUnariaDouble: {12}\n" +
                "RegraBeta: {13}"
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
                toStrContradicoes(),
                toStrRegraUnarias(),
                toStrRegraBinarias(),
                toStrRegraUnariaDoubleProp(),
                toStrRegraBeta()
            );
        }

        #region toStr
        private string toStrContradicoes(string separador = " | ")
        {
            return Contradicoes == null || Contradicoes.Count <= 0 ? "" :
            string.Join(
                separador,
                Contradicoes.Select(c => string.Format("{0} {1} e {2} {3}", c.Formula1?.NumeroFormula, c.Formula1, c.Formula2?.NumeroFormula, c.Formula2))?.ToList()
            );
        }

        private string toStrRegraUnarias(string separador = " | ")
        {
            return ApplyRegraUnarias == null || ApplyRegraUnarias.Count <= 0 ? "" :
                string.Join(
                    separador,
                    ApplyRegraUnarias?.Select(c => string.Format("({0}) {1} {2}: {3} {4}", c.RULE, c.InputFormula.NumeroFormula, c.InputFormula, c.OutputFormula?.NumeroFormula, c.OutputFormula))?.ToList()
                );
        }

        private string toStrRegraBinarias(string separador = " | ")
        {
            return ApplyRegraBinarias == null || ApplyRegraBinarias.Count <= 0 ? "" :
                string.Join(
                    separador,
                    ApplyRegraBinarias?.Select(c => string.Format("({0}) {1} {2}, {3} {4}: {5} {6}",
                        c.RULE,
                        c.InputFormula1?.NumeroFormula, c.InputFormula1,
                        c.InputFormula2?.NumeroFormula, c.InputFormula2,
                        c.OutputFormula?.NumeroFormula, c.OutputFormula))?.ToList()
                );
        }

        private string toStrRegraUnariaDoubleProp(string separador = " | ")
        {
            return ApplyRegraUnariaDoubleProp == null || ApplyRegraUnariaDoubleProp.Count <= 0 ? "" :
                string.Join(
                    separador,
                    ApplyRegraUnariaDoubleProp?.Select(c => string.Format("({0}) {1} {2}: {3} {4}, {5} {6}",
                        c.RULE,
                        c.InputFormula?.NumeroFormula, c.InputFormula,
                        c.OutputFormula1?.NumeroFormula, c.OutputFormula1,
                        c.OutputFormula2?.NumeroFormula, c.OutputFormula2))?.ToList()
                );
        }

        private string toStrRegraBeta(string separador = " | ")
        {
            return ApplyRegraBeta == null || ApplyRegraBeta.Count <= 0 ? "" :
                string.Join(
                    separador,
                    ApplyRegraBeta?.Select(c => string.Format("({0}) {1} {2}: {3} {4}, {5} {6}",
                        c.RULE,
                        c.InputFormula?.NumeroFormula, c.InputFormula,
                        c.OutputFormula1?.NumeroFormula, c.OutputFormula1,
                        c.OutputFormula2?.NumeroFormula, c.OutputFormula2))?.ToList()
                );
        }
        #endregion

        public void Dispose()
        {
            DadosConsumo = null;
            isClosed = false;
            RamosAbertosFechados = null;
            Alturas = null;
            Contradicoes?.Clear();
            Contradicoes = null;
            ApplyRegraUnarias?.Clear();
            ApplyRegraUnarias = null;
            ApplyRegraBinarias?.Clear();
            ApplyRegraBinarias = null;
            Complexidade = Bifurcacoes = NumeroAtomosLivres = NumeroFormulas = NumeroConectores = 0;
        }

    }
}