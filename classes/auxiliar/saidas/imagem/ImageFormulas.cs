using System.Drawing;
using System.Drawing.Drawing2D;

namespace classes.auxiliar.saidas.print
{
    public class ImageFormulas
    {
        private readonly PFormulasToImage.PFormulasToImageBuilder _pformulas2ImgB;

        private const string DotSimbol = "○";

        public ImageFormulas(PFormulasToImage.PFormulasToImageBuilder pformulas2ImgB)
        {
            this._pformulas2ImgB = pformulas2ImgB;
        }

        public void formulasToImage()
        {
            if (_pformulas2ImgB == null) { return; }
            PFormulasToImage pformulas2Img = _pformulas2ImgB.Build();
            if (pformulas2Img == null || pformulas2Img.Formulas == null) { return; }

            if (pformulas2Img.PrintFormulaNumber)
            {
                // atualiza o número das fórmulas antes de imprimir na imagem
                // deve ser antes de chamar o construtor de Quadro
                pformulas2Img.Formulas.updateNumeroFormulas();
            }
            Quadro qTree = new(_pformulas2ImgB);
            //qTree.initClass(_pformulas2ImgB);

            // float incrementoX = 20.0f, incrementY = 20.0f;
            // float espacamentoDivisorY = 25.0f;
            //incrementoX = 0; incrementY = 0;

            Dictionary<int, List<Quadro>>? dicQ = dicQuadrosLevel(qTree, 0);
            if (dicQ != null) { analiseHeight(dicQ, pformulas2Img.EspacamentoDivisorY); }
            //foreach (KeyValuePair<int, List<Quadro>> par in dicQ) { string fStr = string.Join(",", par.Value); p(string.Format("level: {0} {1}", par.Key, fStr)); } p(); p("");

            //posOrder(q); p(); p("");

            List<Quadro>? lquadros = plainQuadros(qTree);
            if (lquadros != null)
            {
                tratarXQuadros(lquadros, 0.0f);
                ajustarQuadradosXY(qTree, lquadros, pformulas2Img.IncrementX, pformulas2Img.IncrementY); // ajusta o X e Y para a tree Quadros

                //lquadros.ForEach(q => p(string.Format("{0} / {1}", q.ToString(), q.Rnd))); p(); p("");
            }

            Size? wh = getImageWidthHeight(lquadros, pformulas2Img.IncrementX, pformulas2Img.IncrementY);
            if (wh == null) { return; }
            drawImg(g =>
            {
                //lquadros.ForEach(q => { drawQuadros(g, q, false); });
                drawQuadros(g, qTree, pformulas2Img.DrawSquare);
                if (pformulas2Img.DivisoriaArvore) { drawDivisoriasArvore(g, qTree); } else { drawDivisorias(g, qTree); }
                if (pformulas2Img.DrawSquare) { drawQuadroArroundFormulas(g, qTree, lquadros, pformulas2Img.IncrementX, pformulas2Img.IncrementY); }

            }, wh.Value.Width, wh.Value.Height, pformulas2Img.PathImgSaida);
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

        private Size? getImageWidthHeight(List<Quadro>? lquadros, float incrementoX = 0.0f, float incrementY = 0.0f)
        {
            if (lquadros == null) { return null; }
            Quadro? lastQuadroLeft = lquadros[0];

            float x1 = incrementoX;
            //float y1 = qTree.XY != null && qTree.XY.HasValue ? qTree.XY.Value.Y : 0.0f;
            float x2 = ((lastQuadroLeft != null && lastQuadroLeft.XY.HasValue ? lastQuadroLeft?.XY.Value.X : 0.0f) ?? 0.0f) + (lquadros.Sum(q => q.Width)) + x1;
            float y2 = lquadros.Max(q => q.XY != null && q.XY.HasValue ? q.XY.Value.Y : 0.0f) + ((lastQuadroLeft == null ? 0.0f : lastQuadroLeft?.Height) ?? 0.0f) + incrementY;

            return new(Convert.ToInt32(x2 + incrementoX), Convert.ToInt32(y2 + incrementY));
        }


        private void drawQuadroArroundFormulas(Graphics g, Quadro? qTree, List<Quadro>? lquadros, float incrementoX = 0.0f, float incrementY = 0.0f)
        {
            if (g == null || qTree == null || lquadros == null || lquadros.Count <= 0) { return; }
            Quadro? lastQuadroLeft = lquadros[0];

            float x1 = incrementoX;
            float y1 = qTree.XY != null && qTree.XY.HasValue ? qTree.XY.Value.Y : 0.0f;
            float x2 = ((lastQuadroLeft != null && lastQuadroLeft.XY.HasValue ? lastQuadroLeft?.XY.Value.X : 0.0f) ?? 0.0f) + (lquadros.Sum(q => q.Width)) + x1;
            float y2 = lquadros.Max(q => q.XY != null && q.XY.HasValue ? q.XY.Value.Y : 0.0f) + ((lastQuadroLeft == null ? 0.0f : lastQuadroLeft?.Height) ?? 0.0f) + incrementY;

            using (Pen blackPen = new(Color.Black, 1))
            {
                g.DrawLine(blackPen, new PointF(x1, y1), new PointF(x2, y1)); // linha de cima
                g.DrawLine(blackPen, new PointF(x1, y1), new PointF(x1, y2)); // lateral esquerda
                g.DrawLine(blackPen, new PointF(x2, y1), new PointF(x2, y2)); // lateral direita
                g.DrawLine(blackPen, new PointF(x1, y2), new PointF(x2, y2)); // linha baixo
            }
        }

        #region divisórias

        // divisórias como linhas horizontais
        private void drawDivisorias(Graphics g, Quadro q)
        {
            if (g == null || q == null) { return; }
            PFormulasToImage pformulas2Img = _pformulas2ImgB.Build();

            using (Pen blackPen = new(Color.Black, 1.5f))
            {
                if (q.Esquerda != null && q.Direita != null)
                {
                    PointF pf1 = new(q.Esquerda.XY.Value.X, q.Esquerda.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                    PointF pf2 = new(q.Direita.XY.Value.X + q.Direita.Width, q.Direita.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                    g.DrawLine(blackPen, pf1, pf2);

                    // p(string.Format("q: {0}, q.Esquerda: {1}, q.Direita: {2}", q, q.Esquerda, q.Direita));
                }
                else
                {
                    if (q.Esquerda != null)
                    {
                        PointF pf1 = new(q.Esquerda.XY.Value.X, q.Esquerda.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                        PointF pf2 = new(q.XY.Value.X + q.Width, q.Esquerda.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                        g.DrawLine(blackPen, pf1, pf2);

                        //p(string.Format("{0}, {1}, q.Direita: {2}, q: {3}", pf1, pf2, q.Direita, q));
                    }
                    else if (q.Direita != null)
                    {
                        PointF pf1 = new(q.XY.Value.X, q.Direita.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                        PointF pf2 = new(q.Direita.XY.Value.X + q.Direita.Width, q.Direita.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                        g.DrawLine(blackPen, pf1, pf2);

                        //p(string.Format("{0}, {1}, q.Direita: {2}, q: {3}", pf1, pf2, q.Direita, q));
                    }
                }
            }

            if (q.Esquerda != null) { drawDivisorias(g, q.Esquerda); }
            if (q.Direita != null) { drawDivisorias(g, q.Direita); }
        }

        // divisória como ramos de uma árvore
        private void drawDivisoriasArvore(Graphics g, Quadro q)
        {
            if (g == null || q == null) { return; }
            if (q.Esquerda == null && q.Direita == null) { return; }
            PFormulasToImage pformulas2Img = _pformulas2ImgB.Build();

            using (Pen blackPen = new(Color.Black, 1.0f))
            {
                if (q.Esquerda != null)
                {
                    PointF pf1 = new(q.Esquerda.XY.Value.X + (q.Esquerda.Width / 2.0f), q.Esquerda.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                    PointF pf2 = new(q.XY.Value.X + (q.Width / 2.0f), q.XY.Value.Y + q.Height + pformulas2Img.HChar * 0.23f);
                    g.DrawLine(blackPen, pf1, pf2);

                    drawDivisoriasArvore(g, q.Esquerda);
                }

                if (q.Direita != null)
                {
                    PointF pf1 = new(q.Direita.XY.Value.X + (q.Direita.Width / 2.0f), q.Direita.XY.Value.Y - pformulas2Img.HChar * 0.5f);
                    PointF pf2 = new(q.XY.Value.X + (q.Width / 2.0f), q.XY.Value.Y + q.Height + pformulas2Img.HChar * 0.23f);
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

        private void drawQuadros(Graphics g, Quadro q, bool drawSquare = false, bool isDireita = false)
        {
            if (g == null || q == null) { return; }
            drawFormula(g, q, isDireita, 0.0f, 0.0f, drawSquare);
            if (q.Esquerda != null) { drawQuadros(g, q.Esquerda, drawSquare, false); }
            if (q.Direita != null) { drawQuadros(g, q.Direita, drawSquare, true); }
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
            Dictionary<int, List<Quadro>> rt = new();
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
            List<Quadro>? aux;
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
            public List<string>? formulas { get; set; }

            public float Width { get; set; }
            public float Height { get; set; }

            public Quadro Esquerda { get; private set; }
            public Quadro Direita { get; private set; }

            private int _rnd = 0;
            public int Rnd { get { return _rnd; } }

            private PFormulasToImage.PFormulasToImageBuilder _pformulas2ImgBuilder;
            private PFormulasToImage _pformulas2Img;

            public Quadro(PFormulasToImage.PFormulasToImageBuilder pformulas2ImgBuilder)
            {
                setRandom();
                initClass(pformulas2ImgBuilder);
            }

            // separado do construtor por causa da recursão e número da fórmula
            private void initClass(PFormulasToImage.PFormulasToImageBuilder pformulas2ImgBuilder)
            {
                setRandom();
                formulas = new();
                this._pformulas2ImgBuilder = pformulas2ImgBuilder;

                _pformulas2Img = _pformulas2ImgBuilder.Build();
                if (_pformulas2Img.Formulas.LConjuntoFormula != null)
                {
                    _pformulas2Img.Formulas.LConjuntoFormula.ForEach(cf =>
                    {
                        formulas.Add(
                            _pformulas2Img.PrintDotTreeMode ?
                            DotSimbol :
                            (_pformulas2Img.PrintFormulaNumber && cf.NumeroFormula >= 0 ? string.Format("{0} {1}", cf.NumeroFormula, cf.ToString()) : cf.ToString())
                        );
                    });
                }

                if (!_pformulas2Img.PrintDotTreeMode)
                {
                    if (_pformulas2Img.PrintAllClosedOpen)
                    {
                        formulas.Add(_pformulas2Img.Formulas.isClosed ? "CLOSED" : "OPEN");
                    }
                    else
                    {
                        if (_pformulas2Img.Formulas.Esquerda == null && _pformulas2Img.Formulas.Direita == null)
                        {
                            formulas.Add(_pformulas2Img.Formulas.isClosed ? "CLOSED" : "OPEN");
                        }
                    }
                }

                if (_pformulas2Img.Formulas.Esquerda != null)
                {
                    PFormulasToImage.PFormulasToImageBuilder pformulas2ImgAux = pformulas2ImgBuilder.copy();
                    pformulas2ImgAux.SetFormulas(_pformulas2Img.Formulas.Esquerda);
                    this.Esquerda = new Quadro(pformulas2ImgAux);
                }
                if (_pformulas2Img.Formulas.Direita != null)
                {
                    PFormulasToImage.PFormulasToImageBuilder pformulas2ImgAux = pformulas2ImgBuilder.copy();
                    pformulas2ImgAux.SetFormulas(_pformulas2Img.Formulas.Direita);
                    this.Direita = new Quadro(pformulas2ImgAux);
                }

                if (_pformulas2Img.PrintDotTreeMode)
                {
                    Width = formulas == null ? 0 : /*DotSimbol.Length * */ _pformulas2Img.Wchar;
                }
                else
                {
                    Width = formulas == null ? 0 : formulas.Max(x => x.Length) * _pformulas2Img.Wchar + _pformulas2Img.Wchar * 0.5f;
                }

                Height = formulas == null ? 0 : formulas.Count * _pformulas2Img.HChar + _pformulas2Img.HChar * 0.5f;
            }

            public Quadro plainCopy()
            {
                return new(this._pformulas2ImgBuilder)
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
                string fStr = formulas == null ? "" : string.Join(",", formulas);
                return string.Format("[X: {0:0.00} Y: {1:0.00}][W: {2:0.00} H: {3:0.00}] {4}", XY != null && XY.HasValue ? XY.Value.X : 0, XY != null && XY.HasValue ? XY.Value.Y : 0, Width, Height, fStr);
            }
        }


        private void drawFormula(Graphics g, Quadro q, bool isDireita, float incrementoX = 0.0f, float incrementY = 0.0f, bool drawSquare = false)
        {
            if (g == null || q == null) { return; }
            PFormulasToImage pformulas2Img = _pformulas2ImgB.Build();

            List<string>? formulas = q.formulas;

            incrementoX += q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.X;
            incrementY += q.XY == null || !q.XY.HasValue ? 0.0f : q.XY.Value.Y;
            if (isDireita && pformulas2Img.PrintDotTreeMode)
            {
                incrementoX += pformulas2Img.Wchar / 1.5f;
            }

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

            Font fonte = pformulas2Img.Fonte ?? new("Consolas", 10, FontStyle.Regular);
            formulas?.ForEach(formula =>
            {
                if (formula == null || string.IsNullOrEmpty(formula)) { return; }
                Brush brush = Brushes.Black;
                PointF pTexto = pTextoDefault;

                if (formula.Equals("CLOSED") || formula.Equals("OPEN"))
                {
                    brush = formula.Equals("CLOSED") ? Brushes.Red : Brushes.Green;
                    pTexto = new PointF(pTextoDefault.X + (q.Width / 2.0f) - (formula.Length * pformulas2Img.Wchar) / 2.0f, pTextoDefault.Y);
                }

                g.DrawString(formula, fonte, brush, pTexto);
                pTextoDefault.Y += pformulas2Img.HChar;
            });
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
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
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