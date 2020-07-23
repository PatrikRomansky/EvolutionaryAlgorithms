using EvolutionaryAlgorithms.Algorithms;

namespace EvolutionaryAlgorithms.Terminations
{
    /// <summary>
    /// Termination condition for genetic algorithm.
    /// </summary>
    public interface ITermination
    {
        /// <summary>
        /// Initialize termination condition.
        /// </summary>
        /// <param name="terminationCondition">Termination limit.</param>
        void InitializeTerminationCondition(object terminationCondition);

        /// <summary>
        /// Determines whether the specified genetic algorithm fulfilled the termination condition.
        /// </summary>
        /// <returns>True if termination has been fulfilled otherwise false.</returns>
        /// <param name="eva">The genetic algorithm.</param>
        bool IsFulfilled(IEVA eva);
    }
}
