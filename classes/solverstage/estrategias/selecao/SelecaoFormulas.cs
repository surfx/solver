using classes.formulas;
using static classes.auxiliar.valoracoes.Valoracoes;

namespace classes.solverstage.estrategias.selecao
{

    public class SelecaoFormulas
    {

        #region maior taxa
        public static void ordernarMaiorTaxa(Formulas f)
        {
            ordernarMaiorTaxa(f.LConjuntoFormula, f);
        }
        public static void ordernarMaiorTaxa(List<ConjuntoFormula>? lc, Formulas f)
        {
            if (f == null || f.LConjuntoFormula == null || lc == null || lc.Count <= 0) { return; }
            lc.Sort((cf1, cf2) =>
            {
                if (cf1.Equals(cf2)) { return 0; }

                Dictionary<string, StFrequenciaAtomosRelativos>? aux1 = frequenciaAtomosRelativo(cf1, f);
                Dictionary<string, StFrequenciaAtomosRelativos>? aux2 = frequenciaAtomosRelativo(cf2, f);
                float tx1 = 0.0f, tx2 = 0.0f;
                if (aux1 != null) foreach (KeyValuePair<string, StFrequenciaAtomosRelativos> entry in aux1) { tx1 += entry.Value.TaxaGlobal; }
                if (aux2 != null) foreach (KeyValuePair<string, StFrequenciaAtomosRelativos> entry in aux2) { tx2 += entry.Value.TaxaGlobal; }
                return tx1 > tx2 ? -1 : 1;
            });
        }
        public static void ordernarMaiorTaxa(List<ConjuntoFormula>? lc, List<ConjuntoFormula?>? formulas)
        {
            if (formulas == null || formulas.Count <= 0 || lc == null || lc.Count <= 0) { return; }
            lc.Sort((cf1, cf2) =>
            {
                if (cf1.Equals(cf2)) { return 0; }

                Dictionary<string, StFrequenciaAtomosRelativos>? aux1 = frequenciaAtomosRelativo(cf1, formulas);
                Dictionary<string, StFrequenciaAtomosRelativos>? aux2 = frequenciaAtomosRelativo(cf2, formulas);
                float tx1 = 0.0f, tx2 = 0.0f;
                if (aux1 != null) foreach (KeyValuePair<string, StFrequenciaAtomosRelativos> entry in aux1) { tx1 += entry.Value.TaxaGlobal; }
                if (aux2 != null) foreach (KeyValuePair<string, StFrequenciaAtomosRelativos> entry in aux2) { tx2 += entry.Value.TaxaGlobal; }
                return tx1 > tx2 ? -1 : 1;
            });
        }
        #endregion

    }
}