using classes.auxiliar.saidas.print;

namespace classes.solverstage.parameters
{
    // fluent builder pattern
    public class SolverParameters
    {
        public Formulas Formulas { get; set; }
        public PFormulasToImage.PFormulasToImageBuilder? PFormulasToImageBuilderParam { get; set; }

        private SolverParameters(Formulas formulas) { this.Formulas = formulas; }

        public class SolverParametersBuilder
        {
            private SolverParameters _solveParameters;

            private SolverParametersBuilder(Formulas formulas)
            {
                _solveParameters = new(formulas)
                {
                    PFormulasToImageBuilderParam = null
                };
            }
            public static SolverParametersBuilder Init(Formulas formulas)
            {
                return new SolverParametersBuilder(formulas);
            }
            public SolverParameters Build() => _solveParameters;

            public SolverParametersBuilder SetFormulas(Formulas formulas) { _solveParameters.Formulas = formulas; return this; }
            public SolverParametersBuilder PFormulasToImageBuilderParam(PFormulasToImage.PFormulasToImageBuilder pFormulasToImageBuilderParam) { _solveParameters.PFormulasToImageBuilderParam = pFormulasToImageBuilderParam; return this; }

        }

    }

}