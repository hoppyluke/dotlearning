using System;
using DotLearning.Mathematics;

namespace DotLearning.NeuralNetworks.Simple
{
    internal class Neuron : ICalculatingNeuron
    {
        private readonly INeuron[] _inputs;
        private readonly double[] _weights;
        private double _bias;
        private readonly Func<double, double> _activationFunction;
        
        public double Output { get; private set; }
        
        /// <summary>
        /// Creates a new sigmoid neuron.
        /// </summary>
        /// <param name="inputs">Inputs for this neuron.</param>
        public Neuron(INeuron[] inputs)
            : this(inputs, new double[inputs.Length], 0d, MathematicalFunctions.Sigmoid)
        { }
        
        public Neuron(INeuron[] inputs, double[] weights, double bias, Func<double, double> activationFunction)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));
            if (weights == null) throw new ArgumentNullException(nameof(weights));
            if (inputs.Length != weights.Length)
                throw new ArgumentException($"Cannot create a neuron with {inputs.Length} inputs and {weights.Length} weights.", nameof(weights));

            _inputs = inputs;
            _weights = weights;
            _bias = bias;
            _activationFunction = activationFunction;
        }

        /// <summary>
        /// Calculates and sets the output value of this neuron.
        /// </summary>
        public void Calculate()
        {
            var weightedInput = 0d;

            for (var i = 0; i < _inputs.Length; i++)
                weightedInput += _inputs[i].Output * _weights[i];

            Output = _activationFunction(weightedInput + _bias);
        }
    }
}