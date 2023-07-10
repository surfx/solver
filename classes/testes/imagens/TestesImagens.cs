using System.Drawing;
using System.Drawing.Drawing2D;
using classes.auxiliar;
using classes.formulas;
using classes.parser;
using classes.solverstage;

namespace classes.testes.imagens
{
    public class TestesImagens
    {

        Parser parser = new();

        public void teste1()
        {
            string firstText = "Hello";
            string secondText = "World";

            PointF firstLocation = new PointF(10f, 10f);
            PointF secondLocation = new PointF(10f, 50f);


            //Bitmap bitmap = (Bitmap)Image.FromFile(imageFilePath);//load the image file
            //using Bitmap bitmap = drawFilledRectangle(500, 600);
            using Bitmap bitmap = new(500, 600);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (Font arialFont = new Font("Arial", 10, FontStyle.Regular))
                {
                    graphics.DrawString(firstText, arialFont, Brushes.Blue, firstLocation);
                    graphics.DrawString(secondText, arialFont, Brushes.Red, secondLocation);
                }

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                graphics.DrawString("My\nText", new System.Drawing.Font("Consola", 15, FontStyle.Regular), Brushes.Black, new PointF(100f, 100f), sf);


                for (int i = 0; i <= 500; i += 15)
                {
                    for (int j = 0; j <= 600; j += 21)
                    {
                        graphics.DrawString("0", new System.Drawing.Font("Consola", 15, FontStyle.Regular), i == 0 || i % 2 == 0 ? Brushes.Blue : Brushes.Green, new PointF(i, j), sf);
                    }
                }

            }

            bitmap.Save(@"imgformulas\bmp.png");//save the image file
        }

        public void teste2()
        {
            drawImg(g =>
            {
                Pen blackPen = new Pen(Color.Black, 1);
                PointF point1 = new PointF(100.0F, 100.0F);
                PointF point2 = new PointF(500.0F, 100.0F);
                g.DrawLine(blackPen, point1, point2);

                using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                {
                    g.DrawString("0,0", fonte, Brushes.Black, new PointF(0, 0));
                    g.DrawString("0,10", fonte, Brushes.Black, new PointF(0, 10));
                    g.DrawString("0,100", fonte, Brushes.Black, new PointF(0, 100));
                    g.DrawString("0,500", fonte, Brushes.Black, new PointF(0, 500));
                    g.DrawString("0,900", fonte, Brushes.Black, new PointF(0, 900));
                    g.DrawString("0,990", fonte, Brushes.Black, new PointF(0, 990));

                    g.DrawString("500,500", fonte, Brushes.Black, new PointF(500, 500));

                    g.DrawString("990,990", fonte, Brushes.Black, new PointF(990, 990));
                    g.DrawString("1000,1000", fonte, Brushes.Black, new PointF(1000, 1000));

                    g.DrawString("990,0", fonte, Brushes.Black, new PointF(990, 0));
                    g.DrawString("1000,0", fonte, Brushes.Black, new PointF(1000, 0));

                    // estudo tamanho caracteres
                    g.DrawString("0123456789", fonte, Brushes.Black, new PointF(300, 300));
                    g.DrawString("abcdefghijklmnopqrstuvxyz".ToUpper(), fonte, Brushes.Black, new PointF(300, 310));
                    g.DrawString("abcdefghijklmnopqrstuvxyz", fonte, Brushes.Black, new PointF(300, 320));

                    // int h = 320;
                    // for(float f = 0.0f; f <= 10.0f; f++){
                    //     h+=10;
                    //     g.DrawString(""+f, fonte, Brushes.Black, new PointF(300.0f + f, h));
                    // }
                    float w = 7.55f;
                    //g.DrawString("a", fonte, Brushes.Black, new PointF(300f, 330));
                    //g.DrawString("c", fonte, Brushes.Black, new PointF(300.0f + w*10, 330));
                    for (float f = 0.0f; f <= 400.0f; f += w)
                    {
                        g.DrawString("c", fonte, Brushes.Black, new PointF(300.0f + f, 330));
                    }

                }

            });
        }

