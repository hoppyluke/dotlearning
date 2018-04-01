using System;
using DotLearning.Mathematics.LinearAlgebra;

namespace DotLearning.NeuralNetworks.MatrixBased
{
    public class NeuralNetwork
    {
        private readonly Layer[] _layers;
        private readonly int _expectedInputs;

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
    }
}
