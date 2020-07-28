using Emgu.CV;
using Emgu.CV.Structure;
using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    public class BaseTrianglesImageProblemConfig : ShapesImageProblemConfig
    {
        /// <summary>
        /// Creates the individual tri.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateEmptyIndividual()
        {
            return new IndividualTriangles(width, height, indSize, false);
        }

        /// <summary>
        /// Creates empty individual rectangles.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateIndividual()
        {
            return new IndividualTriangles(width, height, indSize);
        }
    }
}
