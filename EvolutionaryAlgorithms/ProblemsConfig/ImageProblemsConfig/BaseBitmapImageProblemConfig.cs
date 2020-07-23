using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Fitnesses;
using EvolutionaryAlgorithms.ImageProcessing;
using EvolutionaryAlgorithms.Individuals;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    /// <summary>
    /// Controller for individual, which genes represent Bitmap image (RGB).    
    /// Basic operators selected on the basis of previous experiments.
    /// </summary>
    public class BaseBitmapImageProblemConfig : ImageProblemConfig
    {

        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateIndividual()
        {
            return new IndividualBitmap(width, height) ;
        }


        /// <summary>
        /// Creates the individual.
        /// </summary>
        /// <returns>The individual.</returns>
        public override IIndividual CreateEmptyIndividual()
        {
            return new IndividualBitmap(width, height, false);
        }


        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize(string targetInputfileName)
        {
            var targetBitmap = this.InititializeSame(targetInputfileName);
            /// var targetBitmap = img
            var fit = new FitnessBitmap(targetBitmap);
            fitness = fit;

            base.Initialize(targetInputfileName);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public override void Initialize(object target, string targetInputfileName)
        {
            var targetBitmap = target as Bitmap;
            width = targetBitmap.Width;
            height = targetBitmap.Height;
            rawWidth = width;
            rawHeight = height;
            

            /// var targetBitmap = img
            var fit = new FitnessBitmap(targetBitmap);
            fitness = fit;

            base.Initialize(targetInputfileName);
        }

        /// <summary>
        /// Initializes possible xoversfor this instance.
        /// </summary>
        public override void InitializeXovers()
        {
            this.xovers = new Dictionary<string, Type>
            {
               {typeof(XoverUniform).Name, typeof(XoverUniform)}
            };
        }

        /// <summary>
        /// Initializes possible elities for this instance.
        /// </summary>
        public override void InitializeElities()
        {
            this.elitizmuses = new Dictionary<string, Type>
            {
                { typeof(EliteByFitness).Name, typeof(EliteByFitness)}
            };
        }

        /// <summary>
        /// Initializes possible mutations for this instance.
        /// </summary>
        public override void InitializeMutations()
        {
            this.mutations = new Dictionary<string, Type>
            {
                {typeof(MutationTwors).Name, typeof(MutationTwors)}
            };
        }

        /// <summary>
        /// Initializes possible selections for this instance.
        /// </summary>
        public override void InitializeSelections()
        {
            this.selections = new Dictionary<string, Type>
            {
                {typeof(SelectionTournament).Name, typeof(SelectionTournament)}
            };
        }

        /// <summary>
        /// Initializes possible terminations for this instance.
        /// </summary>
        public override void InitializeTermination()
        {
            this.terminations = new Dictionary<string, Type>
            {
                {typeof(TerminationMaxNumberGeneration).Name, typeof(TerminationMaxNumberGeneration)}
            };
        }
    }
}
