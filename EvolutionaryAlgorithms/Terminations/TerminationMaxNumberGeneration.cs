using EvolutionaryAlgorithms.Algorithms;

namespace EvolutionaryAlgorithms.Terminations
{
    /// <summary>
    /// Termination condition for genetic algorithm.
    /// The condition is the maximum number of generations
    /// </summary>
    public class TerminationMaxNumberGeneration : ITermination
    {
        /// <summary>
        /// maximum generation
        /// </summary>
        private int expectedMaxGeneration;

        /// <summary>
        /// Initialize instence of Termination maximum generation.
        /// Default value of max. gen. = 1000.
        /// </summary>
        public TerminationMaxNumberGeneration()
        {
            expectedMaxGeneration = 1000;
        }


        /// <summary>
        /// Initialize termination condition.
        /// </summary>
        /// <param name="terminationCondition">Termination limit.</param>
        public void InitializeTerminationCondition(object termination)
        {
            expectedMaxGeneration = (int)termination;
        }

        /// <summary>
        /// Determines whether the specified genetic algorithm fulfilled the termination condition.
        /// </summary>
        /// <param name="eva">The genetic algorithm.</param>
        /// <returns>True if termination has been fulfilled, otherwise false.</returns>
        public bool IsFulfilled(IEVA eva)
        {
            if (eva.CurrentGenerationsNumber < expectedMaxGeneration)
            {
                return false;
            }

            return true;
        }
    }
}
