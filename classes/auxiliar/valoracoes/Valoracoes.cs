using classes.formulas;
using classes.solverstage;

namespace classes.auxiliar.valoracoes
{
    public class Valoracoes
    {

        #region complexidade
        public int complexidade(Formulas f)
        {
            return f == null ? 0 :
                (f.LConjuntoFormula == null || f.LConjuntoFormula.Count <= 0 ? 0 : f.LConjuntoFormula.Sum(complexidade)) +
                (f.Esquerda == null ? 0 : complexidade(f.Esquerda)) +
                (f.Direita == null ? 0 : complexidade(f.Direita));
        }

        public int complexidade(ConjuntoFormula cf)
        {
            if (cf == null || cf.AtomoConectorProp == null) { return 0; }
            return complexidade(cf.AtomoConectorProp);
        }

        private int complexidade(AtomoConector? ac)
        {
            if (ac == null) { return 0; }
            if (ac.isAtomo) { return 1; }
            if (ac.ConectorProp == null) { return 0; }
            // símbolo valora 1
            return 1 + (ac.ConectorProp.Esquerda == null ? 0 : complexidade(ac.ConectorProp.Esquerda)) + (ac.ConectorProp.Direita == null ? 0 : complexidade(ac.ConectorProp.Direita));
        }
        #endregion

        #region bifurcacoes
        public int bifurcacoes(Formulas f)
        {
            if (f == null) { return 0; }

            return f.Esquerda == null || f.Direita == null ? 0 :
                (1 +
                (f.Esquerda == null ? 0 : bifurcacoes(f.Esquerda)) +
                (f.Direita == null ? 0 : bifurcacoes(f.Direita))
                );
        }
        #endregion


    }
}