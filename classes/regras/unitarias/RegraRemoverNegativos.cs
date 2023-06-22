using classes.formulas;

namespace classes.regras.unitarias
{
    /* 
        Obs: Não aplicar, mas sim as regras RegraFalsoNegativo e RegraTrueNegativo

        T (¬¬¬(A → ¬¬¬B)) ˄ (¬(C ˅ ¬¬F))
        T (¬(A → ¬B)) ˄ (¬(C ˅ F))
    */
    public class RegraRemoverNegativos : IRegraUnaria
    {
        public string RULE { get => "¬¬¬ : ¬"; }

        public bool isValid(ConjuntoFormula cf)
        {
            if (cf == null || cf.AtomoConectorProp == null ||
                (cf.AtomoConectorProp.isAtomo && cf.AtomoConectorProp.AtomoProp != null && cf.AtomoConectorProp.AtomoProp.NumeroNegados <= 1) ||
                (cf.AtomoConectorProp.isConector && cf.AtomoConectorProp.ConectorProp == null)
            ) { return false; }
            return isValid(cf.AtomoConectorProp);
        }

        public ConjuntoFormula? apply(ConjuntoFormula cf)
        {
            if (!isValid(cf)) { return cf; }

            AtomoConector ac = null;
            if (cf.AtomoConectorProp.isAtomo)
            {
                ac = new AtomoConector(cf.AtomoConectorProp.AtomoProp.copy());
                ac.AtomoProp.NumeroNegados = cf.AtomoConectorProp.AtomoProp.NumeroNegados % 2 == 0 ? 0 : 1;
            }
            else if (cf.AtomoConectorProp.isConector)
            {
                ac = new AtomoConector(cf.AtomoConectorProp.ConectorProp.copy());
                ac.ConectorProp.NumeroNegados = cf.AtomoConectorProp.ConectorProp.NumeroNegados % 2 == 0 ? 0 : 1;
            }

            ac = apply(ac);
            return new ConjuntoFormula(cf.Simbolo, ac);
        }

        #region auxiliar

        private bool isValid(AtomoConector? ac)
        {
            if (ac == null) { return false; }
            if (ac.isAtomo && ac.AtomoProp != null && ac.AtomoProp.NumeroNegados >= 2) { return true; }
            if (!ac.isConector || ac.ConectorProp == null) { return false; }
            return ac.ConectorProp.NumeroNegados >= 2 || isValid(ac.ConectorProp.Esquerda) || isValid(ac.ConectorProp.Direita);
        }

        private AtomoConector? apply(AtomoConector ac)
        {
            // 
            if (ac == null || !isValid(ac)) { return ac; }

            AtomoConector acAux = null;
            if (ac.isAtomo)
            {
                acAux = new AtomoConector(ac.AtomoProp.copy());
                acAux.AtomoProp.NumeroNegados = ac.AtomoProp.NumeroNegados % 2 == 0 ? 0 : 1;
            }
            else if (ac.isConector)
            {
                acAux = new AtomoConector(ac.ConectorProp.copy());
                acAux.ConectorProp.NumeroNegados = ac.ConectorProp.NumeroNegados % 2 == 0 ? 0 : 1;
                acAux.ConectorProp.Esquerda = apply(acAux.ConectorProp.Esquerda);
                acAux.ConectorProp.Direita = apply(acAux.ConectorProp.Direita);
            }
            return acAux;
        }

        #endregion

    }
}