using Accord.Math.Optimization;
using EvolutionaryAlgorithms.Randomization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.Individuals
{
    /// <summary>
    /// Base abstract class for GA-individual.
    /// </summary>
    public abstract class Individual : IIndividual
    {
        /// <summary>
        /// Genes representation (double)
        /// </summary>
        protected double[] genes;

        /// <summary>
        /// genes length
        /// </summary>
        protected int length;

        /// <summary>
        /// max and min. value of gene
        /// </summary>
        protected float minGeneValue = 0;
        protected float maxGeneValue = 255;

        /// <summary>
        /// Gets or sets the fitness of the chromosome in the current problem.
        /// </summary>
        public double? Fitness { get; set; }


        /// <summary>
        /// Gets the length, in genes, of the chromosome.
        /// </summary>
        public int Length { get { return this.length; } set{ this.length = value; } }

        /// <summary>
        /// Initializes a new instance of the IndividualDefault.
        /// </summary>
        /// <param name="length">The length, in genes, of the chromosome.</param>
        protected Individual(int length, int parameterLength = 0)
        {
            this.length = length;
            this.genes = new double[length];
        }

        /// <summary>
        /// Generates the gene for the specified index.
        /// </summary>
        /// <param name="geneIndex">Gene index.</param>
        /// <returns>The gene generated at the specified index.</returns>
        public abstract double GenerateGene(int geneIndex);

        /// <summary>
        /// Creates a new chromosome using the same structure of this.
        /// </summary>
        /// <returns>The new chromosome.</returns>
        public abstract IIndividual CreateNew();

        /// <summary>
        /// Gets the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index.</param>
        /// <returns>
        /// The gene.
        /// </returns>
        public double GetGene(int index)
        {
            return this.genes[index];
        }

        /// <summary>
        /// Gets the genes.
        /// </summary>
        /// <returns>The genes.</returns>
        public double[] GetGenes()
        {
            return this.genes;
        }

        /// <summary>
        /// Replaces the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index to replace.</param>
        /// <param name="gene">The new gene.</param>
        public void ReplaceGene(int index, double gene)
        {
            if (gene > maxGeneValue)
                gene = maxGeneValue;

            if (gene < minGeneValue)
                gene = minGeneValue;

            this.genes[index] = gene;
            Fitness = null;
        }

        /// <summary>
        /// Replaces the genes starting in the specified index.
        /// </summary>
        /// <param name="startIndex">Start index.</param>
        /// <param name="genes">The genes.</param>
        /// <remarks>
        /// The genes to be replaced can't be greater than the available space between the start index and the end of the chromosome.
        /// </remarks>
        public void ReplaceGenes(int startIndex, double[] genes)
        {
            if (genes.Length > 0)
            {
                Array.Copy(genes, 0, this.genes, startIndex, genes.Length);

                Fitness = null;
            }
        }

        /// <summary>
        /// Replaces the genes starting in the specified index.
        /// </summary>
        /// <param name="genes">The genes.</param>
        /// <remarks>
        /// The genes to be replaced can't be greater than the available space between the start index and the end of the chromosome.
        /// </remarks>
        public void ReplaceGenes(double[] genes)
        {
            if (genes.Length > 0)
            {
                for (int i = 0; i < genes.Length; i++)
                {
                    ReplaceGene(i, genes[i]);
                }

                Fitness = null;
            }
        }

        /// <summary>
        /// Created all the genes of an individual.
        /// New genes according to the individual.
        /// </summary>
        protected virtual void CreateGenes()
        {
            for (int i = 0; i < length; i++)
            {
                ReplaceGene(i, GenerateGene(i));
            }
        }

        /// <summary>
        /// Clone individual.
        /// </summary>
        /// <returns>Clone.</returns>
        object ICloneable.Clone()
        {
            var clone = CreateNew();

            clone.ReplaceGenes(0, GetGenes());
            clone.Fitness = Fitness;

            return clone;
        }

        /// <summary>
        /// Gets the phenotype.
        /// </summary>
        /// <returns>The phenotype (representation).</returns>
        public abstract Object[] GetPhenotype();

        /// <summary>
        /// Add new gene to genotype.
        /// </summary>
        /// <param name="gene">New gene.</param>
        public void AddGene(double gene)
        {
            var currLength = Length;
            var genes = this.genes;

            this.genes = new double[currLength + 1];
            Array.Copy(genes, 0, this.genes, 0, currLength);

            this.genes[currLength] = gene;

            this.length = currLength + 1;
        }
    }
}
