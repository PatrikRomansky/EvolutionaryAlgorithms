using EvolutionaryAlgorithms.Individuals;
using System.Collections.Generic;

namespace EvolutionaryAlgorithms.ElitistPrivileges
{
    /// <summary>
    /// The elitizmus operator that does not make a elite selection.
    /// </summary>
    public class EliteNon : IElite
    {
        /// <summary>
        /// Percentage of survivors.
        /// </summary>
        public double ElitePercentage { get; }

        /// <summary>
        /// Construtor: The elitizmus operator that does not make elite slection.
        /// </summary>
        /// <param name="elitePercentage">Percentage of survivors.</param>
        public EliteNon(double elitePercentage = 0)
        {
            ElitePercentage = elitePercentage;
        }

        /// <summary>
        /// DO nothin.
        /// </summary>
        /// <param name="popSize">Population size.</param>
        /// <param name="offspring">New individuals.</param>
        /// <param name="parents">The parents.</param>
        /// <returns>The eite population.</returns>
        public IList<IIndividual> EliteIndividuals(int popSize, IList<IIndividual> offspring, IList<IIndividual> parents)
        {
            return offspring;
        }
    }
}
