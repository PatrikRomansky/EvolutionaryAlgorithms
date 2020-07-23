using EvolutionaryAlgorithms.Algorithms;
using System;

namespace EvolutionaryAlgorithms.Terminations
{
    /// <summary>
    /// Termination condition for genetic algorithm.
    /// The condition is the maximum time.
    /// </summary>
    class TerminationMaxTime : ITermination
    {
        /// <summary>
        /// max. time
        /// </summary>
        private TimeSpan maxTime;

        /// <summary>
        /// Initialize instence of Termination maximum time.
        /// Default value of max. time = 60s.
        /// </summary>
        public TerminationMaxTime()
        {
            this.maxTime = new TimeSpan(0, 0, 60);
        }

        /// <summary>
        /// Initialize termination condition.
        /// </summary>
        /// <param name="terminationCondition">Termination limit.</param>
        public void InitializeTerminationCondition(object termination)
        {
            var sec = (int)termination;
            maxTime = new TimeSpan(0, 0, sec);
        }

        /// <summary>
        /// Determines whether the specified genetic algorithm fulfilled the termination condition.
        /// </summary>
        /// <param name="eva">The genetic algorithm.</param>
        /// <returns>True if termination has been fulfilled, otherwise false.</returns>
        public bool IsFulfilled(IEVA eva)
        {
            if (eva.TimeEvolving < maxTime)
            {
                return false;
            }

            return true;
        }
    }
}
