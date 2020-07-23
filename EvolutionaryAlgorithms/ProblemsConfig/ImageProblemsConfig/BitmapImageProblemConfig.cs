using EvolutionaryAlgorithms.ElitistPrivileges;
using EvolutionaryAlgorithms.Operators.Mutations;
using EvolutionaryAlgorithms.Operators.Xovers;
using EvolutionaryAlgorithms.Selections;
using EvolutionaryAlgorithms.Terminations;
using System;
using System.Collections.Generic;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    /// <summary>
    /// Controller for individual, which genes represent Bitmap image (RGB).    
    /// All usable operators.
    /// </summary>
    class BitmapImageProblemConfig : BaseBitmapImageProblemConfig
    {
        /// <summary>
        /// Initializes possible xovers for this instance.
        /// </summary
        public override void InitializeXovers()
        {
            this.xovers = new Dictionary<string, Type>
            {
                { typeof(XoverUniform).Name, typeof(XoverUniform)},
                { typeof(XoverOnePoint).Name, typeof(XoverOnePoint)},
                { typeof(XoverTwoPoints).Name, typeof(XoverTwoPoints)},
                { typeof(XoverNon).Name, typeof(XoverNon)},
            };
        }

        /// <summary>
        /// Initializes possible elities for this instance.
        /// </summary>
        public override void InitializeElities()
        {
            this.elitizmuses = new Dictionary<string, Type>
            {
                { typeof(EliteByFitness).Name, typeof(EliteByFitness)},
                { typeof(EliteNon).Name, typeof(EliteNon)},
            };
        }

        /// <summary>
        /// Initializes possible mutations for this instance.
        /// </summary>
        public override void InitializeMutations()
        {
            this.mutations = new Dictionary<string, Type>
            {
                { typeof(MutationTwors).Name, typeof(MutationTwors)},
                { typeof(MutationUniform).Name, typeof(MutationUniform)}
            };
        }

        /// <summary>
        /// Initializes possible selections for this instance.
        /// </summary>
        public override void InitializeSelections()
        {
            this.selections = new Dictionary<string, Type>
            {
                { typeof(SelectionElite).Name, typeof(SelectionElite)},
                { typeof(SelectionTournament).Name, typeof(SelectionTournament)},
                { typeof(SelectionNon).Name, typeof(SelectionNon)},
            };
        }

        /// <summary>
        /// Initializes possible terminations for this instance.
        /// </summary>
        public override void InitializeTermination()
        {
            this.terminations = new Dictionary<string, Type>
            {
                { typeof(TerminationMaxTime).Name, typeof(TerminationMaxTime)},
                { typeof(TerminationMaxNumberGeneration).Name, typeof(TerminationMaxNumberGeneration)}
            };
        }
    }
}
