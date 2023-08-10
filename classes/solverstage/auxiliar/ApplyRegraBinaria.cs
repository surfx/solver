namespace classes.solverstage.auxiliar
{
    public class ApplyRegraBinaria<T> : IDisposable
    {
        public string RULE { get; set; } = string.Empty;
        public T? InputFormula1 { get; set; } = default;
        public T? InputFormula2 { get; set; } = default;
        public T? OutputFormula { get; set; } = default;

        public ApplyRegraBinaria(string rule = "", T? inputFormula1 = default, T? inputFormula2 = default, T? outputFormula = default)
        {
            RULE = rule;
            InputFormula1 = inputFormula1;
            InputFormula2 = inputFormula2;
            OutputFormula = outputFormula;
        }

        public override string ToString()
        {
            return string.Format(
                "({0}) {1}, {2}: {3}", RULE, InputFormula1, InputFormula2, OutputFormula
            );
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InputFormula1?.GetHashCode(), InputFormula2?.GetHashCode(), OutputFormula?.GetHashCode(), RULE.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj is ApplyRegraBinaria<T> o && o.GetHashCode() == this.GetHashCode();
        }

        public void Dispose()
        {
            InputFormula1 = default;
            InputFormula2 = default;
            OutputFormula = default;
            RULE = string.Empty;
        }

    }
}