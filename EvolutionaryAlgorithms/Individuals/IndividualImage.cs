using System.Drawing;

namespace EvolutionaryAlgorithms.Individuals
{
   
    /// <summary>
    ///  Base abstract class represents the image.
    /// </summary>
    public abstract class IndividualImage : Individual
    {
        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="lenght"></param>
        public IndividualImage(int lenght) : base(lenght) { }

        /// <summary>
        /// Creates an image from genetic information.
        /// </summary>
        /// <returns>The bitmap.</returns>
        public abstract Bitmap BuildBitmap();
    }
    
}
