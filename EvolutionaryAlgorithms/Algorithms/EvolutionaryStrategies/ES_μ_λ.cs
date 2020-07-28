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
    public class ES_μ_λ : EVA
    {
        protected float mutationProbability;
        protected float xoverProbability;
        protected int popSize;

        protected IXover xover;
        protected IElite elite;
        protected ISelection selection;
        private readonly Xorshift _rng;

        /// <summary>
        /// Evolutionary strategy
        /// </summary>
        /// <param name="fitness">fit. func.</param>
        /// <param name="population">init pop</param>
        public ES_μ_λ(IFitness fitness, IPopulation population) : base(fitness, population)
        {
            xover  = new XoverUniform();
            elite = new EliteByFitness(0.1);
            selection = new SelectionTournament();
            termination = new TerminationMaxNumberGeneration();
            termination.InitializeTerminationCondition(5_000);
            mutationProbability = 0.7f;
            xoverProbability = 0.5f;
            _rng = new Xorshift();

        }

        /// <summary>
        /// Init population
        /// </summary>
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

        /// <summary>
        /// Mutate population
        /// </summary>
        /// <param name="individuals">List of curr indivual</param>
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

            // 1/5 rule

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

        /// <summary>
        /// Mutate one ind.
        /// </summary>
        /// <param name="ind">Individual</param>
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


        /// <summary>
        /// Crossover
        /// </summary>
        /// <param name="population">Pop</param>
        /// <param name="parents">ist of parents</param>
        /// <returns>list of children</returns>
        public IList<IIndividual> Cross(IPopulation population, IList<IIndividual> parents)
        {
            return executor.Cross(population, xover, xoverProbability, parents);
        }

        /// <summary>
        /// Reisert old ind to new gen.
        /// </summary>
        /// <param name="population">Population info</param>
        /// <param name="offspring">New gen.</param>
        /// <param name="parents">Parents</param>
        /// <returns></returns>
        public IList<IIndividual> Reinsert(IPopulation population, IList<IIndividual> offspring, IList<IIndividual> parents)
        {
            return elite.EliteIndividuals(population.Size, offspring, parents);
        }

        /// <summary>
        /// Selection parents
        /// </summary>
        /// <param name="population"></param>
        /// <returns>Ind for xover</returns>
        public IList<IIndividual> SelectParents(IPopulation population)
        {
            return selection.SelectIndividuals(population.Size, population);
        }
    }
}
