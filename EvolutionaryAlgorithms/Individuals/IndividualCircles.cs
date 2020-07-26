using Emgu.CV;
using Emgu.CV.Structure;
using EvolutionaryAlgorithms.Randomization;
using System;
using System.Collections.Generic;
using System.Drawing;
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

    public class IndividualCircles : IndividualShapes
    {
        /// <summary>
        /// Initializes a new instance of the IndividualBitmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualCircles(int width, int height, int numberOfShapes, bool init = true)
            : base(width, height, numberOfShapes * 6)
        {
            geneSize = 6;

            this.numberOfShapes = numberOfShapes;

            // Init
            if (init)
            {

                for (int i = 0; i < numberOfShapes; i++)
                {
                    genes[(i*geneSize)] = FastRandom.GetInt(0, Width);
                    genes[(i * geneSize) + 1] = FastRandom.GetInt(0, Height);
                    genes[(i * geneSize) + 2] = FastRandom.GetInt(0, 25);

                    genes[(i * geneSize) + 3] = FastRandom.GetDouble() * 255;
                    genes[(i * geneSize) + 4] = FastRandom.GetDouble() * 255;
                    genes[(i * geneSize) + 5] = FastRandom.GetDouble() * 255;


                }
            }
        }

        public override IIndividual CreateNew()
        {
            var newInd = new IndividualCircles(Width, Height, numberOfShapes, false);

            newInd.genes = new double[this.Length];
            newInd.Length = this.Length;

            return newInd;
        }

        public override object[] GetPhenotype()
        {
            var result = new Object[numberOfShapes];


            for (int i = 0; i < numberOfShapes; i++)
            {

                var c = new MCvScalar(genes[(i * geneSize) + 3], genes[(i * geneSize) + 4], genes[(i * geneSize) + 5]);

                result[i] = new Circle((int)genes[(i * geneSize)], (int)genes[(i * geneSize) + 1], (int)genes[(i * geneSize) + 2], c);
            }


            return result;
        }

        protected override void DrawShape(Image<Bgr, byte> img, object shape)
        {
            var cir = (Circle) shape;

            CvInvoke.Circle(img, cir.center, cir.radius, cir.color, -1);
        }

        protected override double GetMaxGeneValue(int geneIndex)
        {
            int mod = geneIndex % geneSize;

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
            return coeficient;
        }
    }
}
