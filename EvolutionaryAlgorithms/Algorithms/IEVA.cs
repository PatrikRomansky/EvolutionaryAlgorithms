using EvolutionaryAlgorithms.Individuals;
using System;

namespace EvolutionaryAlgorithms.Algorithms
{
    /// <summary>
    /// Inteface for evolutionary algorithm.
    /// </summary>
    public interface IEVA
    {
        /// <summary>
        /// Occurs when generation ran.
        /// </summary>
        event EventHandler CurrentGenerationInfo;

        /// <summary>
        /// Occurs when termination reached.
        /// </summary>
        event EventHandler TerminationReached;

        /// <summary>
        /// Event handler invoke.
        /// </summary>
        /// <param name="handler">Handler.</param>
        void HandlerInvoke(EventHandler handler);

        /// <summary>
        /// Gets the generations number.
        /// </summary>
        /// <value>The generations number.</value>
        int CurrentGenerationsNumber { get; }

        /// <summary>
        /// Gets the best individual.
        /// </summary>
        /// <value>The best individual.</value>
        IIndividual BestIndividual { get; }

        /// <summary>
        /// Gets the time evolving.
        /// </summary>
        /// <value>The time evolving.</value>
        TimeSpan TimeEvolving { get; }

        /// <summary>
        /// Run GA.
        /// </summary>
        void Run();

        /// <summary>
        /// Stop GA(evolution);
        /// </summary>
        void Stop();

        /// <summary>
        /// The course of one generation of evolutionary algorithm.
        /// </summary>
        void EvolveCurrentGeneration();

        /// <summary>
        /// Evaluation of individuals in the population.
        /// </summary>
        void EvaluateFitness();
    }
}
