using System;
using System.Linq;
using DotLearning.NeuralNetworks.Simple;
using Xunit;

namespace DotLearning.Tests.NeuralNetworks.Simple
{
    public class NeuronTests
    {
        [Fact]
        public void Constructor_Throws_IfInputsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Neuron(null, new double[1], 0d, d => d));
        }

        [Fact]
        public void Constructor_Throws_IfWeightsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Neuron(new INeuron[1], null, 0d, d => d));
        }

        [Theory]
        [InlineData(3, 2)]
        public void Constructor_Throws_IfInputsAndWeightsDoNotMatch(int numberOfInputs, int numberOfWeights)
        {
            var inputs = new INeuron[numberOfInputs];
            var weights = new double[numberOfWeights];

            Assert.Throws<ArgumentException>(() => new Neuron(inputs, weights, 0d, d => d));
        }

        [Theory]
        [InlineData(new [] { 1d }, new[] { 2d }, 2d)]
        [InlineData(new[] { 2d, 2d }, new[] { 1d, 2d }, 6d)]
        public void Calculate_AppliesWeights(double[] inputs, double[] weights, double expectedOutput)
        {
            var inputLayer = inputs.Select(i => new InputNeuron { Value = i }).ToArray();
            var neuron = new Neuron(inputLayer, weights, 0d, d => d);

            neuron.Calculate();

            Assert.Equal(expectedOutput, neuron.Output);
        }

        [Theory]
        [InlineData(0.5d)]
        public void Calculate_AppliesBias(double bias)
        {
            var neuron = new Neuron(new INeuron[0], new double[0], bias, d => d);

            neuron.Calculate();

            Assert.Equal(bias, neuron.Output);
        }

        [Fact]
        public void Calculate_AppliesActivationFunction()
        {
            var neuron = new Neuron(new[] { new InputNeuron { Value = 2d } }, new[] { 1d }, 0d, d => d * 2d);

            neuron.Calculate();

            Assert.Equal(4d, neuron.Output);
        }
    }
}
