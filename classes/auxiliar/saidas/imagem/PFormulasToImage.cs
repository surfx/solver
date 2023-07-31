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
        public float IncrementX { get; set; }
        public float IncrementY { get; set; }
        public float EspacamentoDivisorY { get; set; }
        public bool PrintAllClosedOpen { get; set; }

        private PFormulasToImage() { }
        private PFormulasToImage(Formulas formulas) { this.Formulas = formulas; }

        public class PFormulasToImageBuilder
        {
            private PFormulasToImage _pFormulasToImage = new PFormulasToImage();

            private PFormulasToImageBuilder(Formulas formulas)
            {

                _pFormulasToImage.IncrementX = 20.0f;
                _pFormulasToImage.IncrementY = 20.0f;
                _pFormulasToImage.EspacamentoDivisorY = 25.0f;

                _pFormulasToImage.DivisoriaArvore = true;
                _pFormulasToImage.Formulas = formulas;
                _pFormulasToImage.PrintAllClosedOpen = false;


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
            public PFormulasToImageBuilder SetIncrementoX(float incrementoX) { _pFormulasToImage.IncrementX = incrementoX; return this; }
            public PFormulasToImageBuilder SetIncrementoY(float incrementY) { _pFormulasToImage.IncrementY = incrementY; return this; }
            public PFormulasToImageBuilder SetEspacamentoDivisorY(float espacamentoDivisorY) { _pFormulasToImage.EspacamentoDivisorY = espacamentoDivisorY; return this; }
            public PFormulasToImageBuilder withPrintAllClosedOpen() { _pFormulasToImage.PrintAllClosedOpen = true; return this; }
        }
    }

}