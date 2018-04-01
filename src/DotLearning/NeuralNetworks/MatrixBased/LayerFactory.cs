using DotLearning.Mathematics;
using DotLearning.Mathematics.LinearAlgebra;

namespace DotLearning.NeuralNetworks.MatrixBased
{
    internal static class LayerFactory
    {
        /// <summary>
        /// Creates a layer of sigmoid neurons with randomly initialised weights and biases.
        /// </summary>
        /// <param name="size">Number of neurons in this layer.</param>
        /// <param name="numberOfInputs">Number of inputs to this layer.</param>
        /// <returns>New layer.</returns>
        public static Layer SigmoidNeurons(int size, int numberOfInputs)
        {
            var weights = new Matrix(size, numberOfInputs);
            for (var i = 0; i < size; i++)
                for (var j = 0; j < numberOfInputs; j++)
                    weights[i, j] = Initialiser.Weight();

            var biases = new Vector(size);
            for (var i = 0; i < size; i++)
                biases[i] = Initialiser.Bias();

            return new Layer(weights, biases, MathematicalFunctions.Sigmoid);
        }
    }
}