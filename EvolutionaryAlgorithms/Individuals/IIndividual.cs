using System;

namespace EvolutionaryAlgorithms.Individuals
{
    /// <summary>
    /// Interace for solution reprezentation.
    /// </summary>
    public interface IIndividual:ICloneable
    {
        /// <summary>
        /// Gets or sets the fitness value.
        /// </summary>
        /// <value>The fitness.</value>
        double? Fitness { get; set; }

        /// <summary>
        /// Gets the length of individual.
        /// </summary>
        /// <value>The length.</value>
        int Length { get; set; }


        /// <summary>
        /// Add new gene to genotype.
        /// </summary>
        /// <param name="gene">New gene.</param>
        void AddGene(double gene);

        /// <summary>
        /// Gets the gene in the specified index.
        /// </summary>
        /// <returns>The gene.</returns>
        /// <param name="index">The gene index.</param>
        double GetGene(int index);

        /// <summary>
        /// Gets the genes.
        /// </summary>
        /// <returns>The genes.</returns>
        double[] GetGenes();

        /// <summary>
        /// Generates the gene for the specified index.
        /// </summary>
        /// <returns>The gene.</returns>
        /// <param name="geneIndex">Gene index.</param>
        double GenerateGene(int geneIndex);

        /// <summary>
        /// Replaces the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index to replace.</param>
        /// <param name="gene">The new gene.</param>
        void ReplaceGene(int index, double gene);

        /// <summary>
        /// Replaces the genes starting in the specified index.
        /// </summary>
        /// <remarks>
        /// The genes to be replaced can't be greater than the available space between the start index and the end of the individual.
        /// </remarks>
        /// <param name="startIndex">Start index.</param>
        /// <param name="genes">The genes.</param>
        void ReplaceGenes(int startIndex, double[] genes);


        /// <summary>
        /// Replaces the genes starting in the specified index.
        /// </summary>
        /// <remarks>
        /// The genes to be replaced can't be greater than the available space between the start index and the end of the individual.
        /// </remarks>
        /// <param name="genes">The genes.</param>
        void ReplaceGenes(double[] genes);

        /// <summary>
        /// Creates a new individual using the same structure of this.
        /// </summary>
        /// </summary>
        /// <returns>The new individual.</returns>
        IIndividual CreateNew();

        /// <summary>
        /// Gets the phenotype.
        /// </summary>
        /// <returns>The phenotype (representation).</returns>
        Object[] GetPhenotype();
    }
}
