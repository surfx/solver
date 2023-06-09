namespace clases.formulas
{
    public class ConjuntoFormula
    {

        public bool Simbolo { get; set; } // T / F
        public AtomoConector? AtomoConectorProp { get; set; }

        public ConjuntoFormula(bool simbolo, AtomoConector atomoConector)
        {
            Simbolo = simbolo;
            AtomoConectorProp = atomoConector;
        }

        public ConjuntoFormula(AtomoConector atomoConector)
        {
            Simbolo = true;
            AtomoConectorProp = atomoConector;
        }

        public ConjuntoFormula(bool simbolo, IConversor conversor)
        {
            Simbolo = simbolo;
            AtomoConectorProp = conversor == null ? null : conversor.toAtomoConector();
        }

        public ConjuntoFormula(IConversor conversor)
        {
            Simbolo = true;
            AtomoConectorProp = conversor == null ? null : conversor.toAtomoConector();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            return string.Format("{0} {1}", Simbolo ? "T" : "F", AtomoConectorProp);
        }

    }
}