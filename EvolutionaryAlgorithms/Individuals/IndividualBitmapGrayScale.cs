using EvolutionaryAlgorithms.Randomization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.Individuals
{
    public class IndividualBitmapGrayScale : IndividualImage
    {
        /// <summary>
        /// Initializes a new instance of the IndividualBitmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualBitmapGrayScale(int width, int height, bool init = true)
            : base(width, height, width * height)
        {
            // Init
            if (init)
            {
                for (int i = 0; i < Length; i++)
                {
                    ReplaceGene(i, GenerateGene(i));
                }
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
            var newInd = new IndividualBitmapGrayScale(Width, Height, false);

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

            for (var i = 0; i < Width * Height; i++)
            {
                result[i] = Color.FromArgb((int)genes[i], (int)genes[i], (int)genes[i]);
  
            }

            return result;
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
    }
}
