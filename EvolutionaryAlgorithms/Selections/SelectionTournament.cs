using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Populations;
using EvolutionaryAlgorithms.Randomization;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionaryAlgorithms.Selections
{
    /// <summary>
    /// The operator of the selection that performs the tournament.
    /// </summary>
    class SelectionTournament : ISelection
    {
        /// <summary>
        /// Selects the number of individuals from the generation.
        /// </summary>
        /// <param name="number">Number of selected.</param>
        /// <param name="generation">Cur. generation</param>
        /// <returns>Selected individuals.</returns>
        public IList<IIndividual> SelectIndividuals(int number, IPopulation generation)
        {
            // previous generation
            var candidates = generation.Individuals.ToList();

            // new indiviudals
            var selected = new List<IIndividual>();

            // determine the winner  by fitness
            while (selected.Count < number)
            {
                var randomIndexes = FastRandom.GetUniqueInts(2, 0, candidates.Count);
                var tournamentWinner = candidates.Where((c, i) => randomIndexes.Contains(i)).OrderByDescending(c => c.Fitness).First();

                selected.Add(tournamentWinner.Clone() as IIndividual);

            }

            return selected;
        }
    }
}
