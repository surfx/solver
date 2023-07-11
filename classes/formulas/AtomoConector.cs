namespace classes.formulas
{
    public class AtomoConector
    {

        public Conector? ConectorProp { get; set; }
        public Atomo? AtomoProp { get; set; }

        public bool isConector { get => ConectorProp != null; }
        public bool isAtomo { get => AtomoProp != null; }

        public AtomoConector(Atomo? atomoProp = null)
        {
            AtomoProp = atomoProp;
        }

        public AtomoConector(Conector? conectorProp = null)
        {
            ConectorProp = conectorProp;
        }

        public AtomoConector(Conector? conectorProp = null, Atomo? atomoProp = null)
        {
            ConectorProp = conectorProp;
            AtomoProp = atomoProp;
        }

        #region util

        public AtomoConector copy()
        {
            return new AtomoConector(ConectorProp, AtomoProp);
        }

        public bool isNegado { get => isConector ? ConectorProp.isNegado : AtomoProp.isNegado ; }

        #endregion

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not AtomoConector) { return false; }
            AtomoConector o = (AtomoConector)obj;
            return isConector ? ConectorProp.Equals(o.ConectorProp) : AtomoProp.Equals(o.AtomoProp);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string? ToString()
        {
            if (ConectorProp == null && AtomoProp == null) { return ""; }
            return isConector ? ConectorProp?.ToString() : AtomoProp?.ToString();
        }

    }
}