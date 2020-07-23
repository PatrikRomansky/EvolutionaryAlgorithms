using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Populations;
using EvolutionaryAlgorithms.Terminations;
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using EvolutionaryAlgorithms.Algorithms.EvolutionaryStrategies.CMA_ES;
using EvolutionaryAlgorithms.Individuals;

namespace EvolutionaryAlgorithms.Algorithms.EvolutionaryStrategies
{
    /// <summary>
    /// CMAES
    /// </summary>
    public class EVACMA_ES : EVA
    {
        protected CMA cma;

        IIndividual initialInd;

        public EVACMA_ES(IFitness fitness, IPopulation population) : base(fitness, population)
        {
            termination = new TerminationMaxNumberGeneration();
            termination.InitializeTerminationCondition(2_00);
            // initialInd = population.CreateIndividual();
        }


        protected override void CreatePopulation()
        {
            population.Individuals = new List<IIndividual>();
            initialInd = population.CreateIndividual();
            population.Individuals.Add(initialInd);

            cma = new CMA(initialInd, 1.5);
        }


        protected override IPopulation EvolvedOneGeneration(IPopulation population)
        {
            List<Tuple<Vector<double>,double>> solutions = new List<Tuple<Vector<double>, double>>();
            for (int i = 0; i < cma.PopSize; i++)
            {
                var x = cma.Ask();

                var currInd = population.CreateEmptyIndividual();
                currInd.ReplaceGenes(x.ToArray());

                var value = 1 / fitness.Evaluate(currInd);
                solutions.Add(new Tuple<Vector<double>, double>(x, value));

            }
            cma.Tell(solutions);

            double yCurrentBest = solutions.Min(x => x.Item2);
            var xCurrentBest = solutions.Where(x => x.Item2 == yCurrentBest).FirstOrDefault().Item1;


            IIndividual bestInd = population.CreateEmptyIndividual();
            bestInd.ReplaceGenes(xCurrentBest.ToArray());
            fitness.Evaluate(bestInd);
            BestIndividual = bestInd;

            // Console.WriteLine(j + " - " + yCurrentBest);
            CurrentGenerationsNumber++;
            population.Individuals = new List<IIndividual>();

            population.Individuals.Add(bestInd);

            return population;
        }      
    }
}
