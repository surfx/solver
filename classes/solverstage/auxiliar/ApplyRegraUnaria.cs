namespace classes.solverstage.auxiliar
{
    public class ApplyRegraUnaria<T> : IDisposable
    {
        public string RULE { get; set; } = string.Empty;
        public T? InputFormula { get; set; } = default;
        public T? OutputFormula { get; set; } = default;

        public ApplyRegraUnaria(string rule = "", T? inputFormula = default, T? outputFormula = default)
        {
            RULE = rule;
            InputFormula = inputFormula;
            OutputFormula = outputFormula;
        }

        public override string ToString()
        {
            return string.Format(
                "({0}) {1}: {2}", RULE, InputFormula, OutputFormula
            );
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InputFormula?.GetHashCode(), OutputFormula?.GetHashCode(), RULE.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj is ApplyRegraUnaria<T> o && o.GetHashCode() == this.GetHashCode();
        }

        public void Dispose()
        {
            InputFormula = default;
            OutputFormula = default;
            RULE = string.Empty;
        }

    }
}