using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Randomization;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionaryAlgorithms.Operators.Xovers
{
    /// <summary>
    /// The two points crossover operator.
    /// </summary>
    public class XoverTwoPoints : Xover
    {
        int minParentSize;

        /// <summary>
        /// Initializes a new instance of Two points crossover.
        /// </summary>
        public XoverTwoPoints()
        {
            ParentsNumber = 2;
            ChildrenNumber = 2;
            minParentSize = 3;
        }

        /// <summary>
        /// Cross the specified parents generating the children.
        /// </summary>
        /// <param name="parents">The parents.</param>
        /// <returns>The offspring (children) of the parents.</returns>
        public override IList<IIndividual> Cross(IList<IIndividual> parents)
        {
            // This xover need same length
            if (parents[0].Length != parents[1].Length)
                return parents;

            // minimal parents lenght
            if (parents[0].Length <= minParentSize)
                return parents;

            // cross points
            var crossPoints = FastRandom.GetUniqueInts(2, 1, parents[0].Length - 2);

            // new individuals
            int firstIndex = crossPoints[0];
            int secondIndex = crossPoints[1];

            if (secondIndex < firstIndex)
            {
                secondIndex = crossPoints[0];
                firstIndex = crossPoints[1];
            }

            var firstChild = CreateChild(parents[0], parents[1], firstIndex, secondIndex);
            var secondChild = CreateChild(parents[1], parents[0], firstIndex, secondIndex);

            return new List<IIndividual>() { firstChild, secondChild };
        }

        /// <summary>
        /// Creates the child.
        /// </summary>
        /// <param name="leftParent">Left parent.</param>
        /// <param name="rightParent">Right parent</param>
        /// <param name="firstSwapPoint">he index of the swap point one gene.</param>
        /// <param name="secondSwapPoint">he index of the swap point two gene.</param>
        /// <returns>The child.</returns>
        protected IIndividual CreateChild(IIndividual leftParent, IIndividual rightParent, int firstSwapPoint, int secondSwapPoint)
        {
            var firstCutGenesCount = firstSwapPoint + 1;
            var secondCutGenesCount = secondSwapPoint + 1;
            var child = leftParent.CreateNew();
            child.ReplaceGenes(0, leftParent.GetGenes().Take(firstCutGenesCount).ToArray());
            child.ReplaceGenes(firstCutGenesCount, rightParent.GetGenes().Skip(firstCutGenesCount).Take(secondCutGenesCount - firstCutGenesCount).ToArray());
            child.ReplaceGenes(secondCutGenesCount, leftParent.GetGenes().Skip(secondCutGenesCount).ToArray());

            return child;
        }
    }
}
