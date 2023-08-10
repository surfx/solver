namespace classes.solverstage.auxiliar
{
    public class Contradicoes<T> : IDisposable
    {
        public T? Formula1 { get; set; } = default;
        public T? Formula2 { get; set; } = default;

        public Contradicoes(T? formula1 = default, T? formula2 = default)
        {
            Formula1 = formula1;
            Formula2 = formula2;
        }

        public override string ToString()
        {
            return string.Format(
                "{0} e {1}", Formula1, Formula2
            );
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Formula1?.GetHashCode(), Formula2?.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return obj is Contradicoes<T> o && o.GetHashCode() == this.GetHashCode();
        }

        public void Dispose()
        {
            Formula1 = default;
            Formula2 = default;
        }

    }
}