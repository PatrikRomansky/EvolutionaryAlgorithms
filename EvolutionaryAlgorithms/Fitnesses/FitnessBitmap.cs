using Accord.Imaging.Filters;
using Emgu.CV.Dnn;
using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace EvolutionaryAlgorithms.Fitnesses
{

    /// <summary>
    /// Fitness for individual, which genes represent Bitmap image (RGB).
    /// </summary>
    public class FitnessBitmap : IFitness
    {
        protected Color[] target;
        protected int targetSize;

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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        public FitnessBitmap(object target)
        {
            Initialize(target);
        }

        private double PixelDifference(Color px1, Color px2)
        {
            // return (px1.R - px2.R) * (px1.R - px2.R) + (px1.B - px2.B) * (px1.B - px2.B) + (px1.G - px2.G) * (px1.G - px2.G);
            return Math.Abs(px1.R - px2.R) + Math.Abs(px1.B - px2.B) + Math.Abs(px1.G - px2.G);
        }

        /// <summary>
        /// Performs the evaluation against the specified individual.
        /// </summary>
        /// <param name="individual">The individual to be evaluated.</param>
        /// <returns>The fitness of the individual.</returns>
        public double Evaluate(IIndividual individual)
        {
            double fitness = 0;

            var genes = individual.GetPhenotype();

            for (int i = 0; i < this.targetSize; i++)
            {
                fitness += PixelDifference(this.target[i], (Color)genes[i]);
            }
            return 1 / (1 + fitness);
        }
    }
}
