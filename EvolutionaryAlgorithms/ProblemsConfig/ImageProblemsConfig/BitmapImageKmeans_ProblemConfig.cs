using EvolutionaryAlgorithms.ImageProcessing;
using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    class BitmapImageKmeans_ProblemConfig: BaseBitmapImageProblemConfig
    {
        private Color[] initColors;

        private int k = 10;

        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateIndividual()
        {
            return new IndividualBitmap(width, height, true, initColors);
        }

        public override void Initialize(string targetInputFile)
        {
  
            base.Initialize(targetInputFile);

            var targetBitmap = base.InititializeSame(targetInputFile);
            initColors = Kmeans.GetDominanteColors(targetBitmap, k);
        }


        public override void Initialize(object target,string targetInputFile)
        {

            base.Initialize(target, targetInputFile);

            var targetBitmap = target as Bitmap;
            initColors = Kmeans.GetDominanteColors(targetBitmap, k);
        }
    }
}
