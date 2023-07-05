using System.Drawing;
using System.Drawing.Drawing2D;
using classes.auxiliar;
using classes.formulas;
using classes.parser;
using classes.solverstage;

namespace classes.testes.imagens
{
    public class TestesImagens2
    {

        Parser parser = new();
        // Consolas 10
        const float hchar = 10.0f; // height de 1 char
        const float wchar = 7.55f; // width de 1 char

        public void teste1()
        {
            Formulas f = getFormulaT();
            f = getFormulaABCDEFG();
            //f = getFormulaAB();
            p(f.ToString()); p(); p("");


            Quadro q = new(f);

            Dictionary<int, List<Quadro>>? dicQ = dicQuadros(q, 0);
            analiseHeight(dicQ, 15.0f);
            foreach (KeyValuePair<int, List<Quadro>> par in dicQ)
            {
                string fStr = string.Join(",", par.Value);
                p(string.Format("level: {0} {1}", par.Key, fStr));
            }
            p(); p("");

            //posOrder(q); p(); p("");

            // ajustarXY(q);
            // posOrder(q); p(); p("");

            // drawImg(g =>
            // {
            //     // draw a formula
            //     //drawFormula(g, new Quadro(f), 0, 0);

            //     drawQuadros(g, q);


            // }, 500, 500);

            List<Quadro> lquadros = plainQuadros(q);
            tratarXQuadros(lquadros, 0.0f);

            lquadros.ForEach(q => p(q.ToString()));


            List<Par> lpares = new();
            drawImg(g =>
            {
                int count = lquadros.Count;
                for (int i = 0; i < count; i++)
                {
                    Quadro q = lquadros[i];
                    Quadro? qNext = i + 1 < count ? lquadros[i + 1] : null;
                    drawQuadros(g, q);

                    float incrementoX = q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.X;
                    float incrementY = q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.Y;

                   using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                    {
                        PointF pfX = new PointF((q.Width / 2.0f) - wchar + incrementoX, 0.0f + incrementY - (q.Height / 2.0f));
                        g.DrawString("x", fonte, Brushes.Black, pfX);
                    }

                    if (qNext == null) { continue; }

                    float incrementoX2 = qNext == null || qNext.XY == null || !qNext.XY.HasValue ? 0.0f : qNext.XY.Value.X;
                    float incrementY2 = qNext == null || qNext.XY == null || !qNext.XY.HasValue ? 0.0f : qNext.XY.Value.Y;

                    using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                    {
                        //g.DrawString("o", fonte, Brushes.Black, new PointF((q.Width / 2.0f) - wchar + incrementoX, q.Height + incrementY - (q.Height / 2.0f)));
                        g.DrawString("o", fonte, Brushes.Black, new PointF((qNext.Width / 2.0f) - wchar + incrementoX2, qNext.Height + incrementY2 - (qNext.Height / 2.0f)));

                        //lpares.Add(new(new PointF()));
                    }
                }
                //lquadros.ForEach(q => { drawQuadros(g, q); });


            }, 500, 500);
        }

        class Par
        {
            public PointF P1 { get; set; }
            public PointF P2 { get; set; }
            public Par(PointF P1, PointF P2) { this.P1 = P1; this.P2 = P2; }
        }

        private void drawQuadros(Graphics g, Quadro q)
        {
            if (g == null || q == null) { return; }
            drawFormula(g, q, 0, 0);
            if (q.Esquerda != null) { drawQuadros(g, q.Esquerda); }
            if (q.Direita != null) { drawQuadros(g, q.Direita); }
        }

        // PreOrdem (RED): F,B,A,D,C,E,G,I,H
        // InOrdem (ERD): A,B,C,D,E,F,G,H,I
        // PosOrdem (EDR): A,C,E,D,B,H,I,G,F
        private void posOrder(Quadro q)
        {
            if (q == null) { return; }
            if (q.Esquerda != null) { posOrder(q.Esquerda); }

            if (q.Direita != null) { posOrder(q.Direita); }

            p(q.ToString());
        }

        private void tratarXQuadros(List<Quadro> quadros, float incrementW = 0.0f)
        {
            if (quadros == null) { return; }

            quadros.ForEach(q =>
            {
                if (q == null) { return; }
                q.XY = new PointF(incrementW, q.XY.HasValue ? q.XY.Value.Y : 0.0f);
                incrementW += q.Width;
            });
        }

