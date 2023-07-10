using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using classes.solverstage;

namespace classes.auxiliar.saidas.print
{
    public class ImageFormulas
    {
        // Consolas 10
        private const float hchar = 15.0f; // height de 1 char
        private const float wchar = 7.55f; // width de 1 char

        public void formulasToImage(PFormulasToImage pformulas2Img)
        {
            if (pformulas2Img == null || pformulas2Img.Formulas == null) { return; }

            Quadro qTree = new(pformulas2Img.Formulas);

            // float incrementoX = 20.0f, incrementY = 20.0f;
            // float espacamentoDivisorY = 25.0f;
            //incrementoX = 0; incrementY = 0;

            Dictionary<int, List<Quadro>>? dicQ = dicQuadrosLevel(qTree, 0);
            analiseHeight(dicQ, pformulas2Img.EspacamentoDivisorY);
            //foreach (KeyValuePair<int, List<Quadro>> par in dicQ) { string fStr = string.Join(",", par.Value); p(string.Format("level: {0} {1}", par.Key, fStr)); } p(); p("");

            //posOrder(q); p(); p("");

            List<Quadro> lquadros = plainQuadros(qTree);
            tratarXQuadros(lquadros, 0.0f);
            ajustarQuadradosXY(qTree, lquadros, pformulas2Img.IncrementX, pformulas2Img.IncrementY); // ajusta o X e Y para a tree Quadros

            //lquadros.ForEach(q => p(string.Format("{0} / {1}", q.ToString(), q.Rnd))); p(); p("");

            Size wh = getImageWidthHeight(lquadros, pformulas2Img.IncrementX, pformulas2Img.IncrementY);

            drawImg(g =>
            {
                //lquadros.ForEach(q => { drawQuadros(g, q, false); });
                drawQuadros(g, qTree, pformulas2Img.DrawSquare);
                if (pformulas2Img.DivisoriaArvore) { drawDivisoriasArvore(g, qTree); } else { drawDivisorias(g, qTree); }
                if (pformulas2Img.DrawSquare) { drawQuadroArroundFormulas(g, qTree, lquadros, pformulas2Img.IncrementX, pformulas2Img.IncrementY); }

            }, wh.Width, wh.Height, pformulas2Img.PathImgSaida);
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

        private Size getImageWidthHeight(List<Quadro> lquadros, float incrementoX = 0.0f, float incrementY = 0.0f)
        {
            Quadro? lastQuadroLeft = lquadros[0];

            float x1 = incrementoX;
            //float y1 = qTree.XY != null && qTree.XY.HasValue ? qTree.XY.Value.Y : 0.0f;
            float x2 = ((lastQuadroLeft != null && lastQuadroLeft.XY.HasValue ? lastQuadroLeft?.XY.Value.X : 0.0f) ?? 0.0f) + (lquadros.Sum(q => q.Width)) + x1;
            float y2 = lquadros.Max(q => q.XY != null && q.XY.HasValue ? q.XY.Value.Y : 0.0f) + ((lastQuadroLeft == null ? 0.0f : lastQuadroLeft?.Height) ?? 0.0f) + incrementY;

            return new(Convert.ToInt32(x2 + incrementoX), Convert.ToInt32(y2 + incrementY));
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

        #region divisórias
        // divisórias como linhas
        private void drawDivisorias(Graphics g, Quadro q)
        {
            if (g == null || q == null) { return; }

            using (Pen blackPen = new Pen(Color.Black, 1.5f))
            {
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
            }

            if (q.Esquerda != null) { drawDivisorias(g, q.Esquerda); }
            if (q.Direita != null) { drawDivisorias(g, q.Direita); }
        }

        private void drawDivisoriasArvore(Graphics g, Quadro q)
        {
            if (g == null || q == null) { return; }
            if (q.Esquerda == null && q.Direita == null) { return; }

            using (Pen blackPen = new Pen(Color.Black, 1.0f))
            {
                if (q.Esquerda != null)
                {
                    PointF pf1 = new PointF(q.Esquerda.XY.Value.X + (q.Esquerda.Width / 2.0f), q.Esquerda.XY.Value.Y - hchar * 0.5f);
                    PointF pf2 = new PointF(q.XY.Value.X + (q.Width / 2.0f), q.XY.Value.Y + q.Height + hchar * 0.23f);
                    g.DrawLine(blackPen, pf1, pf2);

                    drawDivisoriasArvore(g, q.Esquerda);
                }

                if (q.Direita != null)
                {
                    PointF pf1 = new PointF(q.Direita.XY.Value.X + (q.Direita.Width / 2.0f), q.Direita.XY.Value.Y - hchar * 0.5f);
                    PointF pf2 = new PointF(q.XY.Value.X + (q.Width / 2.0f), q.XY.Value.Y + q.Height + hchar * 0.23f);
                    g.DrawLine(blackPen, pf1, pf2);

                    drawDivisoriasArvore(g, q.Direita);
                }
            }
        }

        #endregion

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

        // // PreOrdem (RED): F,B,A,D,C,E,G,I,H
        // // InOrdem (ERD): A,B,C,D,E,F,G,H,I
        // // PosOrdem (EDR): A,C,E,D,B,H,I,G,F
        // private void posOrder(Quadro q)
        // {
        //     if (q == null) { return; }
        //     if (q.Esquerda != null) { posOrder(q.Esquerda); }

        //     if (q.Direita != null) { posOrder(q.Direita); }

        //     p(q.ToString());
        // }

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

        private class Quadro
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

                if (f.Esquerda == null && f.Direita == null)
                {
                    formulas.Add(f.isClosed ? "CLOSED" : "OPEN");
                }

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
                using (Pen blackPen = new(Color.Black, 1))
                {
                    g.DrawLine(blackPen, new PointF(0 + incrementoX, 0 + incrementY), new PointF(w + incrementoX, 0 + incrementY)); // linha de cima
                    g.DrawLine(blackPen, new PointF(0 + incrementoX, 0 + incrementY), new PointF(0 + incrementoX, h + incrementY)); // lateral esquerda
                    g.DrawLine(blackPen, new PointF(w + incrementoX, 0 + incrementY), new PointF(w + incrementoX, h + incrementY)); // lateral direita
                    g.DrawLine(blackPen, new PointF(0 + incrementoX, h + incrementY), new PointF(w + incrementoX, h + incrementY)); // linha baixo
                }
            }

            PointF pTextoDefault = new(incrementoX, incrementY);
            using (Font fonte = new("Consolas", 10, FontStyle.Regular))
            {
                formulas.ForEach(x =>
                {
                    if (x == null || string.IsNullOrEmpty(x)) { return; }
                    Brush brush = Brushes.Black;
                    PointF pTexto = pTextoDefault;

                    if (x.Equals("CLOSED") || x.Equals("OPEN"))
                    {
                        brush = x.Equals("CLOSED") ? Brushes.Red : Brushes.Green;
                        pTexto = new PointF(pTextoDefault.X + (q.Width / 2.0f) - (x.Length * wchar) / 2.0f, pTextoDefault.Y);
                    }

                    g.DrawString(x, fonte, brush, pTexto);
                    pTextoDefault.Y += hchar;
                });
            }

        }


        #region imagem
        private void drawImg(Action<Graphics> act, int widthImg = 1000, int heigthImg = 1000, string pathImgSaida = "bmp_formula.png")
        {
            if (act == null) { return; }
            Bitmap? bitmap = drawToImg(act, widthImg, heigthImg);
            if (bitmap == null) { return; }
            bitmap.Save(pathImgSaida);//save the image file
            bitmap.Dispose();
        }

        private Bitmap? drawToImg(Action<Graphics> act, int widthImg = 1000, int heigthImg = 1000)
        {
            if (act == null) { return null; }
            widthImg = widthImg <= 0 ? 1000 : widthImg;
            heigthImg = heigthImg <= 0 ? 1000 : heigthImg;
            Bitmap bitmap = new(widthImg, heigthImg);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                act.Invoke(graphics);
            }
            return bitmap;
        }
        #endregion


    }
}