using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.Fitnesses
{
    public class FitnessShapes : IFitness
    {
        protected Color[] target;
        protected int targetSize;


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="target">The target bitmap.</param>
        public FitnessShapes(object target)
        {
            Initialize(target);
        }

        public double Evaluate(IIndividual individual)
        {
            double fitness = 0;


            var ind = individual as IndividualShapes;
            var bitmap = ind.BuildBitmap();

            int i = 0;
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    fitness += PixelDifference(this.target[i], (Color)bitmap.GetPixel(x, y));
                    i++;
                }
            }
            return fitness;
        }

        private double PixelDifference(Color px1, Color px2)
        {
            // return (px1.R - px2.R) * (px1.R - px2.R) + (px1.B - px2.B) * (px1.B - px2.B) + (px1.G - px2.G) * (px1.G - px2.G);
            return Math.Abs(px1.R - px2.R) + Math.Abs(px1.B - px2.B) + Math.Abs(px1.G - px2.G);
        }


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
