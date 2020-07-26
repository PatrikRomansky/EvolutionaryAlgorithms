using Emgu.CV;
using Emgu.CV.Structure;
using EvolutionaryAlgorithms.Randomization;
using System;
using System.Drawing;

namespace EvolutionaryAlgorithms.Individuals
{
    public abstract class IndividualShapes : IndividualImage
    {
        protected int numberOfShapes;
        protected int geneSize;

        /// <summary>
        /// Initializes a shapes.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="init">Initilaization genes.</param>
        public IndividualShapes(int width, int height, int length) : base(width, height, length) 
        {
            Width = width;
            Height = height;
        }

        protected abstract void DrawShape(Image<Bgr, Byte> img, object shape);

        public override Bitmap BuildBitmap()
        {
            var phenotype = GetPhenotype();

            Image<Bgr, Byte> img = new Image<Bgr, Byte>(Width, Height, new Bgr(255, 255, 255));
            foreach (var shape in phenotype)
            {
               DrawShape(img, shape);
            }

            return img.ToBitmap<Bgr, Byte>();
        }

        protected abstract double GetMaxGeneValue(int geneIndex);

        public override double GenerateGene(int geneIndex)
        {

            var maxValue = GetMaxGeneValue(geneIndex);

            return FastRandom.GetDouble() * maxValue;
        }

        /// <summary>
        /// Replaces the gene in the specified index.
        /// </summary>
        /// <param name="index">The gene index to replace.</param>
        /// <param name="gene">The new gene.</param>
        public override void ReplaceGene(int index, double gene)
        {
            var maxGeneValue = GetMaxGeneValue(index);

            if (gene > maxGeneValue)
                gene = maxGeneValue;

            if (gene < minGeneValue)
                gene = minGeneValue;

            this.genes[index] = gene;
            Fitness = null;
        }
    }
}
