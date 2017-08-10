using System;
using System.Security.Cryptography;

namespace Engine
{
    public static class RandomGenerator
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        /// <summary>
        /// Returns a random number between 0.0 and 1.0 (both inclusive).
        /// </summary>
        public static double NextDouble()
        {

            byte[] bytes = new byte[4];
            _generator.GetBytes(bytes);
            return (double)BitConverter.ToUInt32(bytes, 0) / UInt32.MaxValue;
        }

        /// <summary>
        /// Returns a random number within the specified range (both inclusive).
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the value returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the value returned.</param>
        /// <returns></returns>
        public static int Next(int minValue, int maxValue)
        {
            byte[] bytes = new byte[4];
            _generator.GetBytes(bytes);
            double dbl = (double)BitConverter.ToUInt32(bytes, 0) / ((double)UInt32.MaxValue + 1);

            long range = (long)maxValue - minValue + 1;
            return (int)((long)Math.Floor(dbl * range) + minValue);
        }
    }
}
