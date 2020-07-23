using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.ProblemsConfig.ImageProblemsConfig
{
    /// <summary>
    /// Interface for setting the properties of solved image problems.
    /// </summary>
    public interface IImageProblemConfig : IProblemConfig
    {
        /// <summary>
        /// Resize image.
        /// </summary>
        /// <param name="scale">Scale percentage.</param>
        void InitializeScale(float scale);
    }
}