        private void analiseHeight(Dictionary<int, List<Quadro>> dic, float espacamentoDivisorY = 0.0f)
        {
            if (dic == null || dic.Count <= 0) { return; }
            float heightAux = 0.0f;

            //dic = dic.OrderBy(obj => obj.Key).ToDictionary(obj => obj.Key, obj => obj.Value);

            foreach (KeyValuePair<int, List<Quadro>> par in dic)
            {
                if (par.Value == null) { continue; }
                float maxHeight = par.Value.Max(q => q.Height);

                par.Value.ForEach(q =>
                {
                    q.Height = maxHeight;

                    if (par.Key == 0) { return; }
                    float x = q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.X;
                    float y = q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.Y;
                    y += heightAux;
                    q.XY = new PointF(x, y);
                });
                heightAux += maxHeight + espacamentoDivisorY;

            }
        }

        private Dictionary<int, List<Quadro>>? dicQuadros(Quadro q, int level = 0)
        {
            if (q == null) { return null; }
            Dictionary<int, List<Quadro>> rt = new Dictionary<int, List<Quadro>>();
            List<Quadro> laux = rt.ContainsKey(level) ? rt[level] : new();
            laux.Add(q);
            rt.Remove(level);
            rt.Add(level, laux);

            Dictionary<int, List<Quadro>>? dAux = null;
            if (q.Esquerda != null)
            {
                dAux = dicQuadros(q.Esquerda, level + 1);
                if (dAux != null)
                {
                    foreach (KeyValuePair<int, List<Quadro>> par in dAux)
                    {
                        laux = rt.ContainsKey(par.Key) ? rt[par.Key] : new();
                        par.Value.ForEach(laux.Add);
                        rt.Remove(par.Key);
                        rt.Add(par.Key, laux);
                    }
                }

            }
            if (q.Direita != null)
            {
                dAux = dicQuadros(q.Direita, level + 1);
                if (dAux != null)
                {
                    foreach (KeyValuePair<int, List<Quadro>> par in dAux)
                    {
                        laux = rt.ContainsKey(par.Key) ? rt[par.Key] : new();
                        par.Value.ForEach(laux.Add);
                        rt.Remove(par.Key);
                        rt.Add(par.Key, laux);
                    }
                }

            }
            return rt;
        }

        private List<Quadro>? plainQuadros(Quadro q)
        {
            if (q == null) { return null; }
            List<Quadro>? aux = null;
            List<Quadro> rt = new();
            if (q.Esquerda != null)
            {
                aux = plainQuadros(q.Esquerda);
                if (aux != null)
                {
                    rt.AddRange(aux);
                }
            }
            rt.Add(q.plainCopy());
            if (q.Direita != null)
            {
                aux = plainQuadros(q.Direita);
                if (aux != null)
                {
                    rt.AddRange(aux);
                }
            }
            return rt;
        }

        class Quadro
        {
            public PointF? XY { get; set; }
            public List<string> formulas { get; set; }

            public float Width { get; set; }
            public float Height { get; set; }

            public Quadro Esquerda { get; }
            public Quadro Direita { get; }

            private Quadro() { }

            public Quadro(Formulas f)
            {
                formulas = new();
                if (f.Negativas != null) { f.Negativas.ForEach(x => formulas.Add(x.ToString())); }
                if (f.Positivas != null) { f.Positivas.ForEach(x => formulas.Add(x.ToString())); }

                if (f.Esquerda != null) { this.Esquerda = new Quadro(f.Esquerda); }
                if (f.Direita != null) { this.Direita = new Quadro(f.Direita); }

                Width = formulas == null ? 0 : formulas.Max(x => x.Length) * wchar + wchar * 0.5f;
                Height = formulas == null ? 0 : formulas.Count * hchar + hchar * 0.5f;
            }

            public Quadro plainCopy()
            {
                return new()
                {
                    Width = this.Width,
                    Height = this.Height,
                    formulas = this.formulas,
                    XY = this.XY
                };
            }

            public override string ToString()
            {
                string fStr = string.Join(",", formulas);
                return string.Format("[X: {0:0.00} Y: {1:0.00}][W: {2:0.00} H: {3:0.00}] {4}", XY != null && XY.HasValue ? XY.Value.X : 0, XY != null && XY.HasValue ? XY.Value.Y : 0, Width, Height, fStr);
            }
        }


