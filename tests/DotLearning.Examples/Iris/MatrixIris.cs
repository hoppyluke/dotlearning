using System;
using System.Collections.Generic;
using System.Linq;
using DotLearning.Mathematics.LinearAlgebra;
using DotLearning.NeuralNetworks.MatrixBased;

namespace DotLearning.Examples.Iris
{
    // https://www.tensorflow.org/get_started/get_started_for_beginners
    internal class MatrixIris
    {
        private List<(Vector input, Vector expected)> _trainingData;
        private List<(Vector input, int expected)> _testData;
        private NeuralNetwork _network;
        private List<(int expected, int predicted)> _results;

        public void LoadTrainingData()
        {
            var rawTrainingData = CsvDataLoader.GetData("Datasets\\iris_training.csv", skipRows: 1);

            _trainingData = rawTrainingData.Select(e =>
                (new Vector(new double[] { e[0], e[1], e[2], e[3] }), To1HotVector(e[4], 3))
            ).ToList();
        }

        public void CreateNetwork()
        {
            _network = new NeuralNetwork(4, 10, 10, 3);
        }

        public void Train(int epochs, int batchSize, double learningRate)
        {
            _network.Train(_trainingData, epochs, batchSize, learningRate, (y, a) => a - y);
        }

        public void LoadTestData()
        {
            var rawTestData = CsvDataLoader.GetData("Datasets\\iris_test.csv", skipRows: 1);

            _testData = rawTestData.Select(e =>
                (new Vector(new double[] { e[0], e[1], e[2], e[3] }), (int)e[4])
            ).ToList();
        }

        public void Test()
        {
            _results = new List<(int, int)>(_testData.Count);

            foreach (var example in _testData)
            {
                var output = _network.Calculate(example.input);
                var prediction = InterpretPrediction(output);

                _results.Add((example.expected, prediction));
            }
        }

        public void ShowResults()
        {
            for (var i = 0; i < _results.Count; i++)
            {
                var result = _results[i];
                var outcome = result.predicted == result.expected ? "PASS" : "FAIL";
                var expectedClass = _results[i].expected;
                var predictedClass = _results[i].predicted;
                Console.WriteLine($"Test {i + 1:D2}/{_results.Count:D2}: actual = {GetClassLabel(expectedClass)}, predicted = {GetClassLabel(predictedClass)} [{outcome}]");
            }

            Console.WriteLine();

            var successes = _results.Count(r => r.expected == r.predicted);
            var successRate = successes / (double)_results.Count;
            Console.WriteLine($"Results: {successes} out of {_results.Count} classified correctly ({successRate:P2})");
        }

        private Vector To1HotVector(double value, int classes)
        {
            var vector = new double[classes];
            vector[(int)value] = 1d;
            return new Vector(vector);
        }

        private int InterpretPrediction(Vector output)
        {
            var max = double.MinValue;
            var indexOfMax = -1;

            for (var i = 0; i < output.Count; i++)
            {
                if (output[i] > max)
                {
                    max = output[i];
                    indexOfMax = i;
                }
            }

            return indexOfMax;
        }

        private string GetClassLabel(int classIndex) => new[] { "Setosa", "Veriscolor", "Virginica" }[classIndex];
    }
}
