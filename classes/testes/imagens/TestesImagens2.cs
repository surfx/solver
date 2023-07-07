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
            Formulas formulas = getFormulaT();
            //formulas = getFormulaABCDEFG();
            //formulas = getFormulaAB();
            //formulas = getFormulaAC();
            p(formulas.ToString()); p(); p("");


            Quadro qTree = new(formulas);

            float incrementoX = 20.0f, incrementY = 20.0f;
            //incrementoX = 0; incrementY = 0;

            float widthAjuste = incrementoX * .75f;
            float heightAjuste = 0.0f;

            Dictionary<int, List<Quadro>>? dicQ = dicQuadrosLevel(qTree, 0);
            heightAjuste = analiseHeight(dicQ, 15.0f) - 15.0f;
            //foreach (KeyValuePair<int, List<Quadro>> par in dicQ) { string fStr = string.Join(",", par.Value); p(string.Format("level: {0} {1}", par.Key, fStr)); } p(); p("");

            //posOrder(q); p(); p("");

            List<Quadro> lquadros = plainQuadros(qTree);
            tratarXQuadros(lquadros, 0.0f);
            ajustarQuadradosXY(qTree, lquadros, incrementoX, incrementY); // ajusta o X e Y para a tree Quadros

            //lquadros.ForEach(q => p(string.Format("{0} / {1}", q.ToString(), q.Rnd))); p(); p("");

            WH wh = getImageWidthHeight(lquadros, incrementoX, incrementY);

            drawImg(g =>
            {
                //lquadros.ForEach(q => { drawQuadros(g, q, false); });
                drawQuadros(g, qTree, false);
                drawDivisorias(g, qTree);
                //drawQuadroArroundFormulas(g, qTree, lquadros, incrementoX, incrementY);

            }, wh.WidthInt, wh.HeightInt);

            //p(string.Format("widthImg: {0}, heightImg: {1}", widthImg, heightImg));

        }

        private float getSumWH(Quadro qTree, bool sumWidth)
        {
            float rt = 0.0f;
            if (qTree == null) { return rt; }
            rt += sumWidth ? qTree.Width : qTree.Height;
            if (qTree.Esquerda != null) { rt += getSumWH(qTree.Esquerda, sumWidth); }
            if (qTree.Direita != null) { rt += getSumWH(qTree.Direita, sumWidth); }
            return rt;
        }

        class WH
        {
            public float Width { get; set; }
            public float Height { get; set; }
            public int WidthInt { get { return Convert.ToInt32(Width); } }
            public int HeightInt { get { return Convert.ToInt32(Height); } }
            public WH(float width = 0.0f, float height = 0.0f) { this.Width = width; this.Height = height; }
        }

        private WH getImageWidthHeight(List<Quadro> lquadros, float incrementoX = 0.0f, float incrementY = 0.0f)
        {
            Quadro? lastQuadroLeft = lquadros[0];

            float x1 = incrementoX;
            //float y1 = qTree.XY != null && qTree.XY.HasValue ? qTree.XY.Value.Y : 0.0f;
            float x2 = ((lastQuadroLeft != null && lastQuadroLeft.XY.HasValue ? lastQuadroLeft?.XY.Value.X : 0.0f) ?? 0.0f) + (lquadros.Sum(q => q.Width)) + x1;
            float y2 = lquadros.Max(q => q.XY != null && q.XY.HasValue ? q.XY.Value.Y : 0.0f) + ((lastQuadroLeft == null ? 0.0f : lastQuadroLeft?.Height) ?? 0.0f) + incrementY;

            return new(x2 + incrementoX, y2 + incrementY);
        }


        private void drawQuadroArroundFormulas(Graphics g, Quadro? qTree, List<Quadro> lquadros, float incrementoX = 0.0f, float incrementY = 0.0f)
        {
            if (g == null || qTree == null || lquadros == null || lquadros.Count <= 0) { return; }
            Quadro? lastQuadroLeft = lquadros[0];

            float x1 = incrementoX;
            float y1 = qTree.XY != null && qTree.XY.HasValue ? qTree.XY.Value.Y : 0.0f;
            float x2 = ((lastQuadroLeft != null && lastQuadroLeft.XY.HasValue ? lastQuadroLeft?.XY.Value.X : 0.0f) ?? 0.0f) + (lquadros.Sum(q => q.Width)) + x1;
            float y2 = lquadros.Max(q => q.XY != null && q.XY.HasValue ? q.XY.Value.Y : 0.0f) + ((lastQuadroLeft == null ? 0.0f : lastQuadroLeft?.Height) ?? 0.0f) + incrementY;

            using (Pen blackPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(blackPen, new PointF(x1, y1), new PointF(x2, y1)); // linha de cima
                g.DrawLine(blackPen, new PointF(x1, y1), new PointF(x1, y2)); // lateral esquerda
                g.DrawLine(blackPen, new PointF(x2, y1), new PointF(x2, y2)); // lateral direita
                g.DrawLine(blackPen, new PointF(x1, y2), new PointF(x2, y2)); // linha baixo
            }
        }

        private void drawDivisorias(Graphics g, Quadro q)
        {
            if (g == null || q == null) { return; }

            Pen blackPen = new Pen(Color.Black, 1.5f);

            if (q.Esquerda != null && q.Direita != null)
            {
                PointF pf1 = new PointF(q.Esquerda.XY.Value.X, q.Esquerda.XY.Value.Y - hchar * 0.5f);
                PointF pf2 = new PointF(q.Direita.XY.Value.X + q.Direita.Width, q.Direita.XY.Value.Y - hchar * 0.5f);
                g.DrawLine(blackPen, pf1, pf2);

                // p(string.Format("q: {0}, q.Esquerda: {1}, q.Direita: {2}", q, q.Esquerda, q.Direita));
            }
            else
            {
                if (q.Esquerda != null)
                {
                    PointF pf1 = new PointF(q.Esquerda.XY.Value.X, q.Esquerda.XY.Value.Y - hchar * 0.5f);
                    PointF pf2 = new PointF(q.XY.Value.X + q.Width, q.Esquerda.XY.Value.Y - hchar * 0.5f);
                    g.DrawLine(blackPen, pf1, pf2);

                    //p(string.Format("{0}, {1}, q.Direita: {2}, q: {3}", pf1, pf2, q.Direita, q));
                }
                else if (q.Direita != null)
                {
                    PointF pf1 = new PointF(q.XY.Value.X, q.Direita.XY.Value.Y - hchar * 0.5f);
                    PointF pf2 = new PointF(q.Direita.XY.Value.X + q.Direita.Width, q.Direita.XY.Value.Y - hchar * 0.5f);
                    g.DrawLine(blackPen, pf1, pf2);

                    //p(string.Format("{0}, {1}, q.Direita: {2}, q: {3}", pf1, pf2, q.Direita, q));
                }
            }


            if (q.Esquerda != null) { drawDivisorias(g, q.Esquerda); }
            if (q.Direita != null) { drawDivisorias(g, q.Direita); }
        }

        // atualiza o XY para a qTree
        private void ajustarQuadradosXY(Quadro qTree, List<Quadro> listaPlana, float incrementoX = 0.0f, float incrementY = 0.0f)
        {
            if (qTree == null || listaPlana == null || listaPlana.Count <= 0) { return; }
            //listaPlana.ForEach(q => { p(string.Format("q: {0}, qTree.Rnd: {1}, Equals: {2}, == {3}, {4}, {5}", q.Rnd, qTree.Rnd, q.Rnd.Equals(qTree.Rnd), q.Rnd == qTree.Rnd, q, qTree)); });

            Quadro? qaux = listaPlana.Where(q => q.Rnd.Equals(qTree.Rnd)).FirstOrDefault();
            if (qaux != null && qaux.XY.HasValue)
            {
                qTree.XY = new PointF(qaux.XY.Value.X + incrementoX, qaux.XY.Value.Y + incrementY);
                qTree.Height = qaux.Height;
                qTree.Width = qaux.Width;
                // if (incrementoX >= 0.0)
                // {
                //     qTree.Width += incrementoX;
                //     qaux.Width += incrementoX;
                // }
                // if (incrementY >= 0.0)
                // {
                //     qTree.Height += incrementY;
                //     qaux.Height += incrementoX;
                // }
            }
            if (qTree.Esquerda != null)
            {
                ajustarQuadradosXY(qTree.Esquerda, listaPlana, incrementoX, incrementY);
            }
            if (qTree.Direita != null)
            {
                ajustarQuadradosXY(qTree.Direita, listaPlana, incrementoX, incrementY);
            }
        }

        private void drawQuadros(Graphics g, Quadro q, bool drawSquare = false)
        {
            if (g == null || q == null) { return; }
            drawFormula(g, q, 0.0f, 0.0f, drawSquare);
            if (q.Esquerda != null) { drawQuadros(g, q.Esquerda, drawSquare); }
            if (q.Direita != null) { drawQuadros(g, q.Direita, drawSquare); }
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

        private float analiseHeight(Dictionary<int, List<Quadro>> dic, float espacamentoDivisorY = 0.0f)
        {
            float rt = 0.0f;
            if (dic == null || dic.Count <= 0) { return rt; }
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
                rt += espacamentoDivisorY;
            }
            return rt;
        }

        private Dictionary<int, List<Quadro>>? dicQuadrosLevel(Quadro q, int level = 0)
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
                dAux = dicQuadrosLevel(q.Esquerda, level + 1);
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
                dAux = dicQuadrosLevel(q.Direita, level + 1);
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

            private int _rnd = 0;
            public int Rnd { get { return _rnd; } }

            private Quadro()
            {
                setRandom();
            }

            public Quadro(Formulas f)
            {
                setRandom();
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
                    XY = this.XY,
                    _rnd = this._rnd
                };
            }

            // public override int GetHashCode() { return HashCode.Combine(Width, Height, Direita, Esquerda, XY); }

            private void setRandom()
            {
                if (_rnd > 0) { return; }
                _rnd = new Random().Next();
            }

            public override string ToString()
            {
                string fStr = string.Join(",", formulas);
                return string.Format("[X: {0:0.00} Y: {1:0.00}][W: {2:0.00} H: {3:0.00}] {4}", XY != null && XY.HasValue ? XY.Value.X : 0, XY != null && XY.HasValue ? XY.Value.Y : 0, Width, Height, fStr);
            }
        }


        private void drawFormula(Graphics g, Quadro q, float incrementoX = 0.0f, float incrementY = 0.0f, bool drawSquare = false)
        {
            if (g == null || q == null) { return; }

            List<string> formulas = q.formulas;

            incrementoX += q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.X;
            incrementY += q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.Y;

            float w = q.Width;
            float h = q.Height;

            if (drawSquare)
            {
                using (Pen blackPen = new Pen(Color.Black, 1))
                {
                    g.DrawLine(blackPen, new PointF(0 + incrementoX, 0 + incrementY), new PointF(w + incrementoX, 0 + incrementY)); // linha de cima
                    g.DrawLine(blackPen, new PointF(0 + incrementoX, 0 + incrementY), new PointF(0 + incrementoX, h + incrementY)); // lateral esquerda
                    g.DrawLine(blackPen, new PointF(w + incrementoX, 0 + incrementY), new PointF(w + incrementoX, h + incrementY)); // lateral direita
                    g.DrawLine(blackPen, new PointF(0 + incrementoX, h + incrementY), new PointF(w + incrementoX, h + incrementY)); // linha baixo
                }
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
            f.Direita.addEsquerda(parser.parserCF("T G|T&U"));

            //f.Direita.Negativas.ForEach(x => p(x.ToString()));
            //f.Direita.Positivas.ForEach(x => p(x.ToString()));

            // TESTES
            f.Esquerda.addDireita(parser.parserCF("G & (Y -> B)"));
            f.Esquerda.addEsquerda(parser.parserCF("G & (Y -> B)"));

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

        private Formulas getFormulaAC()
        {
            Formulas f = new();
            Parser parser = new();
            f.addConjuntoFormula(parser.parserCF("A"));
            f.addDireita(parser.parserCF("C"));
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