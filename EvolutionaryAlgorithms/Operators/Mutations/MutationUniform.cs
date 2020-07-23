using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Randomization;

namespace EvolutionaryAlgorithms.Operators.Mutations
{
    /// <summary>
    /// unifrom mutation operator.
    /// </summary>
    public class MutationUniform : Mutation
    {
        // <summary>
        /// Mutate the specified individual.
        /// </summary>
        /// <param name="individual">The individual to be mutated.</param>
        /// <param name="mutation_probabilty">The mutation probability to mutate each individual.</param>
        public override void Mutate(IIndividual individual, float mutation_probabilty)
        {
            for (int index = 0; index < individual.Length; index++)
            {
                if (FastRandom.GetDouble() <= mutation_probabilty)
                {
                    individual.ReplaceGene(index, individual.GenerateGene(index));
                }
            }
        }
    }
}