        public void teste3()
        {
            Formulas f = getFormulaT();
            //f = getFormulaABCDEFG();
            //f.Direita.Esquerda.addDireita(parser.parserCF("H"));

            p(f.ToString()); p(); p("");

            // Consolas 10
            const float hchar = 10.0f; // height de 1 char
            const float wchar = 7.55f; // width de 1 char
            int lineMaxL = lineMaxLength(f, 2) + 2; // 2 espaços: direita/esquerda
            float widthImg = lineMaxL * wchar;
            int numL = numLines(f); // número de linhas
            float heigthImg = (numL + 1.5f) * hchar;
            p(string.Format("lineMaxL: {0}, widthImg: {1}", lineMaxL, widthImg));
            p(string.Format("numL: {0}, heigthImg: {1}", numL, heigthImg));

            //float widthMiddle = widthImg / 2.0f;

            drawImg(g =>
            {
                using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                {
                    drawFormula(f, g, fonte, widthImg, widthImg, 0, hchar, wchar);
                }
            }, Convert.ToInt32(widthImg), Convert.ToInt32(heigthImg));

        }

        public void teste4()
        {
            Formulas f = getFormulaT();
            //f = getFormulaABCDEFG();

            // Consolas 10
            const float hchar = 10.0f; // height de 1 char
            const float wchar = 7.55f; // width de 1 char

            string[] linhas = f.ToString().Split(Environment.NewLine);

            int maxChars = linhas.Max(l => l.Length) + 2; // 2: espaçamento
            float widthImg = maxChars * wchar;
            int numL = numLines(f); // número de linhas
            float heigthImg = (numL + 2.5f) * hchar;
            p(string.Format("maxChars: {0}, widthImg: {1}", maxChars, widthImg));
            p(string.Format("numL: {0}, heigthImg: {1}", numL, heigthImg));

            foreach (string linha in linhas)
            {
                if (linha == null || linha.Length <= 0) { continue; }
                p(string.Format("{0}, l: {1}", linha, linha.Length));
            }

            drawImg(g =>
            {
                using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                {
                    int height = 0;
                    foreach (string linha in linhas)
                    {
                        if (linha == null || linha.Length <= 0) { continue; }
                        g.DrawString(linha, fonte, Brushes.Black, new PointF(0, height++ * (hchar + .75f)));
                    }
                }
            }, Convert.ToInt32(widthImg), Convert.ToInt32(heigthImg));

        }

        public void teste5()
        {
            Formulas f = getFormulaT();
            f = getFormulaABCDEFG();
            p(f.ToString()); p(); p("");

            Quadro q = new Quadro(f);

            //List<Quadro>? lquadros = plainQuadros(q);

            // Consolas 10
            const float hchar = 10.0f; // height de 1 char
            const float wchar = 7.55f; // width de 1 char
            float incrementoW = 10.0f;
            float incrementoH = 10.0f;
            //List<Quadro> lquadros, float incrementoW, float incrementH, float hchar = 10.0f, float wchar = 7.55f

            q = tratarYQuadros(q, incrementoH, 0, hchar, wchar);
            List<Quadro>? lquadros = plainQuadros(q);
            //lquadros!.ForEach(x => p(x.ToString())); p(); p("");

            lquadros = plainQuadrosXY(lquadros, incrementoW, incrementoH, hchar, wchar);
            //lquadros!.ForEach(x => p(x.ToString()));

            float widthMax = lquadros.Sum(x => x.Width) + lquadros.Count * incrementoW;
            float heightMax = lquadros.Sum(x => x.Height) + incrementoH;

            drawImg(g =>
            {

                float widthIncrement = 0.0f;

                lquadros.ForEach(q =>
                {
                    //drawQuadro(g, q1, incrementoW + widthIncrement, q1.XY.Y, hchar, wchar);
                    drawQuadro(g, q, incrementoW + widthIncrement, hchar, wchar);
                    widthIncrement += q.Width;
                });

            }, Convert.ToInt32(widthMax), Convert.ToInt32(heightMax));
        }

