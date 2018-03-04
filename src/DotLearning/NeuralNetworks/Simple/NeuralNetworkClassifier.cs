using System.Collections.Generic;

namespace DotLearning.NeuralNetworks.Simple
{
    public class NeuralNetworkClassifier
    {
        private readonly string[] _classLabels;
        private readonly NeuralNetwork _network;

        public NeuralNetwork Network => _network;

        public NeuralNetworkClassifier(int features, string[] classes, params int[] hiddenLayerSizes)
        {
            var networkLayerSizes = new List<int>();
            networkLayerSizes.Add(features);
            networkLayerSizes.AddRange(hiddenLayerSizes);
            networkLayerSizes.Add(classes.Length);

            _classLabels = classes;

            _network = new NeuralNetwork(networkLayerSizes.ToArray());
        }

        public void Train(IEnumerable<(double[] input, double[] expected)> trainingData, int epochs, int batchSize, double learningRate)
        {
            _network.Train(trainingData, epochs, batchSize, learningRate, (y, a) => a - y);
        }

        public int Classify(double[] example)
        {
            var output = _network.Calculate(example);
            return Interpret(output);
        }

        public string GetLabel(int classIndex) => _classLabels[classIndex];

        private int Interpret(double[] output)
        {
            var max = double.MinValue;
            var indexOfMax = -1;

            for (var i = 0; i < output.Length; i++)
            {
                if (output[i] > max)
                {
                    max = output[i];
                    indexOfMax = i;
                }
            }

            return indexOfMax;
        }
    }
}
