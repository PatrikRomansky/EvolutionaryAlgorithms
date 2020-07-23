using EvolutionaryAlgorithms.Algorithms;
using EvolutionaryAlgorithms.Algorithms.Executors;
using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using System;

namespace EvolutionaryAlgorithms.ProblemsConfig
{
    /// <summary>
    /// Interface for setting the properties of solved problems.
    /// </summary>
    public interface IProblemConfig
    {
        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="target">Problem target</param>
        /// <param name="path">File path (target file)</param>
        void Initialize(Object target, string path);

        /// <summary>
        /// Initialize this instance.
        /// </summary>
        /// <param name="targetInpuFile">File path (target file)</param>
        void Initialize(String targetInpuFile);
        
        /// <summary>
        /// Initialize this instance operator components.
        /// </summary>
        /// <param name="ID">Instacen ID</param>
        void Initialize(int ID);

        /// <summary>
        /// Initializes possible elities for this instance.
        /// </summary>
        void InitializeElities();

        /// <summary>
        /// Initializes possible xoversfor this instance.
        /// </summary>
        void InitializeXovers();

        /// <summary>
        /// Initializes possible mutations for this instance.
        /// </summary>
        void InitializeMutations();

        /// <summary>
        /// Initializes possible selections for this instance.
        /// </summary>
        void InitializeSelections();

        /// <summary>
        /// Initializes possible terminations for this instance.
        /// </summary>
        void InitializeTermination();


        /// <summary>
        /// Configure the Genetic Algorithm.
        /// </summary>
        /// <param name="ga">The genetic algorithm.</param>
        void ConfigGATermination(IEVA ga);

        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        IIndividual CreateIndividual();

        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        IIndividual CreateEmptyIndividual();

        /// <summary>
        /// Draws the sample.
        /// </summary>
        /// <param name="bestIndividual">The current best individual.</param>
        /// <param name="logRate">Logger rate.</param>
        /// <returns>Best ind if exist, else null.</returns>
        Object ShowBestIndividual(IIndividual bestIndividual, int logRate);

        /// <summary>
        /// Creates the fitness.
        /// </summary>
        /// <returns>The fitness.</returns>
        IFitness CreateFitness();

        /// <summary>
        /// Creates the termination.
        /// </summary>
        /// <param name="name">Termination name</param>
        /// <param name="param"></param>
        /// <returns>The termination</returns>
        ITermination CreateTermination(string name, object param);

        /// <summary>
        /// Creates the xover.
        /// </summary>
        /// <param name="name">Xover name.</param>
        /// <returns>The xover</returns>
        IXover CreateXover(string name);

        /// <summary>
        /// Creates the mutation.
        /// </summary>
        /// <param name="name">Mutation name.</param>
        /// <returns>The mutation.</returns>
        IMutation CreateMutation(string name);

        /// <summary>
        /// Creates the selection.
        /// </summary>
        /// <param name="name">Selection name.</param>
        /// <returns>The selection.</returns>
        ISelection CreateSelection(string name);

        /// <summary>
        /// Creates the elite.
        /// </summary>
        /// <param name="name">Elite name.</param>
        /// <returns>The elite.</returns>
        IElite CreateElite(string name, double perc);

        /// <summary>
        ///  Create the executor.
        /// </summary>
        /// <returns>The executor.</returns>
        IExecutor CreateExecutor();

        /// <summary>
        /// Gets possible elities for this instance.
        /// </summary>
        /// <returns>Possible elities.</returns>
        string[] PossibleElities();

        /// <summary>
        /// Gets possible xover for this instance.
        /// </summary>
        /// <returns>Possible xovers.</returns>
        string[] PossibleXovers();

        /// <summary>
        /// Gets possible mutations for this instance.
        /// </summary>
        /// <returns>Possible mutations.</returns>
        string[] PossibleMutations();

        /// <summary>
        /// Gets possible selections for this instance.
        /// </summary>
        /// <returns>Possible selections.</returns>
        string[] PossibleSelections();

        /// <summary>
        /// Gets possible terminations for this instance.
        /// </summary>
        /// <returns>Posiible terminations.</returns>
        string[] PossibleTerminations();

        /// <summary>
        /// Set generation info into log.
        /// </summary>
        /// <param name="fitness">Current fitness.</param>
        /// <param name="speed">current speed.</param>
        /// <param name="all">Current allInfo.</param>
        void SetGenerationInfo(string fitness, string speed, string all);
    }
}
