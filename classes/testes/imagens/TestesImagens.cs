using System.Drawing;
using System.Drawing.Drawing2D;
using classes.auxiliar;
using classes.formulas;
using classes.parser;
using classes.solverstage;
using classes.solverstage.print;

namespace classes.testes.imagens
{
    public class TestesImagens
    {

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
            Formulas f = getFormulaT();
            //f = getFormulaABCDEFG();
            int maxElements = (int)Math.Pow(2, heightTree(f) - 1);
            p(f.ToString());

            PrintFormulas pf = new PrintFormulas();
            Dictionary<int, Dictionary<int, PosElement<Formulas>>> dic = pf.toDict(f, 0, 0, maxElements, 0);

            foreach (KeyValuePair<int, Dictionary<int, PosElement<Formulas>>> kvp1 in dic)
            {
                int linha = kvp1.Key; // level ou linha
                foreach (KeyValuePair<int, PosElement<Formulas>> kvp2 in kvp1.Value)
                {
                    int coluna = kvp2.Key;
                    PosElement<Formulas> pel = kvp2.Value;
                    Formulas faux = pel.Elemento;
                    Console.WriteLine(string.Format("[{0},{1}] [{2},{3}]", linha, coluna, pel.Posicao, pel.Height));
                    faux?.Negativas?.ForEach(x => Console.WriteLine(x));
                    faux?.Positivas?.ForEach(x => Console.WriteLine(x));

                    Console.WriteLine();
                }
            }

            p("maxElements: " + maxElements + ", heightTree: " + heightTree(f));
            p(); p(""); p("");

            List<XYH> lxy = toXYH(f, 0, 0, maxElements, 0);
            lxy.ForEach(x => Console.WriteLine(x));

            int maxML = lxy.Max(xyh => xyh.ML);
            int maxX = lxy.Max(xyh => xyh.X);
            int maxY = lxy.Max(xyh => xyh.Y);
            int maxH = lxy.Max(xyh => xyh.H);
            p("maxX: " + maxX + ", maxML: " + maxML + ", maxY: " + maxY + ", maxH: " + maxH);



            //int widthImg = maxX * (maxML + 55);
            int widthImg = Convert.ToInt32(Math.Pow(heightTree(f), 2) * (maxML + 55));
            int heigthImg = maxY + (++maxH * 20);

            // max width image
            p(string.Format("max width image: {0}", widthImg));

            // max heigth image
            p(string.Format("max heigth image: {0}", heigthImg));


            //-----------------------------

            using Bitmap bitmap = new(widthImg, heigthImg);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.Clear(Color.White);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (Font fonte = new Font("Consolas", 10, FontStyle.Regular))
                {
                    //for (int i = 0; i < linhas.Length; i++) { graphics.DrawString(linhas[i], arialFont, Brushes.Black, new PointF(0,i+(i*20))); }

                    // foreach (KeyValuePair<int, Dictionary<int, PosElement<Formulas>>> kvp1 in dic)
                    // {
                    //     int linha = kvp1.Key; // level ou linha
                    //     foreach (KeyValuePair<int, PosElement<Formulas>> kvp2 in kvp1.Value)
                    //     {
                    //         int coluna = kvp2.Key;
                    //         PosElement<Formulas> pel = kvp2.Value;
                    //         Formulas faux = pel.Elemento;
                    //         Console.WriteLine(string.Format("[{0},{1}] [{2},{3}]", linha, coluna, pel.Posicao, pel.Height));
                    //         faux?.Negativas?.ForEach(x => Console.WriteLine(x));
                    //         faux?.Positivas?.ForEach(x => Console.WriteLine(x));

                    //         Console.WriteLine();
                    //     }
                    // }



                    //graphics.DrawString(linhas[i], arialFont, Brushes.Black, new PointF(0, i + (i * 20)));

                    lxy.ForEach(xyh =>
                    {
                        // esquerda, altura
                        graphics.DrawString(xyh.CF?.ToString(), fonte, Brushes.Black, new PointF(xyh.X * (xyh.ML + 50), xyh.Y + xyh.H * 20));
                    });

                }

            }

