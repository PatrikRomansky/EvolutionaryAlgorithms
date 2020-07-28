using MathNet.Numerics.Random;
using System;
using System.Linq;
using System.Threading;

namespace EvolutionaryAlgorithms.Randomization
{
	/// <summary>
	///  Instence for faster random.
	/// </summary>
	public class FastRandom
    {
		/// <summary>
		/// seed 161
		/// </summary>
		private static readonly Xorshift _globalRandom = new Xorshift(161);
		private static readonly object _globalLock = new object();

		/// <summary> 
		/// Creates a new instance of FastRandom. The seed is derived 
		/// from a global (static) instance of Random, rather 
		/// than time. 
		/// </summary> 
		private static Xorshift NewRandom()
		{
			lock (_globalLock)
			{
				return new Xorshift(_globalRandom.Next(0, int.MaxValue));
			}
		}

		/// <summary> 
		/// Random number generator 
		/// </summary> 
		private static readonly ThreadLocal<Xorshift> _threadRandom = new ThreadLocal<Xorshift>(NewRandom);

		/// <summary> 
		/// Returns an instance of Random which can be used freely 
		/// within the current thread. 
		/// </summary> 
		public static Xorshift Instance { get { return _threadRandom.Value; } }


		/// <summary>
		/// Gets an integer value between minimum value (inclusive) and maximum value (exclusive).
		/// </summary>
		/// <returns>The integer.</returns>
		/// <param name="min">Minimum value (inclusive).</param>
		/// <param name="max">Maximum value (exclusive).</param>
		public static int GetInt(int min, int max)
		{
			return Instance.Next(min, max);
		}

		/// <summary>
		/// Gets a float value between 0.0 and 1.0.
		/// </summary>
		/// <returns>
		/// The float value.
		/// </returns>
		public static float GetFloat()
		{
			return (float)Instance.NextDouble();
		}

		/// <summary>
		/// Gets a double value between 0.0 and 1.0.
		/// </summary>
		/// <returns>The double value.</returns>
		public static double GetDouble()
		{
			return Instance.NextDouble();
		}

		/// <summary>
		/// Gets an integer array with values between minimum value (inclusive) and maximum value (exclusive).
		/// </summary>
		/// <returns>The integer array.</returns>
		/// <param name="length">The array length</param>
		/// <param name="min">Minimum value (inclusive).</param>
		/// <param name="max">Maximum value (exclusive).</param>
		public static int[] GetInts(int length, int min, int max)
		{
			var ints = new int[length];

			for (int i = 0; i < length; i++)
			{
				ints[i] = GetInt(min, max);
			}

			return ints;
		}

		/// <summary>
		/// Gets an integer array with unique values between minimum value (inclusive) and maximum value (exclusive).
		/// </summary>
		/// <returns>The integer array.</returns>
		/// <param name="length">The array length</param>
		/// <param name="min">Minimum value (inclusive).</param>
		/// <param name="max">Maximum value (exclusive).</param>
		public static int[] GetUniqueInts(int length, int min, int max)
		{
			var diff = max - min;

			if (diff < length)
			{
				var msg = String.Format("The length is {0}, but the possible unique values between {1} (inclusive) and {2} (exclusive) are {3}.", length, min, max, diff);
				throw new ArgumentOutOfRangeException(nameof(length), msg);
			}

			var orderedValues = Enumerable.Range(min, diff).ToList();
			var ints = new int[length];

			for (int i = 0; i < length; i++)
			{
				var removeIndex = GetInt(0, orderedValues.Count);
				ints[i] = orderedValues[removeIndex];
				orderedValues.RemoveAt(removeIndex);
			}

			return ints;
		}

		/// <summary>
		/// Gets a float value between minimum value (inclusive) and maximum value (exclusive).
		/// </summary>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Max value.</param>
		/// <returns>
		/// The float value.
		/// </returns>
		public static float GetFloat(float min, float max)
		{
			return min + ((max - min) * GetFloat());
		}

		/// <summary>
		/// Gets a double value between minimum value (inclusive) and maximum value (exclusive).
		/// </summary>
		/// <returns>The double value.</returns>
		/// <param name="min">Minimum value.</param>
		/// <param name="max">Max value.</param>
		public static double GetDouble(double min, double max)
		{
			return min + ((max - min) * GetDouble());
		}
	}
}