        private void drawFormula(Graphics g, Quadro q, float incrementoX = 0.0f, float incrementY = 0.0f)
        {
            if (g == null || q == null) { return; }

            List<string> formulas = q.formulas;

            incrementoX += q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.X;
            incrementY += q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.Y;

            float w = q.Width;
            float h = q.Height;

            using (Pen blackPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(blackPen, new PointF(0 + incrementoX, 0 + incrementY), new PointF(w + incrementoX, 0 + incrementY)); // linha de cima
                g.DrawLine(blackPen, new PointF(0 + incrementoX, 0 + incrementY), new PointF(0 + incrementoX, h + incrementY)); // lateral esquerda
                g.DrawLine(blackPen, new PointF(w + incrementoX, 0 + incrementY), new PointF(w + incrementoX, h + incrementY)); // lateral direita
                g.DrawLine(blackPen, new PointF(0 + incrementoX, h + incrementY), new PointF(w + incrementoX, h + incrementY)); // linha baixo
            }

            PointF pTexto = new PointF(incrementoX, incrementY);
            using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
            {
                formulas.ForEach(x =>
                {
                    if (x == null || string.IsNullOrEmpty(x)) { return; }
                    g.DrawString(x, fonte, Brushes.Black, pTexto);
                    pTexto.Y += hchar;
                });
            }

        }




        #region aux1
        private void drawImg(Action<Graphics> act, int widthImg = 1000, int heigthImg = 1000, string imgNameSaida = "bmp_formula.png")
        {
            if (act == null) { return; }
            using Bitmap bitmap = new(widthImg, heigthImg);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                act.Invoke(graphics);
            }

            bitmap.Save(string.Format(@"{0}\{1}", "imgformulas", imgNameSaida));//save the image file
        }

        private Formulas getFormulaT()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A->B"));
            f.addConjuntoFormula(parser.parserCF("F C->E"));
            f.addConjuntoFormula(parser.parserCF("C"));
            f.addConjuntoFormula(parser.parserCF("F A"));
            //f.addConjuntoFormula(parser.parserCF("T (A | D) -> (C & D)"));
            f.addConjuntoFormula(parser.parserCF("T (A | D)"));


            //f.Negativas.ForEach(x => p(x.ToString()));
            //f.Positivas.ForEach(x => p(x.ToString()));

            f.addEsquerda(parser.parserCF("E"));
            f.addEsquerda(parser.parserCF("F Y -> (A | B)"));
            //f.addEsquerda(parser.parserCF("T A->B"));


            //f.Esquerda.Negativas.ForEach(x => p(x.ToString()));
            //f.Esquerda.Positivas.ForEach(x => p(x.ToString()));

            f.addDireita(parser.parserCF("T H->G"));
            //f.addDireita(parser.parserCF("F (A|Z) & (C | D) -> J"));
            f.addDireita(parser.parserCF("F (A|Z)"));
            f.addDireita(parser.parserCF("T G|T&U"));

            //f.Direita.Negativas.ForEach(x => p(x.ToString()));
            //f.Direita.Positivas.ForEach(x => p(x.ToString()));

            // TESTES
            f.Esquerda.addDireita(parser.parserCF("G & (Y -> B)"));

            return f;
        }

        private Formulas getFormulaABCDEFG()
        {
            Formulas f = new();

            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            // f.addConjuntoFormula(parser.parserCF("J->G"));
            f.addEsquerda(parser.parserCF("B"));
            f.Esquerda.addConjuntoFormula(parser.parserCF("J->G"));

            f.addDireita(parser.parserCF("E"));

            f.Esquerda.addEsquerda(parser.parserCF("C"));
            f.Esquerda.addDireita(parser.parserCF("D"));

            f.Direita.addEsquerda(parser.parserCF("F"));
            f.Direita.addDireita(parser.parserCF("G"));
            //f.Direita.Direita.addEsquerda(parser.parserCF("GT"));

            return f;
        }

        private Formulas getFormulaAB()
        {
            Formulas f = new();
            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A"));
            f.addEsquerda(parser.parserCF("B"));
            return f;
        }

        #endregion

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion


    }

}