using System;
using System.Collections.Generic;

namespace DotLearning.Tests.NeuralNetworks
{
    public class DataGenerator
    {
        private readonly Random _random = new Random();

        public IEnumerable<(double[] input, double[] expected)> RandomTrainingData(int size, int features, int outputs)
        {
            var trainingData = new List<(double[] input, double[] expected)>(size);

            for(var i = 0; i < size; i++)
            {
                var example = (RandomArray(features), RandomArray(outputs));
                trainingData.Add(example);
            }

            return trainingData;
        }

        private double[] RandomArray(int size)
        {
            var a = new double[size];
            for (var i = 0; i < a.Length; i++)
                a[i] = _random.NextDouble();

            return a;
        }
    }
}
