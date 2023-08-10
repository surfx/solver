namespace classes.solverstage.auxiliar
{
    public class ApplyRegraUnariaDouble<T> : IDisposable
    {
        public string RULE { get; set; } = string.Empty;
        public T? InputFormula { get; set; } = default;
        public T? OutputFormula1 { get; set; } = default;
        public T? OutputFormula2 { get; set; } = default;

        public ApplyRegraUnariaDouble(string rule = "", T? inputFormula = default, T? outputFormula1 = default, T? outputFormula2 = default)
        {
            RULE = rule;
            InputFormula = inputFormula;
            OutputFormula1 = outputFormula1;
            OutputFormula2 = outputFormula2;
        }

        public override string ToString()
        {
            return string.Format(
                "({0}) {1}: {2}, {3}", RULE, InputFormula, OutputFormula1, OutputFormula2
            );
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InputFormula?.GetHashCode(), OutputFormula1?.GetHashCode(), OutputFormula2.GetHashCode(), RULE.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj is ApplyRegraUnariaDouble<T> o && o.GetHashCode() == this.GetHashCode();
        }

        public void Dispose()
        {
            InputFormula = default;
            OutputFormula1 = default;
            OutputFormula2 = default;
            RULE = string.Empty;
        }

    }
}