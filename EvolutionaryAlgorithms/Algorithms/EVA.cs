using EvolutionaryAlgorithms.Algorithms.EvolutionaryStrategies;
using EvolutionaryAlgorithms.Algorithms.Executors;
using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Populations;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using System;
using System.Diagnostics;

namespace EvolutionaryAlgorithms.Algorithms
{
    public abstract class EVA : IEVA
    {
        protected IFitness fitness;
        protected IPopulation population;
        protected IExecutor executor;
        protected ITermination termination;
        Stopwatch stopwatch;


        public int CurrentGenerationsNumber { get; protected set; }

        public EVA(IFitness fitness, IPopulation population)
        {
            this.fitness = fitness;
            this.population = population;

            TimeEvolving = TimeSpan.Zero;
            executor = new LinearExecutor();
            CurrentGenerationsNumber = 1;
        }


        public IIndividual BestIndividual { get; protected set; }

        /// <summary>
        /// Evaluates the fitness.
        /// </summary>
        public void EvaluateFitness()
        {
            executor.EvaluateFitness(fitness, population);
        }

        /// <summary>
        /// Gets the time evolving.
        /// </summary>
        public TimeSpan TimeEvolving { get; protected set; }

        public event EventHandler CurrentGenerationInfo;
        public event EventHandler TerminationReached;

        public void EvolveCurrentGeneration()
        {
            EvaluateFitness();

            BestIndividual = population.GetBestIndividual();
            HandlerInvoke(CurrentGenerationInfo);

            if(termination.IsFulfilled(this))
            {
                HandlerInvoke(TerminationReached);
                terminationConditionReached = true;
            }

            population = EvolvedOneGeneration(population);
        }

        protected abstract IPopulation EvolvedOneGeneration(IPopulation population);

        public void HandlerInvoke(EventHandler handler)
        {
            handler?.Invoke(this, EventArgs.Empty);
        }

        // termination algorithm
        protected volatile bool terminationConditionReached = false;

        protected virtual void CreatePopulation()
        {
            population.CreatePopulation();
        }

        public virtual void Run()
        {
            stopwatch = Stopwatch.StartNew(); 
            CreatePopulation();      
            stopwatch.Stop();
            TimeEvolving = stopwatch.Elapsed;

            do
            {
                stopwatch.Restart();
                EvolveCurrentGeneration();
                stopwatch.Stop();
                TimeEvolving += stopwatch.Elapsed;
            }
            while (!terminationConditionReached);
        }

        public void Stop()
        {
            terminationConditionReached = true;
            stopwatch.Stop();
        }
    }
}