        public void teste6()
        {
            Formulas f = getFormulaT();
            f = getFormulaABCDEFG();
            p(f.ToString()); p(); p("");

            // Consolas 10
            const float hchar = 10.0f; // height de 1 char
            const float wchar = 7.55f; // width de 1 char

            Quadro q = new Quadro(f);

            drawImg(g =>
            {

                int lineMaxL = lineMaxLength(q);
                int numL = numLines(q);

                p(string.Format("lineMaxL * wchar: {0}", lineMaxL * wchar));

                float incremento = 50.0f;
                //drawSquare(g, incremento, incremento, lineMaxL, numL, hchar, wchar);

                // descomentar
                //drawQuadro(g, q, incremento, incremento, hchar, wchar); // lineMaxL * wchar

                //drawQuadro(g, q.Esquerda, 50, 50.0f + (q.Height * 1.25f), hchar, wchar);
                //drawQuadro(g, q.Direita, 50.0f + (q.Esquerda.Width * 1.25f), 50.0f + (q.Height * 1.25f), hchar, wchar);
                //drawQuadro(g, q.Esquerda, incremento - (q.Width * 1.25f), incremento + (q.Height * 1.25f), lineMaxL * wchar, hchar, wchar);

                // float pIX = 50;
                // float pFX = 150;
                // drawLine(g, pIX, pIX + q.Width, true, true);
                // drawLine(g, pIX, pIX + q.Height, true, false);

                // //drawLine(g, pIX, pIX + q.Width, false, true);
                // drawLine(g, pIX + q.Height, pIX + q.Width, false, false);

                // using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                // {
                //     g.DrawString("0", fonte, Brushes.Black, new PointF(150, 150));
                //     g.DrawString("0", fonte, Brushes.Black, new PointF(150, 50));
                // }

                // linha vertical
                // Pen blackPen = new Pen(Color.Black, 1);
                // PointF point1 = new PointF(pinicial, pinicial);
                // PointF point2 = new PointF(pfinal, pinicial);
                // g.DrawLine(blackPen, point1, point2);

                // using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                // {
                //     g.DrawString("0", fonte, Brushes.Black, new PointF(50, 50));
                //     g.DrawString("0", fonte, Brushes.Black, new PointF(150, 50));
                // }
            }, 500, 500);

        }


        private void drawSquare(Graphics g, float incrementoW, float incrementH, float widthMax, float heightMax, float hchar = 10.0f, float wchar = 7.55f)
        {
            float w = widthMax * wchar;
            float h = heightMax * hchar;
            using (Pen blackPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(blackPen, new PointF(0 + incrementoW, 0 + incrementH), new PointF(w + incrementoW, 0 + incrementH)); // linha de cima
                g.DrawLine(blackPen, new PointF(0 + incrementoW, 0 + incrementH), new PointF(0 + incrementoW, h + incrementH)); // lateral esquerda
                g.DrawLine(blackPen, new PointF(w + incrementoW, 0 + incrementH), new PointF(w + incrementoW, h + incrementH)); // lateral direita
                g.DrawLine(blackPen, new PointF(0 + incrementoW, h + incrementH), new PointF(w + incrementoW, h + incrementH)); // linha baixo
            }
        }

        private void drawQuadroOLD(Graphics g, Quadro q, float incrementoW, float incrementH, float hchar = 10.0f, float wchar = 7.55f)
        {
            if (g == null || q == null) { return; }
            float w = q.Width;
            float h = q.Height;

            float middleText = (q.formulas.Max(x => x.Length) * wchar) / 2.0f;
            //float heightFormulas = q.formulas.Count * hchar;
            // incrementoW += widthMax / 2.0f - middleText;

            using (Pen blackPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(blackPen, new PointF(0 + incrementoW, 0 + incrementH), new PointF(w + incrementoW, 0 + incrementH)); // linha de cima
                g.DrawLine(blackPen, new PointF(0 + incrementoW, 0 + incrementH), new PointF(0 + incrementoW, h + incrementH)); // lateral esquerda
                g.DrawLine(blackPen, new PointF(w + incrementoW, 0 + incrementH), new PointF(w + incrementoW, h + incrementH)); // lateral direita
                g.DrawLine(blackPen, new PointF(0 + incrementoW, h + incrementH), new PointF(w + incrementoW, h + incrementH)); // linha baixo
            }

            using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
            {

                #region comments
                //drawSquare(g, incrementoW, incrementH, w, h, hchar = 10.0f, wchar = 7.55f);

                // // linha de cima
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, 0 + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, 0 + incrementH));

