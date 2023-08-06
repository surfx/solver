using classes.formulas;

namespace classes.regras.unitarias
{
    /* 
        Obs: Não aplicar, mas sim as regras RegraFalsoNegativo e RegraTrueNegativo

        T (¬¬¬(A → ¬¬¬B)) ˄ (¬(C ˅ ¬¬F))
        T (¬(A → ¬B)) ˄ (¬(C ˅ F))
    */
    [Obsolete, System.Reflection.Obfuscation]
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
            if (!isValid(cf)) { return null; }

            AtomoConector? ac = null;
            if (cf?.AtomoConectorProp?.isAtomo ?? false)
            {
                ac = new AtomoConector(cf?.AtomoConectorProp.AtomoProp?.copy());
                if (ac != null && ac.AtomoProp != null) { ac.AtomoProp.NumeroNegados = cf?.AtomoConectorProp?.AtomoProp?.NumeroNegados % 2 == 0 ? 0 : 1; }
            }
            else if (cf?.AtomoConectorProp?.isConector ?? false)
            {
                ac = new AtomoConector(cf?.AtomoConectorProp?.ConectorProp?.copy());
                if (ac != null && ac.ConectorProp != null) { ac.ConectorProp.NumeroNegados = cf?.AtomoConectorProp?.ConectorProp?.NumeroNegados % 2 == 0 ? 0 : 1; }
            }

            ac = ac == null ? null : apply(ac);
            return new(cf != null && cf.Simbolo, ac);
        }

        #region auxiliar

        private bool isValid(AtomoConector? ac)
        {
            if (ac == null) { return false; }
            if (ac.isAtomo && ac.AtomoProp != null && ac.AtomoProp.NumeroNegados >= 2) { return true; }
            if (!ac.isConector || ac.ConectorProp == null) { return false; }
            return ac.ConectorProp.NumeroNegados >= 2 || isValid(ac.ConectorProp.Esquerda) || isValid(ac.ConectorProp.Direita);
        }

        private AtomoConector? apply(AtomoConector? ac)
        {
            if (ac == null || !isValid(ac)) { return ac; }

            AtomoConector? acAux = null;
            if (ac.isAtomo)
            {
                acAux = new(ac?.AtomoProp?.copy());
                if (acAux.AtomoProp != null) { acAux.AtomoProp.NumeroNegados = ac?.AtomoProp?.NumeroNegados % 2 == 0 ? 0 : 1; }
            }
            else if (ac.isConector)
            {
                acAux = new(ac?.ConectorProp?.copy());
                if (acAux != null && acAux.ConectorProp != null)
                {
                    acAux.ConectorProp.NumeroNegados = ac?.ConectorProp?.NumeroNegados % 2 == 0 ? 0 : 1;
                    acAux.ConectorProp.Esquerda = apply(acAux?.ConectorProp?.Esquerda);
                    acAux.ConectorProp.Direita = apply(acAux?.ConectorProp?.Direita);
                }
            }
            return acAux;
        }

        #endregion

    }
}