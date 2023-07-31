using classes.solverstage;

namespace classes.auxiliar.saidas.print
{
    // fluent builder pattern
    // https://www.digitalocean.com/community/tutorials/builder-design-pattern-in-java
    // https://medium.com/@martinstm/fluent-builder-pattern-c-4ac39fafcb0b
    public class PFormulasToString
    {
        public Formulas Formulas { get; set; }
        public bool PrintAllClosedOpen { get; set; }
        public bool PrintLastClosedOpen { get; set; }

        private PFormulasToString() { }
        private PFormulasToString(Formulas formulas) { this.Formulas = formulas; }

        public class PFormulasToStringBuilder
        {
            private PFormulasToString _pFormulasToString = new PFormulasToString();

            private PFormulasToStringBuilder(Formulas formulas)
            {
                _pFormulasToString.Formulas = formulas;
                _pFormulasToString.PrintAllClosedOpen = false;
                _pFormulasToString.PrintLastClosedOpen = false;
            }
            public static PFormulasToStringBuilder Init(Formulas formulas)
            {
                return new PFormulasToStringBuilder(formulas);
            }
            public PFormulasToString Build() => _pFormulasToString;

            public PFormulasToStringBuilder SetFormulas(Formulas formulas) { _pFormulasToString.Formulas = formulas; return this; }
            public PFormulasToStringBuilder withPrintAllClosedOpen() { _pFormulasToString.PrintAllClosedOpen = true; return this; }
            public PFormulasToStringBuilder withPrintLastClosedOpen() { _pFormulasToString.PrintLastClosedOpen = true; return this; }

            #region copy
            public PFormulasToStringBuilder copy()
            {
                return copy(_pFormulasToString.Formulas);
            }
            public PFormulasToStringBuilder copy(Formulas formulas)
            {
                PFormulasToStringBuilder rt = Init(formulas);
                if (_pFormulasToString.PrintAllClosedOpen) { rt.withPrintAllClosedOpen(); }
                if (_pFormulasToString.PrintLastClosedOpen) { rt.withPrintLastClosedOpen(); }
                return rt;
            }
            #endregion

        }
    }

}