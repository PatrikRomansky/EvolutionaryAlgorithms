using Emgu.CV;
using Emgu.CV.Structure;
using EvolutionaryAlgorithms.Randomization;
using System;
using System.Drawing;

namespace EvolutionaryAlgorithms.Individuals
{
    public class GeneRectangle
    {
        public Rectangle area;

        public MCvScalar color;
        public GeneRectangle(int x, int y, int width, int height, MCvScalar color)
        {
            this.area = new Rectangle(x, y, width, height);

            this.color = color;
        }
    }

    public class IndividualRectangles : IndividualShapes
    {
        int maxWidth = 40;
        int maxHeight = 40;

        const int size = 7;

        /// <summary>
        /// Initializes a new instance of the IndividualRectangles.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualRectangles(int width, int height, int numberOfShapes, bool init = true)
            : base(width, height, numberOfShapes * size)
        {
            geneSize = size;

            this.numberOfShapes = numberOfShapes;

            // Init
            if (init)
            {

                for (int i = 0; i < numberOfShapes; i++)
                {   
                    //left corner
                    genes[(i * geneSize)] = FastRandom.GetInt(0, Width);
                    genes[(i * geneSize) + 1] = FastRandom.GetInt(0, Height);

                    // width
                    genes[(i * geneSize) + 2] = FastRandom.GetInt(0, maxWidth);

                    // height
                    genes[(i * geneSize) + 3] = FastRandom.GetInt(0, maxHeight);

                    // color 
                    genes[(i * geneSize) + 4] = FastRandom.GetDouble() * 255;
                    genes[(i * geneSize) + 5] = FastRandom.GetDouble() * 255;
                    genes[(i * geneSize) + 6] = FastRandom.GetDouble() * 255;


                }
            }
        }

        public override IIndividual CreateNew()
        {
            var newInd = new IndividualRectangles(Width, Height, numberOfShapes, false);

            newInd.genes = new double[this.Length];
            newInd.Length = this.Length;

            return newInd;
        }

        public override object[] GetPhenotype()
        {
            var result = new Object[numberOfShapes];


            for (int i = 0; i < numberOfShapes; i++)
            {

                var c = new MCvScalar(genes[(i * geneSize) + 4], genes[(i * geneSize) + 5], genes[(i * geneSize) + 6]);

                result[i] = new GeneRectangle((int)genes[(i * geneSize)], (int)genes[(i * geneSize) + 1], (int)genes[(i * geneSize) + 2], (int)genes[(i * geneSize) + 3], c);
            }


            return result;
        }

        protected override void DrawShape(Image<Bgr, byte> img, object shape)
        {
            var rec = (GeneRectangle)shape;

            CvInvoke.Rectangle(img, rec.area, rec.color, -1);
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
                    coeficient = maxWidth;
                    break;
                case (3):
                    coeficient = maxHeight;
                    break;
                default:
                    coeficient = 255;
                    break;
            }
            return coeficient;
        }
    }
}
