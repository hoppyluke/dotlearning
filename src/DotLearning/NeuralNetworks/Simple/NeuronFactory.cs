using System;
using DotLearning.Mathematics;

namespace DotLearning.NeuralNetworks.Simple
{
    internal static class NeuronFactory
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// Creates a new sigmoid neuron with randomly initialised weights and bias in [-0.5, 0.5].
        /// </summary>
        /// <param name="inputs">Previous layer for this neuron.</param>
        /// <returns>Neuron.</returns>
        public static ICalculatingNeuron SigmoidNeuron(INeuron[] inputs)
        {
            var weights =  new double[inputs.Length];
            for (var i = 0; i < inputs.Length; i++)
                weights[i] = Random.NextDouble() - 0.5d;

            var bias = Random.NextDouble();

            return new Neuron(inputs, weights, bias, MathematicalFunctions.Sigmoid, MathematicalFunctions.SigmoidPrime);
        }
    }
}
