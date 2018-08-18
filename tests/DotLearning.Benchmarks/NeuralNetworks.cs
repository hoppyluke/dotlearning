using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using DotLearning.Mathematics.LinearAlgebra;
using MatrixNet = DotLearning.NeuralNetworks.MatrixBased.NeuralNetwork;
using SimpleNet = DotLearning.NeuralNetworks.Simple.NeuralNetwork;

namespace DotLearning.Benchmarks
{
    [ShortRunJob]
    public class NeuralNetworks
    {
        private readonly int[] layerSizes = new int[] { 10, 10, 10 };
        private const int epochs = 100;
        private const int batchSize = 100;
        private const double learningRate = 0.05d;
        private readonly Func<double, double, double> meanSquaredErrorDerivative = (y, a) => a - y;

        private const int trainingSetSize = 10000;

        private List<(double[], double[])> trainingData;
        private List<(Vector, Vector)> trainingVectors;

        private SimpleNet simple;
        private MatrixNet matrix;
        
        [GlobalSetup]
        public void CreateTrainingData()
        {
            simple = new SimpleNet(layerSizes);
            matrix = new MatrixNet(layerSizes);

            var inputSize = layerSizes[0];
            var outputSize = layerSizes[layerSizes.Length - 1];

            trainingData = new List<(double[], double[])>(trainingSetSize);
            trainingVectors = new List<(Vector, Vector)>(trainingSetSize);

            for (var i = 0; i < trainingSetSize; i++)
            {
                var input = DataGenerator.Array(inputSize);
                var output = DataGenerator.Array(outputSize);
                trainingData.Add((input, output));
                trainingVectors.Add((new Vector(input), new Vector(output)));
            }
        }

        [Benchmark]
        public void Simple()
        {
            simple.Train(trainingData, epochs, batchSize, learningRate, meanSquaredErrorDerivative);
        }

        [Benchmark]
        public void Matrix()
        {
            matrix.Train(trainingVectors, epochs, batchSize, learningRate, meanSquaredErrorDerivative);
        }
    }
}