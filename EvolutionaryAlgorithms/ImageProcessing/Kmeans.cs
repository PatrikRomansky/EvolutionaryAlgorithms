using Accord.Imaging.Converters;
using Accord.MachineLearning;
using Accord.Math.Distances;
using System.Collections.Generic;
using System.Drawing;

namespace EvolutionaryAlgorithms.ImageProcessing
{
    /// <summary>
    /// Kmeans dominante-color algorithm.
    /// </summary>
    public class Kmeans
    {
        /// <summary>
        /// Image Kmeans.
        /// </summary>
        /// <param name="image">Input Image</param>
        /// <param name="k">Number of colors</param>
        /// <returns>K dominante colors</returns>
        public static Color[] GetDominanteColors(Bitmap image, int k)
        {
            // Create converters
            ImageToArray imageToArray = new ImageToArray(min: -1, max: +1);
            ArrayToImage arrayToImage = new ArrayToImage(1, k, min: -1, max: +1);

            // Transform the image into an array of pixel values
            double[][] pixels; imageToArray.Convert(image, out pixels);

            //  Create a K-Means algorithm using given k and a
            //  square Euclidean distance as distance metric.
            KMeans kmeans = new KMeans(k, new SquareEuclidean())
            {
                Tolerance = 0.05
            };
            // Compute the K-Means algorithm until the difference in
            //  cluster centroids between two iterations is below 0.05
            kmeans.Learn(pixels);

            var controids = kmeans.Clusters.Centroids;

            Bitmap controidsColors; arrayToImage.Convert(controids, out controidsColors);
            List<Color> results = new List<Color>();

            for (var i = 0; i < k; i++)
            {
                Color colorPx = controidsColors.GetPixel(0, i);
                results.Add(colorPx);
            }

            return results.ToArray();
        }
    }
}
