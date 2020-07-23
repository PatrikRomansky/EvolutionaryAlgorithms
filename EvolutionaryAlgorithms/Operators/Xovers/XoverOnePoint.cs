using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Randomization;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionaryAlgorithms.Operators.Xovers
{
    /// <summary>
    /// One point crossover operator.
    /// </summary>
    public class XoverOnePoint : Xover
    {

        /// <summary>
        /// Initializes a new instance of One point crossover.
        /// </summary>
        public XoverOnePoint()
        {
            ParentsNumber = 2;
            ChildrenNumber = 2;
        }

        /// <summary>
        /// Cross the specified parents generating the children.
        /// </summary>
        /// <param name="parents">The parents</param>
        /// <returns>The offspring (children) of the parents.</returns>
        public override IList<IIndividual> Cross(IList<IIndividual> parents)
        {
            // same length
            if (parents[0].Length != parents[1].Length)
                return parents;

            int swapPoint = FastRandom.GetInt(1, parents[0].Length - 2);

            var firstChild = CreateChild(parents[0], parents[1], swapPoint);
            var secondChild = CreateChild(parents[1], parents[0], swapPoint);

            return new List<IIndividual>() { firstChild, secondChild };

        }

        /// <summary>
        /// Creates the child.
        /// </summary>
        /// <param name="leftParent">Left parent.</param>
        /// <param name="rightParent">Right parent</param>
        /// <param name="swapPoint">The index of the swap point.</param>
        /// <returns>The child.</returns>
        protected virtual IIndividual CreateChild(IIndividual leftParent, IIndividual rightParent, int swapPoint)
        {
            var cutGenesCount = swapPoint + 1;
            var child = leftParent.CreateNew();
            child.ReplaceGenes(0, leftParent.GetGenes().Take(cutGenesCount).ToArray());
            child.ReplaceGenes(cutGenesCount, rightParent.GetGenes().Skip(cutGenesCount).ToArray());

            return child;
        }
    }
}
