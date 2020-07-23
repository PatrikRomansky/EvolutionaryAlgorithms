using EvolutionaryAlgorithms.Individuals;
using System.Collections.Generic;

namespace EvolutionaryAlgorithms.Operators.Xovers
{
    /// <summary>
    /// The crossover operator that does not make a cross.
    /// </summary>
    class XoverNon : Xover
    {
        /// <summary>
        /// Construtor: The crossover operator that does not make a cross.
        /// </summary>
        public XoverNon()
        {
            ParentsNumber = 2;
            ChildrenNumber = 2;
        }
        /// <summary>
        /// Do nothing.
        /// </summary>
        /// <param name="parents">The parents.</param>
        /// <returns>The offspring (children) of the parents.</returns>
        public override IList<IIndividual> Cross(IList<IIndividual> parents)
        {
            return parents;
        }
    }
}
