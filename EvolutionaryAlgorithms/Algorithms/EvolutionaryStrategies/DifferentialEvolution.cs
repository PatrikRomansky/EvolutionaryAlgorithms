using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Populations;
using EvolutionaryAlgorithms.Randomization;
using EvolutionaryAlgorithms.Terminations;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.Algorithms.EvolutionaryStrategies
{
    public class DifferentialEvolution : EVA
    {

        /// <summary>
        /// Paramater called "Amplification Factor" or "Differential Weight" (0...2) 
        /// </summary>
        public double F { get; set; }

        /// <summary>
        /// Crossover Probability (0...1)
        /// </summary>
        public double XoverProbability { get; set; }


        public DifferentialEvolution(IFitness fitness, IPopulation population ): base(fitness, population)
        {
            this.XoverProbability = 0.9;
            this.F = 0.5;

            termination = new TerminationMaxNumberGeneration();
            termination.InitializeTerminationCondition(5_000);

        }


        protected override IPopulation EvolvedOneGeneration(IPopulation population)
        {

            List<IIndividual> newGeneration = new List<IIndividual>();

            foreach (var orginal in population.Individuals)
            {
                // generate unique random numbers
                var randomValues = FastRandom.GetUniqueInts(3, 0, population.Size);
                int a = randomValues[0];
                int b = randomValues[1];
                int c = randomValues[2];

                // choose random individuals (agents) from population
                IIndividual individual1 = population.Individuals[a];
                IIndividual individual2 = population.Individuals[b];
                IIndividual individual3 = population.Individuals[c];

                int i = 0;


                int R = FastRandom.GetInt(0, population.Size);

                var candidate = population.CreateEmptyIndividual();
                foreach (var orginalElement in orginal.GetGenes())
                {
                    double probXover = FastRandom.GetDouble();

                    if (probXover < XoverProbability || i == R)
                    {
                        // simple mutation
                        double newElement = individual1.GetGene(i) + F * (individual2.GetGene(i) - individual3.GetGene(i));
                        candidate.ReplaceGene(i, newElement);
                    }
                    else
                    {
                        candidate.ReplaceGene(i, orginalElement);
                    }

                    i++;
                }


                var fit = fitness.Evaluate(candidate);

                if (fit < orginal.Fitness)
                    newGeneration.Add(candidate);
                else
                    newGeneration.Add(orginal);

            }

            CurrentGenerationsNumber++;

            population.Individuals = newGeneration;

            return population;
        }
    }
}
