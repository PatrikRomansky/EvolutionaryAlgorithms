using EvolutionaryAlgorithms.Individuals;
using System.Collections.Generic;

namespace EvolutionaryAlgorithms.ElitistPrivileges
{
    /// <summary>
    /// Interface for elitizmus genetic operator.
    /// Reinsert the number of individuals from the previous generation.
    /// </summary>
    public interface IElite
    {
        /// <summary>
        /// Percentage of survivors.
        /// </summary>
        double ElitePercentage { get; }

        /// <summary>
        /// Reinsert the number of individuals from the previous generation.
        /// </summary>
        /// <param name="popSize">Population size.</param>
        /// <param name="offspring">New individuals.</param>
        /// <param name="parents">The Parents.</param>
        /// <returns>The elite population.</returns>
        IList<IIndividual> EliteIndividuals(int popSize, IList<IIndividual> offspring, IList<IIndividual> parents);
    }
}
