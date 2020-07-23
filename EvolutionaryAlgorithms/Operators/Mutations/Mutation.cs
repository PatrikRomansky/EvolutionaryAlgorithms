using EvolutionaryAlgorithms.Individuals;

namespace EvolutionaryAlgorithms.Operators.Mutations
{
    /// <summary>
    /// Base mutation abstract class.
    /// </summary>
    public abstract class Mutation : IMutation
    {

        /// <summary>
        /// Mutate the specified individual.
        /// </summary>
        /// <param name="individual">The individual.</param>
        /// <param name="mutation_probabilty">The probability to mutate each indiviudal.</param>
        public abstract void Mutate(IIndividual individual, float mutation_probabilty);
    }
}
