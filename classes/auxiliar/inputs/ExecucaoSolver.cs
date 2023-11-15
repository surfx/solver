using classes.auxiliar.saidas.print;
using classes.formulas;
using classes.parser;
using classes.solverstage;
using classes.solverstage.auxiliar;
using classes.solverstage.parameters;

namespace classes.auxiliar.inputs
{

    public class ExecucaoSolver
    {

        public static void executarSolver(ParametrosEntrada? pe)
        {
            if (pe == null || !AvaliarParametrosEntrada.isParametrosOk(pe)) { return; }

            Formulas? formulas = analiseFileFormulas(pe.Value);
            if (formulas == null) { Console.WriteLine("Erro ao recuperar as fórmulas"); return; }

            bool hasImageFile = !string.IsNullOrEmpty(pe.Value.fileImagem) && existeDiretorio(pe.Value.fileImagem);

            Stage stage = new();
            SolverParameters.SolverParametersBuilder solverParametersBuilder = SolverParameters.SolverParametersBuilder.Init(formulas);
            if (hasImageFile)
            {
                solverParametersBuilder.PFormulasToImageBuilderParam(PFormulasToImage.PFormulasToImageBuilder.Init(formulas)
                    .SetPathImgSaida(pe.Value.fileImagem)
                    .withDivisoriaArvore()
                    .withPrintAllClosedOpen()
                    .withPrintFormulaNumber());
            }
            SolverParameters solverParameters = solverParametersBuilder.Build();

            DadosSolver? ds = stage.solve(solverParameters);
            if (ds != null)
            {
                saveFile(pe.Value.fileEstatisticas, ds.ToString());
            }
            ds?.Dispose();

            if (hasImageFile && solverParameters.PFormulasToImageBuilderParam != null)
            {
                new ImageFormulas(solverParameters.PFormulasToImageBuilderParam).formulasToImage();
            }
            solverParameters.Formulas.Dispose(); formulas.Dispose();
            stage.Dispose();

            Console.WriteLine("terminado");
        }

        private static Formulas? analiseFileFormulas(ParametrosEntrada pe)
        {
            if (string.IsNullOrEmpty(pe.fileFormulas)) { return null; }
            if (!File.Exists(pe.fileFormulas)) { return null; }

            Formulas f = new();
            Parser parser = new();

            string[] formulas = File.ReadAllLines(pe.fileFormulas);
            foreach (string formula in formulas)
            {
                ConjuntoFormula? cf = parser.parserCF(formula);
                if (cf == null)
                {
                    Console.WriteLine("Erro ao parsear a fórmula: {0}", formula);
                    continue;
                }
                f.addConjuntoFormula(cf);
            }

            return f;
        }

        private static void saveFile(string file, string conteudo)
        {
            if (!existeDiretorio(file)) { return; }
            File.WriteAllText(file, conteudo);
        }

        private static bool existeDiretorio(string filePath)
        {
            string diretorio = filePath.Replace("/", @"\");
            if (!diretorio.EndsWith(@"\"))
            {
                diretorio = diretorio.Substring(0, diretorio.LastIndexOf(@"\"));
            }
            return Directory.Exists(diretorio);
        }

    }



}