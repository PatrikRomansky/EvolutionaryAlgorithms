using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Populations;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionaryAlgorithms.Selections
{
    /// <summary>
    /// The selection by fitness.
    /// </summary>
    public class SelectionElite : ISelection
    {

        /// <summary>
        /// Selects the first n-individuals of a generation by fitness.
        /// </summary>
        /// <param name="number">Number of selected.</param>
        /// <param name="generation">Cur. generation</param>
        /// <returns>Selected individuals.</returns>
        public IList<IIndividual> SelectIndividuals(int number, IPopulation generation)
        {
            var orderedIndividuals = generation.Individuals.OrderByDescending(c => c.Fitness);

            return orderedIndividuals.Take(number).ToList();
        }
    }
}