            bitmap.Save(@"imgformulas\bmp_formula.png");//save the image file

        }

        public void teste3()
        {
            Formulas f = getFormulaT();
            //PrintFormulas pf = new PrintFormulas();

            //int maxElements = (int)Math.Pow(2, pf.heightTree(f) - 1);
            //Dictionary<int, Dictionary<int, PosElement<Formulas>>> dic = pf.toDict(f, 0, 0, maxElements, 0);

            // 1º int: level ou linha
            // 2º int: coluna

            //string[] linhas = f.ToString().Split("\n");


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
                    //for (int i = 0; i < linhas.Length; i++) { graphics.DrawString(linhas[i], arialFont, Brushes.Black, new PointF(0,i+(i*20))); }

                    // foreach (KeyValuePair<int, Dictionary<int, PosElement<Formulas>>> kvp1 in dic)
                    // {
                    //     int linha = kvp1.Key; // level ou linha
                    //     foreach (KeyValuePair<int, PosElement<Formulas>> kvp2 in kvp1.Value)
                    //     {
                    //         int coluna = kvp2.Key;
                    //         PosElement<Formulas> pel = kvp2.Value;
                    //         Formulas faux = pel.Elemento;
                    //         Console.WriteLine(string.Format("[{0},{1}] [{2},{3}]", linha, coluna, pel.Posicao, pel.Height));
                    //         faux?.Negativas?.ForEach(x => Console.WriteLine(x));
                    //         faux?.Positivas?.ForEach(x => Console.WriteLine(x));

                    //         Console.WriteLine();
                    //     }
                    // }



                    //graphics.DrawString(linhas[i], arialFont, Brushes.Black, new PointF(0, i + (i * 20)));
                }

                //StringFormat sf = new StringFormat();
                //sf.Alignment = StringAlignment.Center;
                //sf.LineAlignment = StringAlignment.Center;

                //graphics.DrawString("My\nText", new System.Drawing.Font("Consola", 15, FontStyle.Regular), Brushes.Black, new PointF(100f, 100f), sf);


                // for (int i = 0; i <= 500; i += 15)
                // {
                //     for (int j = 0; j <= 600; j += 21)
                //     {
                //         graphics.DrawString("0", new System.Drawing.Font("Consola", 15, FontStyle.Regular), i == 0 || i % 2 == 0 ? Brushes.Blue : Brushes.Green, new PointF(i, j), sf);
                //     }
                // }

            }

            bitmap.Save(@"imgformulas\bmp_formula.png");//save the image file
        }

        #region x,y,h
        public class XYH
        {
            public int X { get; set; }
            public int Y { get; set; }
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
                return string.Format("{0}, {1}, {2}, {3}, {4}", X, Y, H, ML, CF?.ToString());
            }
        }

        public List<XYH> toXYH(Formulas f, int level, int pos, int maxElements, int height = 0)
        {

            int nAux = level <= 1 ? maxElements : maxElements / level;
            int posMap = (level == 0 ? nAux : nAux / 2 + pos * nAux) - 1;

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
            f.addEsquerda(parser.parserCF("B"));
            f.addDireita(parser.parserCF("E"));

            f.Esquerda.addEsquerda(parser.parserCF("C"));
            f.Esquerda.addDireita(parser.parserCF("D"));

            f.Direita.addEsquerda(parser.parserCF("F"));
            f.Direita.addDireita(parser.parserCF("G"));

            return f;
        }

        private int heightTree(Formulas? f)
        {
            return f == null ? 0 : 1 + Math.Max(heightTree(f.Esquerda), heightTree(f.Direita));
        }

        #region auxiliar
        private void p() { UtilFormulas.p(); }
        private void p(string str) { UtilFormulas.p(str); }
        private string toStr<T>(IEnumerable<T> values, String? separator = " ") { return UtilFormulas.toStr(values, separator); }
        #endregion


    }

}