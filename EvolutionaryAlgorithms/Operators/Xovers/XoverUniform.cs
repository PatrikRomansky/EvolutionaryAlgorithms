using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Randomization;
using System.Collections.Generic;

namespace EvolutionaryAlgorithms.Operators.Xovers
{
    /// <summary>
    /// Uniform crossover operator.
    /// </summary>
    public class XoverUniform : Xover
    {
        // unofrom corss probability
        private float mixProbability;

        /// <summary>
        /// Initializes a new instance of uniform crossover.
        /// </summary>
        public XoverUniform()
        {
            this.mixProbability = 0.5f;
            ParentsNumber = 2;
            ChildrenNumber = 2;
        }

        /// <summary>
        ///  Cross the specified parents generating the children (Uniform cross).
        /// </summary>
        /// <param name="firstParent"> First parent</param>
        /// <param name="secondParent">Second parent</param>
        /// <returns>The offspring (children) of the parents.</returns>
        private IList<IIndividual> PerfomCross(IIndividual firstParent, IIndividual secondParent)
        {

            var firstChild = firstParent.CreateNew();
            var secondChild = firstParent.CreateNew();

            // uniform cross by pmixProb
            for (int i = 0; i < firstParent.Length; i++)
            {
                if (FastRandom.GetDouble() < mixProbability)
                {
                    firstChild.ReplaceGene(i, firstParent.GetGene(i));
                    secondChild.ReplaceGene(i, secondParent.GetGene(i));
                }
                else
                {
                    firstChild.ReplaceGene(i, secondParent.GetGene(i));
                    secondChild.ReplaceGene(i, firstParent.GetGene(i));
                }
            }
            return new List<IIndividual> { firstChild, secondChild };
        }

        /// <summary>
        /// Cross the specified parents generating the children.
        /// </summary>
        /// <param name="parents">The parents.</param>
        /// <returns>The offspring (children) of the parents.</returns>
        public override IList<IIndividual> Cross(IList<IIndividual> parents)
        {
            // same lenght of parents
            if (parents[0].Length< parents[1].Length)
            {
                return PerfomCross(parents[0], parents[1]);
            }
            else
            {
                return PerfomCross(parents[1], parents[0]);
            }
        }
    }
}
