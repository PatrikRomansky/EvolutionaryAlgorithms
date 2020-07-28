using Emgu.CV;
using Emgu.CV.Structure;
using EvolutionaryAlgorithms.Randomization;
using System;
using System.Drawing;

namespace EvolutionaryAlgorithms.Individuals
{
    /// <summary>
    /// Triangle gene instance.
    /// </summary>
    public class GeneTriangle
    {
        public Point[] vertices;

        public Bgr color;


        public GeneTriangle(Point v1, Point v2, Point v3, Bgr color)
        {
            this.vertices = new Point[] { v1, v2, v3 };

            this.color = color;
        }
    }

    public class IndividualTriangles : IndividualShapes
    {
        const int size = 9;

        /// <summary>
        /// Initializes a new instance of the IndividualRriangles.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualTriangles(int width, int height, int numberOfShapes, bool init = true)
            : base(width, height, numberOfShapes * size)
        {
            geneSize = size;

            this.numberOfShapes = numberOfShapes;

            // Init
            if (init)
            {

                for (int i = 0; i < numberOfShapes; i++)
                {
                    //1. vertex
                    genes[(i * geneSize)] = FastRandom.GetInt(0, Width);
                    genes[(i * geneSize) + 1] = FastRandom.GetInt(0, Height);

                    // 2. vertex
                    genes[(i * geneSize) + 2] = FastRandom.GetInt(0, Width);
                    genes[(i * geneSize) + 3] = FastRandom.GetInt(0, Height);

                    // 3. vertex
                    genes[(i * geneSize) + 4] = FastRandom.GetInt(0, Width);
                    genes[(i * geneSize) + 5] = FastRandom.GetInt(0, Height);


                    // color 
                    genes[(i * geneSize) + 6] = FastRandom.GetDouble() * 255;
                    genes[(i * geneSize) + 7] = FastRandom.GetDouble() * 255;
                    genes[(i * geneSize) + 8] = FastRandom.GetDouble() * 255;


                }
            }
        }

        public override IIndividual CreateNew()
        {
            var newInd = new IndividualTriangles(Width, Height, numberOfShapes, false);

            newInd.genes = new double[this.Length];
            newInd.Length = this.Length;

            return newInd;
        }

        public override object[] GetPhenotype()
        {
            var result = new Object[numberOfShapes];

            for (int i = 0; i < numberOfShapes; i++)
            {
                var v1 = new Point((int)genes[(i * geneSize)], (int)genes[(i * geneSize) + 1]);
                var v2 = new Point((int)genes[(i * geneSize) + 2], (int)genes[(i * geneSize) + 3]);
                var v3 = new Point((int)genes[(i * geneSize) + 4], (int)genes[(i * geneSize) + 5]);


                var c = new Bgr(genes[(i * geneSize) + 6], genes[(i * geneSize) + 7], genes[(i * geneSize) + 8]);

                result[i] = new GeneTriangle(v1, v2, v3, c);
            }


            return result;
        }

        protected override void DrawShape(Image<Bgr, byte> img, object shape)
        {
            var tri = (GeneTriangle)shape;

            img.FillConvexPoly(tri.vertices, tri.color);
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
                    coeficient = Width;
                    break;
                case (3):
                    coeficient = Height;
                    break;
                case (4):
                    coeficient = Width;
                    break;
                case (5):
                    coeficient = Height;
                    break;
                default:
                    coeficient = 255;
                    break;
            }
            return coeficient;
        }
    }
}
