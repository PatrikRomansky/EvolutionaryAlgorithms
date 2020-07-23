using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Randomization;

namespace EvolutionaryAlgorithms.Operators.Mutations
{
    public class MutationShift: Mutation
    {
        // max line-shift
        int shift;
        int mutateRate;


        /// <summary>
        /// Constructor: Line mutation.
        /// </summary>
        public MutationShift()
        {
            this.shift = 5;
            this.mutateRate = 1;
        }

        /// <summary>
        /// Mutate the specified individual.
        /// Shifted line
        /// </summary>
        /// <param name="individual">The individual.</param>
        /// <param name="mutation_probabilty">The probability to mutate each indiviudal.</param>
        public override void Mutate(IIndividual individual, float mutation_probabilty)
        {
            var indexes = FastRandom.GetInts(mutateRate, 0, individual.Length);
            foreach (var index in indexes)
            {
                var oldGene = individual.GetGene(index);

                // line endPoints shift
                var currShift = FastRandom.GetInt(-shift, shift);


                // sets new gene
                individual.ReplaceGene(index, oldGene + currShift);
            }
        }
    }
}
