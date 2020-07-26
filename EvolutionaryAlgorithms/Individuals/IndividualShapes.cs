using Accord.Math.Geometry;
using Emgu.CV;
using Emgu.CV.Structure;
using EvolutionaryAlgorithms.Randomization;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.Individuals
{
    public class Circle 
    {
        public Point center;
        public int radius;
        public MCvScalar color;
        public Circle(int x, int y, int radius, MCvScalar color)
        {
            this.center = new Point(x, y);
            this.radius = radius;
            this.color = color;
        }

        
    }

    public class IndividualShapes : IndividualImage
    {
        protected int numberOfShapes;

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; private set; }



        public override Bitmap BuildBitmap()
        {
            var phenotype = this.GetPhenotype();
            /*
            var bitmap = new Bitmap(Width, Height);
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;

                foreach (var p in phenotype)
                {
                    var c = p as Circle;

                    SolidBrush iconBrush = new SolidBrush(c.color);
                    gr.FillEllipse(iconBrush, new Rectangle(c.center.X, c.center.Y, c.radius, c.radius));
                }
            }

            return bitmap;

            */

            Image<Bgr, Byte> img = new Image<Bgr, Byte>(Width, Height, new Bgr(255, 255, 255));
            foreach (var lineObject in phenotype)
            {

                var line = (Circle)lineObject;

                CvInvoke.Circle(img, line.center, line.radius, line.color, -1);
            }

            return img.ToBitmap<Bgr, Byte>();

        }

        /// <summary>
        /// Replaces the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index to replace.</param>
        /// <param name="gene">The new gene.</param>
        public override void ReplaceGene(int index, double gene)
        {
            maxGeneValue = Width;

            if (index % 6 == 1)
                maxGeneValue = Width;

            if (index % 6 == 2)
                maxGeneValue = 25;

            if (index % 6 == 3 || index % 6 == 4 || index % 6 == 5)
                maxGeneValue = 255;

           if (gene > maxGeneValue)
                gene = maxGeneValue;

            if (gene < minGeneValue)
                gene = minGeneValue;

            this.genes[index] = gene;
            Fitness = null;
        }

        public override double GenerateGene(int geneIndex)
        {
            int mod = geneIndex % 6;

            double coeficient;

            switch (mod)
            {
                case (0):
                    coeficient = Width;
                    break;
                case (1):
                    coeficient = Height;
                    break;
                case (2):
                    coeficient = 25;
                    break;
                default:
                    coeficient = 255;
                    break;
            }
            return FastRandom.GetDouble() * coeficient;
        }

        public override IIndividual CreateNew()
        {
            var newInd = new IndividualShapes(Width, Height, numberOfShapes, false);

            newInd.genes = new double[this.Length];
            newInd.Length = this.Length;

            return newInd;
        }

        public override object[] GetPhenotype()
        {
            var result = new Object[numberOfShapes];

            int indexX1 = 0;
            int indexY1 = 1;
            int indexRadius = 2;
            int indexR = 3;
            int indexG = 4;
            int indexB = 5;


            for (int i = 0; i < numberOfShapes; i++)
            {
                var c = new MCvScalar(genes[indexR], genes[indexG], genes[indexB]);
                result[i] = new Circle((int)genes[indexX1], (int)genes[indexY1], (int)genes[indexRadius], c);

                indexX1 += 6;
                indexY1 += 6;
                indexRadius += 6;
                indexR += 6;
                indexG += 6;
                indexB += 6;
            }


            return result;
        }


        /// <summary>
        /// Initializes a new instance of the IndividualBitmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualShapes(int width, int height, int numberOfShapes, bool init = true):base(numberOfShapes * 6)
        {
            Width = width;
            Height = height;
            this.numberOfShapes = numberOfShapes;

            // Init
            if (init)
            {
                int indexX1 = 0;
                int indexY1 = 1;
                int indexRadius = 2;
                int indexR = 3;
                int indexG = 4;
                int indexB = 5;


                for (int i = 0; i <  numberOfShapes; i++)
                {
                    genes[indexX1] = FastRandom.GetInt(0, Width);
                    genes[indexY1] = FastRandom.GetInt(0, Height);
                    genes[indexRadius] = FastRandom.GetInt(0, 25);

                    genes[indexR] = FastRandom.GetDouble() * 255;
                    genes[indexG] = FastRandom.GetDouble() * 255;
                    genes[indexB] = FastRandom.GetDouble() * 255;

                    indexX1 += 6;
                    indexY1 += 6;
                    indexRadius += 6;
                    indexR += 6;
                    indexG += 6;
                    indexB += 6;
                }
            }
        }
    }
}
