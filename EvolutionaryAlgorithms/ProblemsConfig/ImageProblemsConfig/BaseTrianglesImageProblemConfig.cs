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
        public override IIndividual CreateEmptyIndividual()
        {
            return new IndividualTriangles(width, height, indSize, false);
        }

        public override IIndividual CreateIndividual()
        {
            return new IndividualTriangles(width, height, indSize);
        }
    }
}
