using System;
using System.Linq;

namespace DotLearning.NeuralNetworks.Simple
{
    /// <summary>
    /// A feedforward neural network.
    /// </summary>
    public class NeuralNetwork
    {
        private readonly InputNeuron[] _inputLayer;
        private readonly ICalculatingNeuron[][] _hiddenLayers;
        private readonly ICalculatingNeuron[] _outputLayer;

        /// <summary>
        /// Creates a network of sigmoid neurons.
        /// </summary>
        /// <param name="layerSizes">Number of neurons in each layer of the network.</param>
        public NeuralNetwork(params int[] layerSizes)
            : this(NeuronFactory.SigmoidNeuron, layerSizes)
        { }

        /// <summary>
        /// Creates a new neural network.
        /// </summary>
        /// <param name="neuronFactory">Factory to create a neuron given the previous layer.</param>
        /// <param name="layerSizes">Number of neurons in each layer of the network.</param>
        internal NeuralNetwork(Func<INeuron[], ICalculatingNeuron> neuronFactory, params int[] layerSizes)
        {
            if (neuronFactory == null) throw new ArgumentNullException(nameof(neuronFactory));
            if (layerSizes == null) throw new ArgumentNullException(nameof(layerSizes));

            var numberOfLayers = layerSizes.Length;

            if (numberOfLayers < 2)
                throw new ArgumentOutOfRangeException(nameof(layerSizes), "Network must have at least 2 layers");

            _inputLayer = new InputNeuron[layerSizes[0]];
            for (var i = 0; i < _inputLayer.Length; i++)
                _inputLayer[i] = new InputNeuron();

            INeuron[] previousLayer = _inputLayer;
            _hiddenLayers = new ICalculatingNeuron[numberOfLayers - 2][];
            for (var i = 1; i < numberOfLayers - 1; i++)
            {
                var currentLayer = CreateLayer(layerSizes[i], previousLayer, neuronFactory);
                _hiddenLayers[i - 1] = currentLayer;
                previousLayer = currentLayer;
            }

            _outputLayer = CreateLayer(layerSizes[numberOfLayers - 1], previousLayer, neuronFactory);
        }

        public double[] Calculate(double[] inputs)
        {
            if (inputs == null) throw new ArgumentNullException(nameof(inputs));

            if (inputs.Length != _inputLayer.Length)
                throw new ArgumentException("Number of inputs must equal size of input layer.", nameof(inputs));

            for (var i = 0; i < _inputLayer.Length; i++)
                _inputLayer[i].Value = inputs[i];

            foreach (var layer in _hiddenLayers)
                foreach (var neuron in layer)
                    neuron.Calculate();

            foreach (var neuron in _outputLayer)
                neuron.Calculate();

            return _outputLayer.Select(n => n.Output).ToArray();
        }

        private static ICalculatingNeuron[] CreateLayer(int size, INeuron[] previousLayer, Func<INeuron[], ICalculatingNeuron> neuronFactory)
        {
            var layer = new ICalculatingNeuron[size];

            for (var i = 0; i < size; i++)
                layer[i] = neuronFactory(previousLayer);

            return layer;
        }
    }
}
