using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    public class BitmapImageKmeansProblemConfig: BitmapImageKmeansInitProblemConfig
    {
        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateIndividual()
        {
            return new IndividualKmeansBitmap(width, height, true, initColors);
        }
    }
}
