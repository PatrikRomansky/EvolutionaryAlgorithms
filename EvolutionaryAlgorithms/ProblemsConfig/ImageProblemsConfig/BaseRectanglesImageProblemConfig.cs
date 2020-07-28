using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    public class BaseRectanglesImageProblemConfig: ShapesImageProblemConfig
    {
        /// <summary>
        /// Creates the individual rectangles.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateIndividual()
        {
            return new IndividualRectangles(width, height, indSize);
        }

        /// <summary>
        /// Creates empty individual rectangles.
        /// </summary>
        /// <returns></returns>
        public override IIndividual CreateEmptyIndividual()
        {
            return new IndividualRectangles(width, height, indSize, false);
        }
    }
}
