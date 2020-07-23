using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Populations;
using EvolutionaryAlgorithms.Randomization;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionaryAlgorithms.Algorithms.Executors
{
    /// <summary>
    /// Linear performs the following operations on the population (evuluate fitness, mutation, crossover)
    /// </summary>
    public class LinearExecutor : IExecutor
    {
        /// <summary>
        /// Linear fitness evaluation.
        /// </summary>
        /// <param name="fitness">Fitness.</param>
        /// <param name="population">Input Population</param>
        public virtual void EvaluateFitness(IFitness fitness, IPopulation population)
        {
            foreach (var ind in population.Individuals)
            {
               //non-fitness value
               if (!ind.Fitness.HasValue)
               {
                    // recalculation fitness for cur. individual
                    ind.Fitness = fitness.Evaluate(ind);
                }
            }

            // sorts population by fitness
          //  population.Individuals = population.Individuals.OrderBy(c => c.Fitness.Value).ToList();
        }


        /// <summary>
        /// Linear fitness evaluation.
        /// </summary>
        /// <param name="fitness">Fitness.</param>
        /// <param name="population">Input Population</param>
        public virtual void EvaluateFitness(IFitness fitness, IList<IIndividual> population)
        {
            foreach (var ind in population)
            {
                //non-fitness value
                if (!ind.Fitness.HasValue)
                {
                    // recalculation fitness for cur. individual
                    ind.Fitness = fitness.Evaluate(ind);
                }
            }

            // sorts population by fitness
            // population.Individuals = population.Individuals.OrderByDescending(c => c.Fitness.Value).ToList();
        }

        /// <summary>
        /// Linear mutaion execution.
        /// </summary>
        /// <param name="mutation">Mutation operator.</param>
        /// <param name="mutationProbability">Probability of mutaiton</param>
        /// <param name="individuals">Individual</param>
        public virtual void Mutate(IMutation mutation, float mutationProbability, IList<IIndividual> individuals)
        {
            for (int i = 0; i < individuals.Count; i++)
            {
                mutation.Mutate(individuals[i], mutationProbability);
            }
        }

        /// <summary>
        /// Linear xover executor.
        /// </summary>
        /// <param name="population">Input population.</param>
        /// <param name="xover">Xover operator.</param>
        /// <param name="xoverProbability">Xover probability</param>
        /// <param name="parents">Parents list</param>
        /// <returns>Childten(individuals)</returns>
        public virtual IList<IIndividual> Cross(IPopulation population, IXover xover, float xoverProbability, IList<IIndividual> parents)
        {
            var size = population.Size;

            var offspring = new List<IIndividual>(size);

            for (int i = 0; i < size; i += xover.ChildrenNumber)
            {
                // selected parents from population
                var selectedParents = parents.Skip(i).Take(xover.ParentsNumber).ToList();

                // If match the probability cross is made, otherwise the offspring is an exact copy of the parents.
                // Checks if the number of selected parents is equal which the crossover expect, because the in the end of the list we can
                // have some rest individual
                if (selectedParents.Count == xover.ParentsNumber && FastRandom.GetDouble() <= xoverProbability)
                {
                    offspring.AddRange(xover.Cross(selectedParents));
                }
                else
                {
                    offspring.AddRange(selectedParents);
                }
            }

            return offspring;

        }
    }
}
