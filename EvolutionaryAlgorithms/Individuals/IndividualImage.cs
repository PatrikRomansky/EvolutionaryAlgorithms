using System.Drawing;

namespace EvolutionaryAlgorithms.Individuals
{
   
    /// <summary>
    ///  Base abstract class represents the image.
    /// </summary>
    public abstract class IndividualImage : Individual
    {
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width { get; protected set; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; protected set; }

        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="lenght"></param>
        public IndividualImage(int width, int height, int lenght) : base(lenght) 
        {
            Width = width;
            Height = height;       
        }

        /// <summary>
        /// Creates an image from genetic information.
        /// </summary>
        /// <returns>The bitmap.</returns>
        public abstract Bitmap BuildBitmap();
    }
    
}
