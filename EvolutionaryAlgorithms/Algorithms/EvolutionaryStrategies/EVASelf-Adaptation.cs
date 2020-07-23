using System.Collections.Generic;
using System.Threading.Tasks;
using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Populations;
using EvolutionaryAlgorithms.Randomization;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;
using System;

namespace EvolutionaryAlgorithms.Algorithms.EvolutionaryStrategies
{
    public class SelfAdaptation : EVA
    {
        protected float mutationProbability;
        protected float xoverProbability;
        protected int popSize;

        protected IXover xover;
        protected IElite elite;
        protected ISelection selection;
        private readonly Xorshift _rng;

        public SelfAdaptation(IFitness fitness, IPopulation population) : base(fitness, population)
        {
            xover  = new XoverUniform();
            elite = new EliteByFitness(0.1);
            selection = new SelectionTournament();
            termination = new TerminationMaxNumberGeneration();
            termination.InitializeTerminationCondition(15_000);
            mutationProbability = 0.7f;
            xoverProbability = 0.5f;
            _rng = new Xorshift();

        }

        protected override void CreatePopulation()
        {
            popSize = population.Size;

            population.CreatePopulation();

            foreach (var ind in population.Individuals)
            {
                var sigma = Normal.Sample(_rng, 0, 50);

                if (sigma < Double.Epsilon)
                    sigma = Double.Epsilon;


                ind.AddGene(sigma);
            }
        }


        protected override IPopulation EvolvedOneGeneration(IPopulation population)
        {
            // var parents = SelectParents(population);

            var parents = population.Individuals;
            var offspring = Cross(population, parents);
            Mutate(offspring);
            offspring = Reinsert(population, offspring, parents);

            population.Individuals = offspring;

            CurrentGenerationsNumber++;
            
            return population;
        }

        public void Mutate(IList<IIndividual> individuals)
        {
            double?[] fits = new double?[individuals.Count];

            executor.EvaluateFitness(fitness, individuals);
            for (int i = 0; i < individuals.Count; i++)
            {
                fits[i] = individuals[i].Fitness;
            }

            Parallel.ForEach(individuals, ind =>
            {
                Mutate(ind);
            });

            executor.EvaluateFitness(fitness, individuals);

            // double?[] fits = new double?[individuals.Count];
            int c = 0;
            for (int i = 0; i < individuals.Count; i++)
            {
                var f = individuals[i].Fitness;

                if (f <= fits[i])
                    c++;
            }

            // 1/5 

            var success = (double) c / individuals.Count;

            if (success > 0.2)
            {

                foreach (var ind in individuals)
                {

                    var index = ind.Length - 1;
                    var sigma = ind.GetGene(index) + Normal.Sample(_rng, 0, 1);


                    ind.ReplaceGene(index, sigma);
                }
            }
            else if (success < 0.2)
            {

                foreach (var ind in individuals)
                {
                    var index = ind.Length - 1;
                    var sigma = ind.GetGene(index) - Normal.Sample(_rng, 0, 1);


                    if (sigma < Double.Epsilon)
                        sigma = Double.Epsilon;


                    ind.ReplaceGene(index, sigma);
                }
            }
        }

        void Mutate(IIndividual ind)
        {
            // last gene is sigma parameter
            // var sigma = 1;
            var sigma = ind.GetGene(ind.Length - 1);


            for (int index = 0; index < ind.Length - 1; index++)
            {
                if (FastRandom.GetDouble() <= mutationProbability)
                {
                    var g = ind.GetGene(index);

                    var s = Normal.Sample(_rng, 0, sigma);

                    ind.ReplaceGene(index, g + s);
                }
            }

        }


        public IList<IIndividual> Cross(IPopulation population, IList<IIndividual> parents)
        {
            return executor.Cross(population, xover, xoverProbability, parents);
        }

        public IList<IIndividual> Reinsert(IPopulation population, IList<IIndividual> offspring, IList<IIndividual> parents)
        {
            return elite.EliteIndividuals(population.Size, offspring, parents);
        }

        public IList<IIndividual> SelectParents(IPopulation population)
        {
            return selection.SelectIndividuals(population.Size, population);
        }
    }
}
