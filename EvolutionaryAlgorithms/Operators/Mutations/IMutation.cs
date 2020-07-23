using EvolutionaryAlgorithms.Individuals;

namespace EvolutionaryAlgorithms.Operators.Mutations
{
    /// <summary>
    /// Defines an interface for mutation function.
    /// Mutation is a genetic operator used to maintain genetic diversity.
    /// </summary>
    public interface IMutation
    {
        /// <summary>
        /// Mutate the specified individual in population.
        /// </summary>
        /// <param name="individual">The individual to be mutated.</param>
        /// <param name="mut_probability">The mutation probability to mutate each individual.</param>
        void Mutate(IIndividual individual, float mut_probability);
    }
}
