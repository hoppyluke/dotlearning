using System;
using System.Collections.Generic;
using System.Linq;
using DotLearning.NeuralNetworks.Simple;

namespace DotLearning.Examples
{
    // https://www.tensorflow.org/get_started/get_started_for_beginners
    internal class Iris
    {
        private List<(double[] input, double[] expected)> _trainingData;
        private List<(double[] input, double[] expected)> _testData;
        private NeuralNetworkClassifier _classifier;
        private List<(int expected, int predicted)> _results;
        
        public void LoadTrainingData()
        {
            var rawTrainingData = CsvDataLoader.GetData("Datasets\\iris_training.csv", skipRows: 1);

            _trainingData = rawTrainingData.Select(e => (new double[] { e[0], e[1], e[2], e[3] }, To1HotVector(e[4], 3)))
                .ToList();
        }

        public void CreateNetwork()
        {
            _classifier = new NeuralNetworkClassifier(4, new[] { "Setosa", "Veriscolor", "Virginica" }, 10, 10);
        }

        public void Train(int epochs, int batchSize, double learningRate)
        {
            _classifier.Train(_trainingData, epochs, batchSize, learningRate);
        }

        public void LoadTestData()
        {
            var rawTestData = CsvDataLoader.GetData("Datasets\\iris_test.csv", skipRows: 1);

            _testData = rawTestData.Select(e => (new double[] { e[0], e[1], e[2], e[3] }, new double[] { e[4] }))
                .ToList();
        }

        public void Test()
        {
            _results = new List<(int, int)>(_testData.Count);

            foreach(var example in _testData)
            {
                var prediction = _classifier.Classify(example.input);

                _results.Add(((int)example.expected[0], prediction));
            }
        }

        public void ShowResults()
        {
            for(var i = 0; i < _results.Count; i++)
            {
                var result = _results[i];
                var outcome = result.predicted == result.expected ? "PASS" : "FAIL";
                var expectedClass = _results[i].expected;
                var predictedClass = _results[i].predicted;
                Console.WriteLine($"Test {i + 1:D2}/{_results.Count:D2}: actual = {_classifier.GetLabel(expectedClass)}, predicted = {_classifier.GetLabel(predictedClass)} [{outcome}]");
            }

            Console.WriteLine();

            var successes = _results.Count(r => r.expected == r.predicted);
            var successRate = successes / (double)_results.Count;
            Console.WriteLine($"Results: {successes} out of {_results.Count} classified correctly ({successRate:P2})");
        }

        private double[] To1HotVector(double value, int classes)
        {
            var vector = new double[classes];
            vector[(int)value] = 1d;
            return vector;
        }
    }
}
