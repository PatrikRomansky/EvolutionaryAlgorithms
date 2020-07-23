using EvolutionaryAlgorithms.Individuals;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvolutionaryAlgorithms.ImageProcessing
{
    /// <summary>
    /// Class for spliting image to blocks and concate them.
    /// </summary>
    public class Split_Concate
    {
        /// <summary>
        /// Concate img from input inds(blocks).
        /// </summary>
        /// <param name="inds">Input imd(block).</param>
        /// <param name="blockSize">Block size.</param>
        /// <param name="width">New image width.</param>
        /// <param name="height">New image height.</param>
        /// <returns>Bitmap(width, height)</returns>
        public static Bitmap ConcateImgs(IIndividual[] inds, int blockSize, int width, int height)
        {
            var result = new Bitmap(width, height);
            int column = width / blockSize;
            int row = height / blockSize;

            /*
             * _____________________
             * |  1  |  2   |  3   |
             * |_____|______|______|
             * |  4  |  5   |  6   |
             * |_____|______|______|
             * |  7  |  8   |  9   |
             * |_____|______|______|
             * |  10 |  11  |  12  |
             * |_____|______|______|
             * 
             * Each ind represented one block.
             * The order is line by line.
             */
            int i = 0;

            // for each block
            for (int blockY = 0; blockY < row; blockY++)
            {
                for (int blockX = 0; blockX < column; blockX++)
                {
                    // ind to Bitmap
                    var ind = inds[i++] as IndividualImage;
                    var bitmap = ind.BuildBitmap();

                    // Sets ind as Block
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            result.SetPixel(blockX * blockSize + x, blockY * blockSize + y, bitmap.GetPixel(x, y));

                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Splits image to small block.
        /// </summary>
        /// <param name="inputBitmap">Input image.</param>
        /// <param name="blockSize">Block size</param>
        /// <returns>Blocks.</returns>
        public static Bitmap[] SplitImg(Bitmap inputBitmap, int blockSize)
        {
            /*
            * _____________________
            * |  1  |  2   |  3   |
            * |_____|______|______|
            * |  4  |  5   |  6   |
            * |_____|______|______|
            * |  7  |  8   |  9   |
            * |_____|______|______|
            * |  10 |  11  |  12  |
            * |_____|______|______|
            * 
            * Each ind represented one block.
            */
            int column = inputBitmap.Width / blockSize;
            int row = inputBitmap.Height / blockSize;

            var result = new Bitmap[row * column];

            int i = 0;
            System.Drawing.Imaging.PixelFormat format = inputBitmap.PixelFormat;
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < column; x++)
                {
                    RectangleF cloneRect = new RectangleF(x * blockSize, y * blockSize, blockSize, blockSize);
                    result[i++] = inputBitmap.Clone(cloneRect, format);
                }
            }

            return result;
        }
    }
}
