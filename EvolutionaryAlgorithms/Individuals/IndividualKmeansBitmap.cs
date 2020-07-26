using EvolutionaryAlgorithms.Randomization;
using System.Drawing;

namespace EvolutionaryAlgorithms.Individuals
{
    public class IndividualKmeansBitmap : IndividualBitmap
    {
        private Color[] colorsDomain;

        /// <summary>
        /// Initializes a new instance of the IndividualBitmap.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualKmeansBitmap(int width, int height, bool init = true, Color[] initColors = null)
            : base(width, height, init, initColors)
        {

            this.colorsDomain = initColors;
        }

        /// <summary>
        /// Generates the gene.
        /// </summary>
        /// <param name="geneIndex">Index of the gene.</param>
        /// <returns>The new Gene.</returns>
        public override double GenerateGene(int geneIndex)
        {
            var colorIndex = FastRandom.GetInt(0, colorsDomain.Length);
            var color = colorsDomain[colorIndex];

            if (geneIndex % 3 == 0)
                return color.R;

            if (geneIndex % 3 == 1)
                return color.G;

            return color.B;
        }

        /// <summary>
        /// Creates a new individual using the same structure of this.
        /// </summary>
        /// <returns>
        /// The new individual.
        /// </returns>
        public override IIndividual CreateNew()
        {
            var newInd = new IndividualKmeansBitmap(Width, Height, false);

            newInd.genes = new double[this.Length];
            newInd.Length = this.Length;

            return newInd;
        }
    }
}