                // // lateral esquerda
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, 0 + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, h + incrementH));

                // // lateral direita
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, 0 + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, h + incrementH));

                // // linha baixo
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, h + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, h + incrementH));
                #endregion

                float hPlus = incrementH + hchar * .25f;

                // Texto
                q.formulas.ForEach(f =>
                {
                    if (f == null || string.IsNullOrEmpty(f)) { return; }

                    string texto = f;
                    int lTexto = texto.Length;

                    //PointF pm = q.MeanMiddle.Value;
                    PointF pm = q.XY;
                    //pm.X += incrementoW - ((lTexto * wchar) / 2.0f); // centraliza
                    //pm.X += incrementoW - middleText; //((lTexto * wchar) / 2.0f); - centraliza
                    pm.Y = hPlus;
                    g.DrawString(texto, fonte, Brushes.Black, pm);
                    //g.DrawString(texto, fonte, Brushes.Black, q.XY);

                    hPlus += hchar;
                });

            }

            #region comments
            // // Graphics g, Quadro q, float incrementoW, float incrementH, float widthMax, float hchar = 10.0f, float wchar = 7.55f
            // if (q.Esquerda != null)
            // {
            //     //drawQuadro(g, q.Esquerda, incrementoW - w/2.0f - widthMax * .5f, incrementH + h, widthMax, hchar, wchar);
            //     drawQuadro(g, q.Esquerda, incrementoW - widthMax / 2.0f - q.Esquerda.Width, incrementH + h, widthMax, hchar, wchar);
            // }
            // if (q.Direita != null)
            // {
            //     //drawQuadro(g, q.Direita, incrementoW + w/2.0f - widthMax * 0.25f, incrementH + h, widthMax, hchar, wchar);
            //     drawQuadro(g, q.Direita, incrementoW + q.Direita.Width, incrementH + h, widthMax, hchar, wchar);
            // }
            #endregion

        }

        private void drawQuadro(Graphics g, Quadro q, float incrementoW, float hchar = 10.0f, float wchar = 7.55f)
        {
            if (g == null || q == null) { return; }
            float w = q.Width;
            float h = q.Height;
            float incrementH = q.XY.Y;

            float middleText = (q.formulas.Max(x => x.Length) * wchar) / 2.0f;
            //float heightFormulas = q.formulas.Count * hchar;
            // incrementoW += widthMax / 2.0f - middleText;

            using (Pen blackPen = new Pen(Color.Black, 1))
            {
                g.DrawLine(blackPen, new PointF(0 + incrementoW, 0 + incrementH), new PointF(w + incrementoW, 0 + incrementH)); // linha de cima
                g.DrawLine(blackPen, new PointF(0 + incrementoW, 0 + incrementH), new PointF(0 + incrementoW, h + incrementH)); // lateral esquerda
                g.DrawLine(blackPen, new PointF(w + incrementoW, 0 + incrementH), new PointF(w + incrementoW, h + incrementH)); // lateral direita
                g.DrawLine(blackPen, new PointF(0 + incrementoW, h + incrementH), new PointF(w + incrementoW, h + incrementH)); // linha baixo
            }

            using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
            {

                #region comments
                //drawSquare(g, incrementoW, incrementH, w, h, hchar = 10.0f, wchar = 7.55f);

                // // linha de cima
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, 0 + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, 0 + incrementH));

                // // lateral esquerda
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, 0 + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, h + incrementH));

                // // lateral direita
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, 0 + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, h + incrementH));

                // // linha baixo
                // g.DrawString("0", fonte, Brushes.Black, new PointF(0 + incrementoW, h + incrementH));
                // g.DrawString("0", fonte, Brushes.Black, new PointF(w + incrementoW, h + incrementH));
                #endregion

                float hPlus = incrementH + hchar * .25f;

                // Texto
                q.formulas.ForEach(f =>
                {
                    if (f == null || string.IsNullOrEmpty(f)) { return; }

                    string texto = f;
                    int lTexto = texto.Length;

                    //PointF pm = q.MeanMiddle.Value;
                    PointF pm = q.XY;
                    //pm.X += incrementoW - ((lTexto * wchar) / 2.0f); // centraliza
                    //pm.X += incrementoW - middleText; //((lTexto * wchar) / 2.0f); - centraliza
                    pm.Y = hPlus;
                    g.DrawString(texto, fonte, Brushes.Black, pm);
                    //g.DrawString(texto, fonte, Brushes.Black, q.XY);

                    hPlus += hchar;
                });

            }

            #region comments
            // // Graphics g, Quadro q, float incrementoW, float incrementH, float widthMax, float hchar = 10.0f, float wchar = 7.55f
            // if (q.Esquerda != null)
            // {
            //     //drawQuadro(g, q.Esquerda, incrementoW - w/2.0f - widthMax * .5f, incrementH + h, widthMax, hchar, wchar);
            //     drawQuadro(g, q.Esquerda, incrementoW - widthMax / 2.0f - q.Esquerda.Width, incrementH + h, widthMax, hchar, wchar);
            // }
            // if (q.Direita != null)
            // {
            //     //drawQuadro(g, q.Direita, incrementoW + w/2.0f - widthMax * 0.25f, incrementH + h, widthMax, hchar, wchar);
            //     drawQuadro(g, q.Direita, incrementoW + q.Direita.Width, incrementH + h, widthMax, hchar, wchar);
            // }
            #endregion

        }


        class Quadro
        {
            public float Width { get; set; }
            public float Height { get; set; }
            public PointF? TopMiddle { get { return Width <= 0f ? null : new PointF(Width / 2.0f, 0.0f); } }
            public PointF? MeanMiddle { get { return Width <= 0f || Height <= 0f ? null : new PointF(Width / 2.0f, Height / 2.0f); } }
            public PointF? BottomMiddle { get { return Height <= 0f ? null : new PointF(0.0f, Height / 2.0f); } }

            public PointF XY { get; set; }

            public List<String> formulas { get; set; }
            public Quadro Esquerda { get; set; }
            public Quadro Direita { get; set; }

            private Quadro() { }

            public Quadro(Formulas f, float hchar = 10.0f, float wchar = 7.55f)
            {
                if (f == null) { return; }
                formulas = new();
                if (f.Negativas != null)
                {
                    f.Negativas.ForEach(x =>
                    {
                        if (x != null) { formulas.Add(x.ToString()); }
                    });
                }
                if (f.Positivas != null)
                {
                    f.Positivas.ForEach(x =>
                    {
                        if (x != null) { formulas.Add(x.ToString()); }
                    });
                }

                int lineMaxL = formulas.Max(x => x.Length) + 2; // 2 espaços: direita/esquerda
                Width = lineMaxL * wchar;
                int numL = formulas.Count(); // número de linhas
                Height = (numL + 1.5f) * hchar;

                if (f.Esquerda != null) { this.Esquerda = new Quadro(f.Esquerda); }
                if (f.Direita != null) { this.Direita = new Quadro(f.Direita); }
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
                return string.Format("[{0:0.00}x{1:0.00}][{2:0.00}x{3:0.00}] {4}", XY.X, XY.Y, Width, Height, fStr);
            }

        }


        private void drawFormula(Formulas f, Graphics g, Font fonte, float widthImg, float widthImgOriginal, float height = 0, float hchar = 10.0f, float wchar = 7.55f)
        {
            if (f == null || g == null) { return; }

            float widthMiddle = widthImg / 2.0f;
            float maxL = (Math.Max(f.Positivas == null ? 0 : f.Positivas.Max(x => x.ToString().Length), f.Negativas == null ? 0 : f.Negativas.Max(x => x.ToString().Length)) * wchar) / 2.0f;

            if (f.Positivas != null)
            {
                f.Positivas.ForEach(x => g.DrawString(x.ToString(), fonte, Brushes.Black, new PointF(widthMiddle - maxL, (height++) * hchar)));
            }
            if (f.Negativas != null)
            {
                f.Negativas.ForEach(x => g.DrawString(x.ToString(), fonte, Brushes.Black, new PointF(widthMiddle - maxL, (height++) * hchar)));
            }

            if (f.Esquerda != null)
            {
                drawFormula(f.Esquerda, g, fonte, widthMiddle, widthImgOriginal, height, hchar, wchar);
            }
            if (f.Direita != null)
            {
                drawFormula(f.Direita, g, fonte, widthImgOriginal + widthMiddle, widthImgOriginal, height, hchar, wchar);
            }
        }

        #region lines

        // se todas as fórmulas estivessem todas na mesma linha
        private int lineMaxLength(Formulas f, int espaco = 0)
        {
            if (f == null) { return 0; }
            int aux = Math.Max(f.Positivas == null ? 0 : f.Positivas.Max(x => x.ToString().Length), f.Negativas == null ? 0 : f.Negativas.Max(x => x.ToString().Length)) + espaco;
            return (aux + (f.Esquerda == null ? 0 : lineMaxLength(f.Esquerda) + espaco) + (f.Direita == null ? 0 : lineMaxLength(f.Direita)) + espaco);
        }

        // se todos os quadros estivessem todas na mesma linha
        private int lineMaxLength(Quadro q, int espaco = 0)
        {
            if (q == null) { return 0; }
            int aux = (q.formulas == null ? 0 : q.formulas.Max(x => x.Length)) + espaco;
            return (aux + (q.Esquerda == null ? 0 : lineMaxLength(q.Esquerda) + espaco) + (q.Direita == null ? 0 : lineMaxLength(q.Direita)) + espaco);
        }

        private int numLines(Formulas f)
        {
            if (f == null) { return 0; }
            int aux = (f.Positivas == null ? 0 : f.Positivas.Count()) + (f.Negativas == null ? 0 : f.Negativas.Count());
            return aux + Math.Max(f.Esquerda == null ? 0 : numLines(f.Esquerda), f.Direita == null ? 0 : numLines(f.Direita));
        }

        private int numLines(Quadro q)
        {
            if (q == null) { return 0; }
            int aux = q.formulas == null ? 0 : q.formulas.Count();
            return aux + Math.Max(q.Esquerda == null ? 0 : numLines(q.Esquerda), q.Direita == null ? 0 : numLines(q.Direita));
        }

        private Quadro? tratarYQuadros(Quadro q, float incrementH, float incrementHDivisao, float hchar = 10.0f, float wchar = 7.55f)
        {
            if (q == null) { return null; }

            q.XY = new PointF(0.0f, incrementH);

            //p(string.Format("q.Height: {0}, CF: {1}, incrementH: {2}", q.Height, q.ToString(), incrementH));

            if (q.Esquerda != null) { q.Esquerda = tratarYQuadros(q.Esquerda, incrementH + q.Height + incrementHDivisao, incrementHDivisao, hchar, wchar); }
            if (q.Direita != null) { q.Direita = tratarYQuadros(q.Direita, incrementH + q.Height + incrementHDivisao, incrementHDivisao, hchar, wchar); }

            return q;
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

        private List<Quadro>? plainQuadrosXY(List<Quadro> lquadros, float incrementoW, float incrementH, float hchar = 10.0f, float wchar = 7.55f)
        {
            if (lquadros == null) { return null; }

            lquadros.ForEach(q =>
            {
                float middleText = (q.formulas.Max(x => x.Length) * wchar) / 2.0f;
                float hPlus = incrementH + hchar * .25f;

                // Texto
                q.formulas.ForEach(f =>
                {
                    if (f == null || string.IsNullOrEmpty(f)) { return; }

                    string texto = f;
                    int lTexto = texto.Length;

                    //PointF pm = q1.MeanMiddle.Value;
                    PointF pm = q.XY;
                    pm.X = incrementoW;
                    //pm.X += incrementoW - middleText; //((lTexto * wchar) / 2.0f); - centraliza
                    //pm.Y += hPlus + q1.XY.Y;
                    q.XY = pm;

                    //hPlus += hchar;
                    // o incremento deve ser feito no momento de realizar o draw nas fórmulas
                });

                incrementoW += q.Width;
            });

            return lquadros;
        }

        #endregion

        #region x,y,h
        public class XYH
        {
            public float X { get; set; }
            public float Y { get; set; }
            public int H { get; set; }
            public int ML { get; set; }
            public ConjuntoFormula? CF { get; set; }

            public XYH copy()
            {
                return new XYH()
                {
                    X = this.X,
                    Y = this.Y,
                    H = this.H,
                    ML = this.ML,
                    CF = this.CF
                };
            }

            public override string ToString()
            {
                return string.Format("X: {0}, Y: {1}, H: {2}, ML: {3}, CF: {4}", X, Y, H, ML, CF?.ToString());
            }
        }

        public List<XYH> toXYH(Formulas f, float level, int pos, int maxElements, int height = 0)
        {

            float nAux = level <= 1 ? maxElements : maxElements / level;
            float posMap = (level == 0 ? nAux : nAux / 2 + pos * nAux) - 1;

            XYH xyh = new()
            {
                X = posMap,
                Y = level,
                //H = height + (f.Negativas == null ? 0 : f.Negativas.Count) + (f.Positivas == null ? 0 : f.Positivas.Count),
            };

            int ml = Math.Max(f.Negativas == null ? 0 : f.Negativas.Max(x => x == null ? 0 : x.ToString().Length), f.Positivas == null ? 0 : f.Positivas.Max(x => x == null ? 0 : x.ToString().Length));

            List<XYH> rt = new List<XYH>();
            f.Negativas?.ForEach(neg =>
            {
                XYH aux = xyh.copy();
                aux.CF = neg;
                aux.H = height++;
                aux.ML = ml;
                rt.Add(aux);
            });
            f.Positivas?.ForEach(pos =>
            {
                XYH aux = xyh.copy();
                aux.CF = pos;
                aux.H = height++;
                aux.ML = ml;
                rt.Add(aux);
            });


            if (f.Esquerda != null)
            {
                rt.AddRange(toXYH(f.Esquerda, level + 1, pos << 1, maxElements, height));
            }
            if (f.Direita != null)
            {
                rt.AddRange(toXYH(f.Direita, level + 1, (pos << 1) + 1, maxElements, height));
            }

            return rt;
        }

        #endregion

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


                // using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                // {
                //     //for (int i = 0; i < linhas.Length; i++) { graphics.DrawString(linhas[i], arialFont, Brushes.Black, new PointF(0,i+(i*20))); }

                //     // foreach (KeyValuePair<int, Dictionary<int, PosElement<Formulas>>> kvp1 in dic)
                //     // {
                //     //     int linha = kvp1.Key; // level ou linha
                //     //     foreach (KeyValuePair<int, PosElement<Formulas>> kvp2 in kvp1.Value)
                //     //     {
                //     //         int coluna = kvp2.Key;
                //     //         PosElement<Formulas> pel = kvp2.Value;
                //     //         Formulas faux = pel.Elemento;
                //     //         Console.WriteLine(string.Format("[{0},{1}] [{2},{3}]", linha, coluna, pel.Posicao, pel.Height));
                //     //         faux?.Negativas?.ForEach(x => Console.WriteLine(x));
                //     //         faux?.Positivas?.ForEach(x => Console.WriteLine(x));

                //     //         Console.WriteLine();
                //     //     }
                //     // }



                //     //graphics.DrawString(linhas[i], arialFont, Brushes.Black, new PointF(0, i + (i * 20)));


                // }

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

        private int heightTree(Formulas? f)
        {
            return f == null ? 0 : 1 + Math.Max(heightTree(f.Esquerda), heightTree(f.Direita));
        }

        private int heightTreeFormulas(Formulas? f)
        {
            if (f == null) { return 0; }
            int aux = f.Negativas == null ? 0 : f.Negativas.Count;
            aux += f.Positivas == null ? 0 : f.Positivas.Count;
            return aux + Math.Max(heightTreeFormulas(f.Esquerda), heightTreeFormulas(f.Direita));
        }

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion


    }

}