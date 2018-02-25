using System;

namespace DotLearning.Mathematics
{
    public static class MathematicalFunctions
    {
        /// <summary>
        /// Sigmoid function: f(z) = 1 / 1 + exp(-z)
        /// </summary>
        public static double Sigmoid(double z) => 1 / (1 + Math.Exp(-z));

        /// <summary>
        /// Derivative of the Sigmoid function.
        /// </summary>
        public static double SigmoidPrime(double z) => Sigmoid(z) * (1 - Sigmoid(z));
    }
}