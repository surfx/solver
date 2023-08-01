using classes.solverstage;

namespace classes.auxiliar.saidas.print
{
    // fluent builder pattern
    // https://www.digitalocean.com/community/tutorials/builder-design-pattern-in-java
    // https://medium.com/@martinstm/fluent-builder-pattern-c-4ac39fafcb0b
    public class PFormulasToImage
    {
        public Formulas Formulas { get; set; }
        public string PathImgSaida { get; set; }
        public bool DivisoriaArvore { get; set; }
        public bool DrawSquare { get; set; }
        public float HChar { get; set; } // height de 1 char
        public float Wchar { get; set; } // width de 1 char
        public float IncrementX { get; set; }
        public float IncrementY { get; set; }
        public float EspacamentoDivisorY { get; set; }
        public bool PrintAllClosedOpen { get; set; }
        public bool PrintFormulaNumber { get; set; }
        public bool PrintDotTreeMode { get; set; }

        private PFormulasToImage() { }
        private PFormulasToImage(Formulas formulas) { this.Formulas = formulas; }

        public class PFormulasToImageBuilder
        {
            private PFormulasToImage _pFormulasToImage = new PFormulasToImage();

            private PFormulasToImageBuilder(Formulas formulas, float hchar = 15.0f, float wchar = 7.55f)
            {
                // Consolas 10
                _pFormulasToImage.HChar = hchar <= 0 ? 15.0f : hchar;
                _pFormulasToImage.Wchar = wchar <= 0 ? 7.55f : wchar;
                _pFormulasToImage.IncrementX = 20.0f;
                _pFormulasToImage.IncrementY = 20.0f;
                _pFormulasToImage.EspacamentoDivisorY = 25.0f;

                _pFormulasToImage.DivisoriaArvore = true;
                _pFormulasToImage.Formulas = formulas;
                _pFormulasToImage.PrintAllClosedOpen = false;
                _pFormulasToImage.PrintFormulaNumber = false;
                _pFormulasToImage.PrintDotTreeMode = false;
            }
            public static PFormulasToImageBuilder Init(Formulas formulas)
            {
                return new PFormulasToImageBuilder(formulas);
            }
            public PFormulasToImage Build() => _pFormulasToImage;

            public PFormulasToImageBuilder SetFormulas(Formulas formulas) { _pFormulasToImage.Formulas = formulas; return this; }
            public PFormulasToImageBuilder SetPathImgSaida(string pathImgSaida) { _pFormulasToImage.PathImgSaida = pathImgSaida; return this; }
            public PFormulasToImageBuilder withDivisoriaArvore() { _pFormulasToImage.DivisoriaArvore = true; return this; }
            public PFormulasToImageBuilder withDrawSquare() { _pFormulasToImage.DrawSquare = true; return this; }
            public PFormulasToImageBuilder SetHChar(float hChar) { _pFormulasToImage.HChar = hChar; return this; }
            public PFormulasToImageBuilder SetWchar(float wchar) { _pFormulasToImage.Wchar = wchar; return this; }
            public PFormulasToImageBuilder SetIncrementoX(float incrementoX) { _pFormulasToImage.IncrementX = incrementoX; return this; }
            public PFormulasToImageBuilder SetIncrementoY(float incrementY) { _pFormulasToImage.IncrementY = incrementY; return this; }
            public PFormulasToImageBuilder SetEspacamentoDivisorY(float espacamentoDivisorY) { _pFormulasToImage.EspacamentoDivisorY = espacamentoDivisorY; return this; }
            public PFormulasToImageBuilder withPrintAllClosedOpen() { _pFormulasToImage.PrintAllClosedOpen = true; return this; }
            public PFormulasToImageBuilder withPrintFormulaNumber() { _pFormulasToImage.PrintFormulaNumber = true; return this; }
            public PFormulasToImageBuilder withPrintDotTreeMode()
            {
                _pFormulasToImage.PrintAllClosedOpen = false;
                _pFormulasToImage.PrintFormulaNumber = false;
                _pFormulasToImage.PrintDotTreeMode = true;
                return this;
            }

            public PFormulasToImageBuilder copy()
            {
                PFormulasToImageBuilder rt = PFormulasToImageBuilder.Init(_pFormulasToImage.Formulas);
                rt.SetPathImgSaida(_pFormulasToImage.PathImgSaida);
                rt.SetHChar(_pFormulasToImage.HChar);
                rt.SetWchar(_pFormulasToImage.Wchar);
                rt.SetIncrementoX(_pFormulasToImage.IncrementX);
                rt.SetIncrementoY(_pFormulasToImage.IncrementY);
                rt.SetEspacamentoDivisorY(_pFormulasToImage.EspacamentoDivisorY);
                if (_pFormulasToImage.DivisoriaArvore) { rt.withDivisoriaArvore(); }
                if (_pFormulasToImage.DrawSquare) { rt.withDrawSquare(); }
                if (_pFormulasToImage.DrawSquare) { rt.withDrawSquare(); }
                if (_pFormulasToImage.PrintAllClosedOpen) { rt.withPrintAllClosedOpen(); }
                if (_pFormulasToImage.PrintFormulaNumber) { rt.withPrintFormulaNumber(); }
                if (_pFormulasToImage.PrintDotTreeMode) { rt.withPrintDotTreeMode(); }
                return rt;
            }

        }
    }

}
/*
    .withPrintDotTreeMode()
    .SetHChar(8)
    .SetWchar(5)
    .SetEspacamentoDivisorY(8.5f)
*/