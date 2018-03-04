using System;
using System.Collections.Generic;

namespace DotLearning.NeuralNetworks.Simple
{
    /// <summary>
    /// A neuron which calculates its output based on input from previous layer.
    /// </summary>
    internal interface ICalculatingNeuron : INeuron
    {
        /// <summary>
        /// Current bias value of this neuron.
        /// </summary>
        double Bias { get; }

        /// <summary>
        /// Current weights applied to activations from input neurons.
        /// </summary>
        IReadOnlyList<double> Weights { get; }
        
        /// <summary>
        /// Calculates the output value.
        /// </summary>
        void Calculate();

        /// <summary>
        /// Calculates and records the error at this neuron from the total cost of network output.
        /// This applies where the neuron is an output neuron.
        /// </summary>
        /// <param name="expectedOutput">Expected output from this neuron.</param>
        /// <param name="costDerivative">Partial derivative of the cost function with respect to activation of a single output neuron.</param>
        void CalculateError(double expectedOutput, Func<double, double, double> costDerivative);

        /// <summary>
        /// Calculates and records the error at this neuron from the error of neurons in the next layer
        /// and the weights applied to output of this neuron by the next layer.
        /// This applies where the neuron is a hidden neuron.
        /// </summary>
        /// <param name="nextLayerErrors">Errors at each neuron in the next layer.</param>
        /// <param name="nextLayerWeights">Weight applied by each neuron in the next layer to the output of this neuron.</param>
        void CalculateError(double[] nextLayerErrors, double[] nextLayerWeights);

        /// <summary>
        /// Last error observed at this neuron.
        /// </summary>
        double Error { get; }

        /// <summary>
        /// Updates weights and biases based on errors observed at this neuron
        /// since the last time learn was called.
        /// </summary>
        /// <param name="learningRate">Learning rate of the network.</param>
        void Learn(double learningRate);
    }
}