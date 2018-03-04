using System;
using System.Collections.Generic;
using System.Linq;
using DotLearning.Mathematics;

namespace DotLearning.NeuralNetworks.Simple
{
    /// <summary>
    /// A non-input neuron.
    /// </summary>
    internal class Neuron : ICalculatingNeuron
    {
        private readonly INeuron[] _inputs;
        private readonly double[] _weights;
        private readonly Func<double, double> _activationFunction;
        private readonly Func<double, double> _activationFunctionDerivative;
        private double _weightedInput;
        
        /// <summary>
        /// Errors seen whilst training.
        /// </summary>
        private List<double> _errors;

        /// <summary>
        /// Activations from the previous layer seen whilst training.
        /// </summary>
        private List<double[]> _trainingInputs;

        private double _currentError;

        public double Bias { get; private set; }
        public IReadOnlyList<double> Weights => _weights;
        public double Output { get; private set; }

        public double Error
        {
            get => _currentError;
            private set
            {
                _currentError = value;
                _errors.Add(value);
                _trainingInputs.Add(_inputs.Select(n => n.Output).ToArray());
            }
        }

        /// <summary>
        /// Creates a new sigmoid neuron.
        /// </summary>
        /// <param name="inputs">Inputs for this neuron.</param>
        public Neuron(INeuron[] inputs)
            : this(inputs, new double[inputs.Length], 0d, MathematicalFunctions.Sigmoid, MathematicalFunctions.SigmoidPrime)
        { }
        
        public Neuron(INeuron[] inputs, double[] weights, double bias, Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));
            if (weights == null) throw new ArgumentNullException(nameof(weights));
            if (inputs.Length != weights.Length)
                throw new ArgumentException($"Cannot create a neuron with {inputs.Length} inputs and {weights.Length} weights.", nameof(weights));

            _activationFunction = activationFunction ?? throw new ArgumentNullException(nameof(activationFunction));
            _activationFunctionDerivative = activationFunctionDerivative ?? throw new ArgumentNullException(nameof(activationFunctionDerivative));

            _inputs = inputs;
            _weights = weights;
            Bias = bias;
            
            _errors = new List<double>();
            _trainingInputs = new List<double[]>();
        }
        
        public void Calculate()
        {
            _weightedInput = _inputs
                .Zip(_weights, (n, w) => n.Output * w)
                .Sum() + Bias;
            
            Output = _activationFunction(_weightedInput);
        }
        
        public void CalculateError(double expectedOutput, Func<double, double, double> costDerivative)
        {
            if (costDerivative == null) throw new ArgumentNullException(nameof(costDerivative));

            Error = costDerivative(expectedOutput, Output) * _activationFunctionDerivative(_weightedInput);
        }
        
        public void CalculateError(double[] nextLayerErrors, double[] nextLayerWeights)
        {
            if (nextLayerErrors == null) throw new ArgumentNullException(nameof(nextLayerErrors));
            if (nextLayerWeights == null) throw new ArgumentNullException(nameof(nextLayerWeights));
            if (nextLayerErrors.Length != nextLayerWeights.Length)
                throw new ArgumentException($"Got {nextLayerErrors.Length} error and {nextLayerWeights.Length} length values but was expecting an equal number");

            Error = nextLayerErrors
                .Zip(nextLayerWeights, (e, w) => e * w)
                .Sum() * _activationFunctionDerivative(_weightedInput);
        }
        
        public void Learn(double learningRate)
        {
            // Update bias by average error across training examples mutliplied by learning rate
            Bias -= _errors.Average() * learningRate;

            // Update weights by average(error * input) for each input multiplied by learning rate
            for(var i = 0; i < _weights.Length; i++)
            {
                var averageErrorForWeight = _trainingInputs
                    .Select(input => input[i])
                    .Zip(_errors, (input, error) => input * error)
                    .Average();
                
                _weights[i] -= averageErrorForWeight * learningRate;
            }

            // Reset learning status for next batch
            _currentError = 0d;
            _errors.Clear();
            _trainingInputs.Clear();
        }
    }
}