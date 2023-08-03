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
            if (f == null || (f.Esquerda == null && f.Direita == null)) { return 0; }
            return
                (f.Esquerda != null && f.Direita != null ? 1 : 0) +
                (f.Esquerda == null ? 0 : bifurcacoes(f.Esquerda)) +
                (f.Direita == null ? 0 : bifurcacoes(f.Direita))
                ;
        }
        #endregion

        #region número átomos
        // átomos "livres"
        public int numeroAtomosLivres(Formulas f)
        {
            return f == null ? 0 :
                (f.LConjuntoFormula == null || f.LConjuntoFormula.Count <= 0 ? 0 :
                    f.LConjuntoFormula.Sum(cf => cf == null || cf.AtomoConectorProp == null || cf.AtomoConectorProp.isConector ? 0 : 1)) +
                (f.Esquerda == null ? 0 : numeroAtomosLivres(f.Esquerda)) +
                (f.Direita == null ? 0 : numeroAtomosLivres(f.Direita));
        }
        #endregion

        #region frequência átomos conectores
        public Dictionary<string, int>? frequenciaAtomosConectores(Formulas f, bool ignorarSimbolo /*ignorar T e F*/ = false, bool apenasAtomos = true)
        {
            if (f == null) { return null; }
            Dictionary<string, int> rt = new();
            if (f.LConjuntoFormula != null && f.LConjuntoFormula.Count > 0)
            {
                f.LConjuntoFormula.ForEach(cf =>
                {
                    if (cf == null || cf.AtomoConectorProp == null) { return; }
                    if (apenasAtomos && cf.AtomoConectorProp.isConector) { return; }
                    string key = ignorarSimbolo ? cf.AtomoConectorProp.ToString() : cf.ToString();
                    if (rt.ContainsKey(key))
                    {
                        rt[key] += 1;
                    }
                    else
                    {
                        rt.Add(key, 1);
                    }
                });
            }
            Dictionary<string, int>? aux = null;
            if (f.Esquerda != null)
            {
                aux = frequenciaAtomosConectores(f.Esquerda, ignorarSimbolo, apenasAtomos);
                if (aux != null)
                {
                    foreach (KeyValuePair<string, int> entry in aux)
                    {
                        if (rt.ContainsKey(entry.Key))
                        {
                            rt[entry.Key] += entry.Value;
                            continue;
                        }
                        rt.Add(entry.Key, entry.Value);
                    }
                }
            }
            if (f.Direita != null)
            {
                aux = frequenciaAtomosConectores(f.Direita, ignorarSimbolo, apenasAtomos);
                if (aux != null)
                {
                    foreach (KeyValuePair<string, int> entry in aux)
                    {
                        if (rt.ContainsKey(entry.Key))
                        {
                            rt[entry.Key] += entry.Value;
                            continue;
                        }
                        rt.Add(entry.Key, entry.Value);
                    }
                }
            }
            return rt;
        }
        #endregion

        #region número de fórmulas
        public int numeroFormulas(Formulas f)
        {
            return f == null ? 0 :
                (f.LConjuntoFormula == null || f.LConjuntoFormula.Count <= 0 ? 0 : f.LConjuntoFormula.Count) +
                (f.Esquerda == null ? 0 : numeroFormulas(f.Esquerda)) +
                (f.Direita == null ? 0 : numeroFormulas(f.Direita));
        }
        #endregion

    }
}