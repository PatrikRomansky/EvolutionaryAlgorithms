using Emgu.CV;
using Emgu.CV.Structure;
using EvolutionaryAlgorithms.Randomization;
using System;
using System.Drawing;

namespace EvolutionaryAlgorithms.Individuals
{
    /// <summary>
    /// This class represents the individual (image).
    /// The image passes edge detection (canny detection) and then lienes detection (HoughLinesP).
    /// Individual genes represent line image (sketch).
    /// </summary>
    public class IndividualLines : IndividualImage
    {

        private int numberOfLines;

        /// <summary>
        /// Initializes a new instance of the IndividualBitmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualLines(int width, int height, int numberOfLines, bool init = true)
            : base(width, height, numberOfLines * 4)
        {

            this.numberOfLines = numberOfLines;

            // Init
            if (init)
            {

                int indexX1 = 0;
                int indexY1 = 1;
                int indexX2 = 2;
                int indexY2 = 3;

                for (int i = 0; i < numberOfLines; i++)
                {
                    var sX = FastRandom.GetInt(0, width);
                    var sY = FastRandom.GetInt(0, height);
                    var shifts = FastRandom.GetInts(2, 0, 30);

                    genes[indexX1] = sX;
                    genes[indexY1] = sY;
                    genes[indexX2] = sX + shifts[0];
                    genes[indexY2] = sY + shifts[1];

                    indexX1 += 4;
                    indexY1 += 4;
                    indexX2 += 4;
                    indexY2 += 4;
                }
            }
        }

        /// <summary>
        /// Generates the gene.
        /// </summary>
        /// <param name="geneIndex">Index of the gene.</param>
        /// <returns>The new Gene.</returns>
        public override double GenerateGene(int geneIndex)
        {
            if (geneIndex % 2 == 0)
            {
                return FastRandom.GetFloat() * Width;
            }
            else
            {
                return FastRandom.GetFloat() * Height;
            }
        }

        /// <summary>
        /// Creates a new individual using the same structure of this.
        /// </summary>
        /// <returns>
        /// The new individual.
        /// </returns>
        public override IIndividual CreateNew()
        {
            var newInd = new IndividualLines(Width, Height, numberOfLines, false);

            newInd.genes = new double[this.Length];
            newInd.Length = this.Length;

            return newInd;
        }

        /// <summary>
        /// Builds the bitmap from genes.
        /// </summary>
        /// <returns>The bitmap.</returns>
        public override Bitmap BuildBitmap()
        {
            var phenotype = this.GetPhenotype();


            Image<Bgr, Byte> img = new Image<Bgr, Byte>(Width, Height, new Bgr(255, 255, 255));
            foreach (var lineObject in phenotype)
            {

                var line = (LineSegment2D)lineObject;

                CvInvoke.Line(img, line.P1, line.P2, new MCvScalar(0, 0, 0));
            }

            return img.ToBitmap<Bgr, Byte>();
        }

        /// <summary>
        /// Gets the phenotype.
        /// </summary>
        /// <returns>The phenotype (representation).</returns>
        public override Object[] GetPhenotype()
        {
            var result = new Object[numberOfLines];

            int indexX1 = 0;
            int indexY1 = 1;
            int indexX2 = 2;
            int indexY2 = 3;

            for (int i = 0; i < numberOfLines; i++)
            {

                var p1 = new Point((int)genes[indexX1], (int)genes[indexY1]);
                var p2 = new Point((int)genes[indexX2], (int)genes[indexY2]);

                result[i] = new LineSegment2D(p1, p2);

                indexX1 += 4;
                indexY1 += 4;
                indexX2 += 4;
                indexY2 += 4;
            }


            return result;
        }

        /// <summary>
        /// Replaces the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index to replace.</param>
        /// <param name="gene">The new gene.</param>
        public override void ReplaceGene(int index, double gene)
        {
            maxGeneValue = Height;

            if (index % 2 == 0)
                maxGeneValue = Width;

            if (gene > maxGeneValue)
                gene = maxGeneValue;

            if (gene < minGeneValue)
                gene = minGeneValue;

            this.genes[index] = gene;
            Fitness = null;
        }
    }
}