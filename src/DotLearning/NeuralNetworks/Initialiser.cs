using System;

namespace DotLearning.NeuralNetworks
{
    internal static class Initialiser
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Generates a random weight in [-0.5, 0.5].
        /// </summary>
        /// <returns>Randomly initialised weight value.</returns>
        public static double Weight() => Random.NextDouble() - 0.5d;

        /// <summary>
        /// Generates a random bias in [0, 1].
        /// </summary>
        /// <returns>Randomly initialised bias value.</returns>
        public static double Bias() => Random.NextDouble();
        
    }
}
