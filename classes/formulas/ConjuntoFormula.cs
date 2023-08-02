namespace classes.formulas
{
    public class ConjuntoFormula
    {

        public int NumeroFormula { get; set; }
        public bool Simbolo { get; set; } // T / F
        public AtomoConector? AtomoConectorProp { get; set; }

        public ConjuntoFormula(bool simbolo = true, AtomoConector? atomoConector = null, IConversor? conversor = null, int numeroFormula = -1)
        {
            Simbolo = simbolo;
            AtomoConectorProp = atomoConector != null ? atomoConector : (conversor == null ? null : conversor.toAtomoConector());
            NumeroFormula = numeroFormula;
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