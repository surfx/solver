using classes.auxiliar;
using classes.formulas;

namespace classes.parser
{
    // item de uma lista de análise sintática
    public class ItemList
    {

        public AtomoConector? AtomoConectorProp { get; set; }
        public bool? Parentesis { get; set; } // true: (, false: )
        public ESimbolo? Simbolo { get; set; } // E, OU, IMPLICA
        public string? Negacao { get; set; }

        public ItemList()
        {
            AtomoConectorProp = null; Parentesis = null; Simbolo = null; Negacao = null;
        }

        public override string? ToString()
        {
            if (AtomoConectorProp != null) { return AtomoConectorProp.ToString(); }
            if (Simbolo != null) { return Auxiliar.toSimbolo(Simbolo); }
            if (Negacao != null && !string.IsNullOrEmpty(Negacao)) { return Negacao; }
            if (Parentesis != null) { return (bool)Parentesis ? "(" : ")"; }
            return "";
        }

    }
}