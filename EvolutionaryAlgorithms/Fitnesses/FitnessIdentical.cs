using EvolutionaryAlgorithms.Individuals;
using System.Drawing;

namespace EvolutionaryAlgorithms.Fitnesses
{
    /// <summary>
    /// Return count of different px.
    /// </summary>
    public class FitnessIdentical : IFitness
    {
        protected Color[] target;
        protected int targetSize;
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        public FitnessIdentical(object target)
        {
            Initialize(target);
        }

        public double Evaluate(IIndividual individual)
        {
            double fitness = 0;

            var genes = individual.GetPhenotype();

            for (int i = 0; i < this.targetSize; i++)
            {

                if (this.target[i] != (Color)genes[i])
                {
                    fitness++;
                }
            }
            return fitness;
        }

        /// <summary>
        /// Transform bitmap to genes.
        /// </summary>
        /// <param name="target">Bitmap.</param>
        public void Initialize(object target)
        {
            var bitmap = target as Bitmap;
            var width = bitmap.Width;
            var height = bitmap.Height;


            this.target = new Color[width * height];

            int i = 0;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    this.target[i] = bitmap.GetPixel(x, y);
                    i++;
                }
            }

            targetSize = this.target.Length;
        }
    }
}
