using EvolutionaryAlgorithms.Randomization;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EvolutionaryAlgorithms.Individuals
{
    /// <summary>
    /// This class represents the individual (image).
    /// Individual genes represent image pixels (RGB).
    /// </summary>
    public class IndividualBitmap : IndividualImage
    {
        /// <summary>
        /// Initializes a new instance of the IndividualBitmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualBitmap(int width, int height, bool init = true, Color[] initColors = null)
            : base(width, height, width * height * 3)
        {
            // Init
            if (init)
            {
                if (initColors == null)
                {
                    for (int i = 0; i < Length; i++)
                    {
                        ReplaceGene(i, GenerateGene(i));
                    }
                }
                else
                {
                    int indexR = 0;
                    int indexG = 1;
                    int indexB = 2;
                    for (int i = 0; i < width * height; i++)
                    {
                        Color c = initColors[FastRandom.GetInt(0, initColors.Length)];

                        genes[indexR] = c.R;
                        genes[indexG] = c.G;
                        genes[indexB] = c.B;

                        indexR += 3;
                        indexG += 3;
                        indexB += 3;
                    }
                
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
            return FastRandom.GetFloat() * maxGeneValue;
        }

        /// <summary>
        /// Creates a new individual using the same structure of this.
        /// </summary>
        /// <returns>
        /// The new individual.
        /// </returns>
        public override IIndividual CreateNew()
        {
            var newInd = new IndividualBitmap(Width, Height, false);

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
            var result = new Bitmap(Width, Height);
            var phenotypeIndex = 0;

            var phenotype = this.GetPhenotype();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    result.SetPixel(x, y, (Color)phenotype[phenotypeIndex++]);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the phenotype.
        /// </summary>
        /// <returns>The phenotype (representation).</returns>
        public override Object[] GetPhenotype()
        {
            var result = new Object[Width * Height];

            int indexR = 0;
            int indexG = 1;
            int indexB = 2;

            for(var i= 0; i < Width*Height; i++)
            {
                result[i] = Color.FromArgb((int)genes[indexR], (int)genes[indexG], (int)genes[indexB]);
                indexR+=3;
                indexG+=3;
                indexB+=3;
            }


            return result;
        }
    }
}
