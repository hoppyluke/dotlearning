using System;
using System.Collections.Generic;
using System.Linq;
using DotLearning.Mathematics.LinearAlgebra;

namespace DotLearning.NeuralNetworks.MatrixBased
{
    internal class Layer
    {
        private readonly Func<double, double> _activationFunction;
        private readonly Func<double, double> _activationFunctionDerivative;
        public Matrix _weights;
        private Vector _biases;
        
        private Vector _lastInput, _weightedInput, _output, _error;
        private List<Vector> _errors, _trainingInputs;
        
        public Layer(Matrix weights, Vector biases, Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative)
        {
            if (weights == null) throw new ArgumentNullException(nameof(weights));
            if (biases == null) throw new ArgumentNullException(nameof(biases));
            if (weights.Rows != biases.Count)
                throw new ArgumentException($"Number of weights ({weights.Rows}x{weights.Columns}) and biases ({biases.Count}) do not match");
            
            _weights = weights;
            _biases = biases;
            _activationFunction = activationFunction ?? throw new ArgumentNullException(nameof(activationFunction));
            _activationFunctionDerivative = activationFunctionDerivative ?? throw new ArgumentNullException(nameof(activationFunction));
            _errors = new List<Vector>();
            _trainingInputs = new List<Vector>();
        }

        public virtual Vector Calculate(Vector input)
        {
            _lastInput = input ?? throw new ArgumentNullException(nameof(input));

            _weightedInput = (Vector)(_weights * input) + _biases;
            return _output = Vector.Apply(_activationFunction, _weightedInput);
        }
        
        /// <summary>
        /// Calculates the error in the output layer.
        /// </summary>
        /// <param name="expectedOutput">Expected output from the network.</param>
        /// <param name="costDerivative">Partial derivative of cost function with respect to single activation.</param>
        /// <returns>Vector of errors.</returns>
        public void CalculateError(Vector expectedOutput, Func<double, double, double> costDerivative)
        {
            var costDerivatives = Vector.Zip(expectedOutput, _output, costDerivative);
            CalculateError(costDerivatives);
        }

        /// <summary>
        /// Calculates the error in a hidden layer.
        /// </summary>
        /// <param name="nextLayer">Layer after this one in the network.</param>
        public void CalculateError(Layer nextLayer)
        {
            var weightedErrors = nextLayer._weights.Transpose() * nextLayer._error;
            CalculateError((Vector)weightedErrors);
        }

        public void Learn(double learningRate)
        {
            var examples = (double)_trainingInputs.Count;

            var totalError = _errors.Aggregate((sum, e) => sum + e);

            var deltaWeights = (learningRate / examples)
                * _trainingInputs.Zip(_errors, (input, e) => e * ((Matrix)input).Transpose())
                    .Aggregate((sum, e) => sum + e);

            var deltaBiases = (learningRate / examples) * totalError;

            _weights -= deltaWeights;
            _biases -= deltaBiases;

            _trainingInputs.Clear();
            _errors.Clear();
            _error = null;

        }

        private void CalculateError(Vector costDerivatives)
        {
            var inputDerivatives = Vector.Apply(_activationFunctionDerivative, _weightedInput);
            _error = Vector.HadamardProduct(costDerivatives, inputDerivatives);
            
            _trainingInputs.Add(_lastInput);
            _errors.Add(_error);
        }
    }
}
