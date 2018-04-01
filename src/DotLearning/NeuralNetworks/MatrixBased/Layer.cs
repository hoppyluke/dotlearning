using System;
using DotLearning.Mathematics.LinearAlgebra;

namespace DotLearning.NeuralNetworks.MatrixBased
{
    internal class Layer
    {
        private readonly Matrix _weights;
        private readonly Vector _biases;
        private readonly Func<double, double> _activationFunction;

        private Vector _weightedInput;

        public Layer(Matrix weights, Vector biases, Func<double, double> activationFunction)
        {
            if (weights == null) throw new ArgumentNullException(nameof(weights));
            if (biases == null) throw new ArgumentNullException(nameof(biases));
            if (weights.Rows != biases.Count)
                throw new ArgumentException($"Number of weights ({weights.Rows}x{weights.Columns}) and biases ({biases.Count}) do not match");
            
            _weights = weights;
            _biases = biases;
            _activationFunction = activationFunction ?? throw new ArgumentNullException(nameof(activationFunction));
        }

        public virtual Vector Calculate(Vector input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            
            _weightedInput = (Vector)(_weights * input) + _biases;
            return Vector.Apply(_activationFunction, _weightedInput);
        }
    }
}
