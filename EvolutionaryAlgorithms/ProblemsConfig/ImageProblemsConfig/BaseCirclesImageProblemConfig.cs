﻿using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    public class BaseCirclesImageProblemConfig: ShapesImageProblemConfig
    {

        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateIndividual()
        {
            return new IndividualCircles(width, height, indSize);
        }

        public override IIndividual CreateEmptyIndividual()
        {
            return new IndividualCircles(width, height, indSize, false);
        }
    }
}
