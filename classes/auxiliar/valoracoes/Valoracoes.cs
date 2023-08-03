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
                        return;
                    }
                    rt.Add(key, 1);
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

        #region distribuição de frequências de forma geral

        public Dictionary<string, int>? frequenciaAtomos(Formulas f)
        {
            if (f == null) { return null; }
            Dictionary<string, int> rt = frequenciaAtomos(f.LConjuntoFormula);
            if (f.Esquerda != null)
            {
                Dictionary<string, int>? aux = frequenciaAtomos(f.Esquerda);
                foreach (KeyValuePair<string, int> entry in aux)
                {
                    if (entry.Key == null || string.IsNullOrEmpty(entry.Key)) { continue; }
                    if (rt.ContainsKey(entry.Key))
                    {
                        rt[entry.Key] += entry.Value;
                    }
                    else
                    {
                        rt.Add(entry.Key, entry.Value);
                    }
                }
            }
            if (f.Direita != null)
            {
                Dictionary<string, int>? aux = frequenciaAtomos(f.Direita);
                foreach (KeyValuePair<string, int> entry in aux)
                {
                    if (entry.Key == null || string.IsNullOrEmpty(entry.Key)) { continue; }
                    if (rt.ContainsKey(entry.Key))
                    {
                        rt[entry.Key] += entry.Value;
                    }
                    else
                    {
                        rt.Add(entry.Key, entry.Value);
                    }
                }
            }
            return rt;
        }

        public Dictionary<string, int>? frequenciaAtomos(List<ConjuntoFormula> listaFormulas)
        {
            if (listaFormulas == null || listaFormulas.Count <= 0) { return null; }
            Dictionary<string, int> rt = new();
            listaFormulas.ForEach(cf =>
            {
                if (cf == null) { return; }
                Dictionary<string, int>? aux = frequenciaAtomos(cf);
                if (aux == null || aux.Count <= 0) { return; }
                foreach (KeyValuePair<string, int> entry in aux)
                {
                    if (entry.Key == null || string.IsNullOrEmpty(entry.Key)) { continue; }
                    if (rt.ContainsKey(entry.Key))
                    {
                        rt[entry.Key] += entry.Value;
                    }
                    else
                    {
                        rt.Add(entry.Key, entry.Value);
                    }
                }
            });
            return rt;
        }

        public Dictionary<string, int>? frequenciaAtomos(ConjuntoFormula cf)
        {
            if (cf == null || cf.AtomoConectorProp == null) { return null; }
            Dictionary<string, int> rt = new();

            if (cf.AtomoConectorProp.isAtomo && cf.AtomoConectorProp.AtomoProp != null)
            {
                if (rt.ContainsKey(cf.AtomoConectorProp.AtomoProp.Simbolo))
                {
                    rt[cf.AtomoConectorProp.AtomoProp.Simbolo] += 1;
                }
                else
                {
                    rt.Add(cf.AtomoConectorProp.AtomoProp.Simbolo, 1);
                }
            }
            if (cf.AtomoConectorProp.isConector && cf.AtomoConectorProp.ConectorProp != null)
            {
                Dictionary<string, int>? aux = frequenciaAtomos(cf.AtomoConectorProp);
                if (aux != null && aux.Count > 0)
                {
                    foreach (KeyValuePair<string, int> entry in aux)
                    {
                        if (entry.Key == null || string.IsNullOrEmpty(entry.Key)) { continue; }
                        if (rt.ContainsKey(entry.Key))
                        {
                            rt[entry.Key] += entry.Value;
                        }
                        else
                        {
                            rt.Add(entry.Key, entry.Value);
                        }
                    }
                }
            }
            return rt;
        }

        public Dictionary<string, int>? frequenciaAtomos(AtomoConector ac)
        {
            if (ac == null) { return null; }
            Dictionary<string, int> rt = new();

            if (ac.isAtomo && ac.AtomoProp != null)
            {
                if (rt.ContainsKey(ac.AtomoProp.Simbolo))
                {
                    rt[ac.AtomoProp.Simbolo] += 1;
                }
                else
                {
                    rt.Add(ac.AtomoProp.Simbolo, 1);
                }
            }
            if (ac.isConector && ac.ConectorProp != null)
            {
                if (ac.ConectorProp.Esquerda != null)
                {
                    Dictionary<string, int>? aux = frequenciaAtomos(ac.ConectorProp.Esquerda);
                    if (aux != null && aux.Count > 0)
                    {
                        foreach (KeyValuePair<string, int> entry in aux)
                        {
                            if (entry.Key == null || string.IsNullOrEmpty(entry.Key)) { continue; }
                            if (rt.ContainsKey(entry.Key))
                            {
                                rt[entry.Key] += entry.Value;
                            }
                            else
                            {
                                rt.Add(entry.Key, entry.Value);
                            }
                        }
                    }
                }
                if (ac.ConectorProp.Direita != null)
                {
                    Dictionary<string, int>? aux = frequenciaAtomos(ac.ConectorProp.Direita);
                    if (aux != null && aux.Count > 0)
                    {
                        foreach (KeyValuePair<string, int> entry in aux)
                        {
                            if (entry.Key == null || string.IsNullOrEmpty(entry.Key)) { continue; }
                            if (rt.ContainsKey(entry.Key))
                            {
                                rt[entry.Key] += entry.Value;
                            }
                            else
                            {
                                rt.Add(entry.Key, entry.Value);
                            }
                        }
                    }
                }
            }
            return rt;
        }

        #region relações frequências de forma global
        public struct StFrequenciaAtomosRelativos
        {
            public int FrequenciaAtomoFormula { get; set; }
            public int FrequenciaAtomoGlobal { get; set; }
            public float TaxaGlobal { get; set; }

            public override string ToString()
            {
                return string.Format("faf: {0}, fag: {1}, tx: {2}", FrequenciaAtomoFormula, FrequenciaAtomoGlobal, TaxaGlobal);
            }
        }
        public Dictionary<string, StFrequenciaAtomosRelativos>? frequenciaAtomosRelativo(ConjuntoFormula cf, Formulas f)
        {
            if (cf == null || f == null) { return null; }
            return frequenciaAtomosRelativo(cf, frequenciaAtomos(f));
        }
        public Dictionary<string, StFrequenciaAtomosRelativos>? frequenciaAtomosRelativo(ConjuntoFormula cf, List<ConjuntoFormula> formulas)
        {
            if (cf == null || formulas == null || formulas.Count <= 0) { return null; }
            return frequenciaAtomosRelativo(cf, frequenciaAtomos(formulas));
        }
        public Dictionary<string, StFrequenciaAtomosRelativos>? frequenciaAtomosRelativo(ConjuntoFormula cf, Dictionary<string, int> dicFrequenciasGlobal)
        {
            if (cf == null || dicFrequenciasGlobal == null || dicFrequenciasGlobal.Count <= 0) { return null; }
            Dictionary<string, int>? dicFreqquenciasCF = frequenciaAtomos(cf);
            if (dicFreqquenciasCF == null || dicFreqquenciasCF.Count <= 0) { return null; }
            Dictionary<string, StFrequenciaAtomosRelativos> rt = new();
            foreach (KeyValuePair<string, int> entry in dicFreqquenciasCF)
            {
                if (entry.Key == null || string.IsNullOrEmpty(entry.Key)) { continue; }
                StFrequenciaAtomosRelativos staux = rt.ContainsKey(entry.Key) ? rt[entry.Key] : new();
                staux.FrequenciaAtomoFormula = entry.Value;
                if (dicFrequenciasGlobal.ContainsKey(entry.Key))
                {
                    staux.FrequenciaAtomoGlobal = dicFrequenciasGlobal[entry.Key];
                    if (staux.FrequenciaAtomoFormula > 0 && staux.FrequenciaAtomoGlobal > 0)
                    {
                        staux.TaxaGlobal = (staux.FrequenciaAtomoFormula * 1.0f) / (staux.FrequenciaAtomoGlobal * 1.0f);
                    }
                }
                if (rt.ContainsKey(entry.Key))
                {
                    rt[entry.Key] = staux;
                }
                else
                {
                    rt.Add(entry.Key, staux);
                }
            }
            return rt;
        }
        #endregion

        #endregion

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

        #region ramos fechados
        public struct StRamosAbertosFechados
        {
            public int Abertos { get; set; }
            public int Fechados { get; set; }
            public override string ToString()
            {
                return string.Format("Abertos: {0}, Fechados: {1}", Abertos, Fechados);
            }
        }
        public StRamosAbertosFechados ramosAbertosEFechados(Formulas f)
        {
            return new()
            {
                Abertos = ramosAbertosEFechados(f, false),
                Fechados = ramosAbertosEFechados(f, true)
            };
        }
        private int ramosAbertosEFechados(Formulas f, bool isClosed = true)
        {
            return f == null ? 0 :
                (f.Esquerda == null && f.Direita == null ?
                    (f.isClosed ? (isClosed ? 1 : 0) : (isClosed ? 0 : 1)) : 0
                ) +
                (f.Esquerda == null ? 0 : ramosAbertosEFechados(f.Esquerda, isClosed)) +
                (f.Direita == null ? 0 : ramosAbertosEFechados(f.Direita, isClosed));
        }
        #endregion

        #region altura
        public struct StAlturas
        {
            public int MaxHeight { get; set; }
            public int MinHeight { get; set; }
            public float AvgHeight { get; set; }
            public override readonly string ToString()
            {
                return string.Format("min: {0}, max: {1}, avg: {2}", MinHeight, MaxHeight, AvgHeight);
            }
        }
        public StAlturas? altura(Formulas f)
        {
            if (f == null) { return null; }
            StAlturas rt = new();
            rt.MinHeight = rt.MaxHeight = f.LConjuntoFormula == null || f.LConjuntoFormula.Count <= 0 ? 0 : f.LConjuntoFormula.Count;
            rt.MinHeight += Math.Min(f.Esquerda == null ? 0 : alturaMaxMin(f.Esquerda, false), f.Direita == null ? 0 : alturaMaxMin(f.Direita, false));
            rt.MaxHeight += Math.Max(f.Esquerda == null ? 0 : alturaMaxMin(f.Esquerda, true), f.Direita == null ? 0 : alturaMaxMin(f.Direita, true));
            rt.AvgHeight = rt.MinHeight <= 0 || rt.MaxHeight <= 0 ? 0.0f : (rt.MinHeight * 1.0f / rt.MaxHeight * 1.0f);
            return rt;
        }

        public int alturaMaxMin(Formulas f, bool max = true)
        {
            int auxEsquerda = f.Esquerda == null ? 0 : alturaMaxMin(f.Esquerda, max);
            int auxDireita = f.Direita == null ? 0 : alturaMaxMin(f.Direita, max);
            return f == null ? 0 :
                (f.LConjuntoFormula == null || f.LConjuntoFormula.Count <= 0 ? 0 : f.LConjuntoFormula.Count) +
                (max ? Math.Max(auxEsquerda, auxDireita) : Math.Min(auxEsquerda, auxDireita))
                ;
        }
        #endregion

        #region número de conectores
        public int numeroConectores(Formulas f)
        {
            return f == null ? 0 :
                (f.LConjuntoFormula == null || f.LConjuntoFormula.Count <= 0 ? 0 : f.LConjuntoFormula.Sum(numeroConectores)) +
                (f.Esquerda == null ? 0 : numeroConectores(f.Esquerda)) +
                (f.Direita == null ? 0 : numeroConectores(f.Direita));
        }
        public int numeroConectores(ConjuntoFormula cf)
        {
            if (cf == null || cf.AtomoConectorProp == null) { return 0; }
            return numeroConectores(cf.AtomoConectorProp);
        }
        private int numeroConectores(AtomoConector ac)
        {
            if (ac == null || ac.isAtomo || ac.ConectorProp == null) { return 0; }
            if ((ac.ConectorProp.Esquerda != null && ac.ConectorProp.Esquerda.isAtomo) && (ac.ConectorProp.Direita != null && ac.ConectorProp.Direita.isAtomo)) { return 1; }
            return (((ac.ConectorProp.Esquerda != null && ac.ConectorProp.Esquerda.isAtomo) || (ac.ConectorProp.Direita != null && ac.ConectorProp.Direita.isAtomo)) ? 1 : 0) +
                (ac.ConectorProp.Esquerda == null || ac.ConectorProp.Esquerda.isAtomo ? 0 : numeroConectores(ac.ConectorProp.Esquerda)) +
                (ac.ConectorProp.Direita == null || ac.ConectorProp.Direita.isAtomo ? 0 : numeroConectores(ac.ConectorProp.Direita));
        }
        #endregion


    }
}