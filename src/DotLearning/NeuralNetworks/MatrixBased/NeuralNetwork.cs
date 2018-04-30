using System;
using System.Collections.Generic;
using System.Linq;
using DotLearning.Mathematics.LinearAlgebra;

namespace DotLearning.NeuralNetworks.MatrixBased
{
    public class NeuralNetwork
    {
        private readonly Layer[] _layers;
        private readonly int _expectedInputs;

        private Layer OutputLayer => _layers[_layers.Length - 1];

        public NeuralNetwork(params int[] layerSizes)
            : this(LayerFactory.SigmoidNeurons, layerSizes)
        { }

        internal NeuralNetwork(Func<int, int, Layer> layerFactory, params int[] layerSizes)
        {
            if (layerFactory == null) throw new ArgumentNullException(nameof(layerFactory));
            if (layerSizes == null) throw new ArgumentNullException(nameof(layerSizes));

            var numberOfLayers = layerSizes.Length;

            if (numberOfLayers < 2)
                throw new ArgumentOutOfRangeException(nameof(layerSizes), "Network must have at least 2 layers");

            // Input layer is not required
            _expectedInputs = layerSizes[0];
            _layers = new Layer[numberOfLayers - 1];

            for (var i = 0; i < _layers.Length; i++)
            {
                var neuronsInThisLayer = layerSizes[i + 1];
                var neuronsInPreviousLayer = layerSizes[i];
                _layers[i] = layerFactory(neuronsInThisLayer, neuronsInPreviousLayer);
            }
        }

        public Vector Calculate(Vector input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.Count != _expectedInputs) throw new ArgumentException($"Network requires {_expectedInputs} inputs");

            var previousActivation = input;

            foreach (var layer in _layers)
                previousActivation = layer.Calculate(previousActivation);
            
            return previousActivation;
        }

        /// <summary>
        /// Trains the neural network by stochastic gradient descent.
        /// </summary>
        /// <param name="trainingData">Pairs of example input and expected output to train on.</param>
        /// <param name="epochs">Number of training epochs to run.</param>
        /// <param name="batchSize"></param>
        /// <param name="learningRate"></param>
        /// <param name="costDerivative">
        /// Partial derivative of the cost function with respect to a single activation of the output layer.
        /// Takes as arguments the expected and actual activation (in that order).
        /// </param>
        public void Train(IEnumerable<(Vector input, Vector expected)> trainingData, int epochs, int batchSize, double learningRate, Func<double, double, double> costDerivative)
        {
            var examples = trainingData.ToList();
            var totalExamples = examples.Count;

            for (var i = 0; i < epochs; i++)
            {
                var shuffledExamples = examples.OrderBy(d => Guid.NewGuid()).ToList();

                for (var j = 0; j < totalExamples; j += batchSize)
                {
                    var currentBatchSize = Math.Min(batchSize, totalExamples - j);
                    var batch = shuffledExamples.GetRange(j, currentBatchSize);

                    TrainBatch(batch, learningRate, costDerivative);
                }
            }
        }

        private void TrainBatch(IEnumerable<(Vector input, Vector expected)> batch, double learningRate, Func<double, double, double> costDerivative)
        {
            foreach (var example in batch)
            {
                // Feedforward
                Calculate(example.input);

                // Back propagate the error

                OutputLayer.CalculateError(example.expected, costDerivative);

                for(var i = _layers.Length - 2; i >= 0; i--)
                    _layers[i].CalculateError(_layers[i + 1]);
            }

            // Update neuron weights/biases
            foreach (var layer in _layers)
                layer.Learn(learningRate);
        }
    }
}
