using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EvolutionaryAlgorithms.Populations
{
    /// <summary>
    /// Population fo EVA.
    /// </summary>
    public class Population : IPopulation
    {
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int Size { get; set; }

        /// <summary>
        /// Create individual function.
        /// </summary>
        public Func<IIndividual> CreateIndividual { get; private set; }

        /// <summary>
        /// Create individual function.
        /// </summary>
        public Func<IIndividual> CreateEmptyIndividual { get; private set; }

        /// <summary>
        /// Gets the iindividuals.
        /// </summary>
        /// <value>The individuals.</value>
        public IList<IIndividual> Individuals { get; set; }


        /// <summary>
        /// Constructor for problem population.
        /// </summary>
        /// <param name="size">popoluation size,</param>
        /// <param name="createIndividual">Create individual function.</param>
        public Population(int size, Func<IIndividual> createIndividual, Func<IIndividual> creeteEmptyIndividual)
        {
            Size = size;
            CreateIndividual = createIndividual;
            CreateEmptyIndividual = creeteEmptyIndividual;
        }

        /// <summary>
        /// Inititialize first population.
        /// </summary>
        public virtual void CreatePopulation()
        {
            Individuals = new List<IIndividual>();

            // random population
            for (int i = 0; i < Size; i++)
            {
                var c = CreateIndividual();
                Individuals.Add(c);
            }
        }

        /// <summary>
        /// Gets the best individual in population
        /// </summary>
        /// <returns>Best individividual.</returns>
        public IIndividual GetBestIndividual()
        {
            Individuals = Individuals.OrderByDescending(c => c.Fitness.Value).ToList();
            return Individuals.First();
        }
    }
}
