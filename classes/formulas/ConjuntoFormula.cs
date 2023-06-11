namespace classes.formulas
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

        #region util

        public int numeroConectores()
        {
            if (AtomoConectorProp == null || AtomoConectorProp.isAtomo) { return 0; }
            return numeroConectoresAux(AtomoConectorProp);
        }

        private int numeroConectoresAux(AtomoConector ac)
        {
            if (ac == null || ac.isAtomo) { return 0; }
            return 1 + numeroConectoresAux(ac.ConectorProp.Esquerda) + numeroConectoresAux(ac.ConectorProp.Direita);
        }

        #endregion

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not ConjuntoFormula) { return false; }
            ConjuntoFormula o = (ConjuntoFormula)obj;
            return Simbolo == o.Simbolo &&
                    (AtomoConectorProp != null && AtomoConectorProp.Equals(o.AtomoConectorProp));
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