using System;
using System.Collections.Generic;
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
            Assert.Throws<ArgumentNullException>(() => new Neuron(null, new double[1], 0d, d => d, d => d));
        }

        [Fact]
        public void Constructor_Throws_IfWeightsAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Neuron(new INeuron[1], null, 0d, d => d, d => d));
        }

        [Fact]
        public void Constructor_Throws_IfActivationFunctionIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Neuron(new INeuron[1], new[] { 1d }, 0d, null, d => d));
        }

        [Fact]
        public void Constructor_Throws_IfActivationFunctionDerivativeIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Neuron(new INeuron[1], new[] { 1d }, 0d, d => d, null));
        }

        [Theory]
        [InlineData(3, 2)]
        public void Constructor_Throws_IfInputsAndWeightsDoNotMatch(int numberOfInputs, int numberOfWeights)
        {
            var inputs = new INeuron[numberOfInputs];
            var weights = new double[numberOfWeights];

            Assert.Throws<ArgumentException>(() => new Neuron(inputs, weights, 0d, d => d, d => d));
        }

        [Theory]
        [InlineData(new[] { 1d }, new[] { 2d }, 2d)]
        [InlineData(new[] { 2d, 2d }, new[] { 1d, 2d }, 6d)]
        public void Calculate_AppliesWeights(double[] inputs, double[] weights, double expectedOutput)
        {
            var inputLayer = inputs.Select(i => new InputNeuron { Value = i }).ToArray();
            var neuron = new Neuron(inputLayer, weights, 0d, d => d, d => d);

            neuron.Calculate();

            Assert.Equal(expectedOutput, neuron.Output);
        }

        [Theory]
        [InlineData(0.5d)]
        public void Calculate_AppliesBias(double bias)
        {
            var neuron = new Neuron(new INeuron[0], new double[0], bias, d => d, d => d);

            neuron.Calculate();

            Assert.Equal(bias, neuron.Output);
        }

        [Fact]
        public void Calculate_AppliesActivationFunction()
        {
            var neuron = new Neuron(new[] { new InputNeuron { Value = 2d } }, new[] { 1d }, 0d, d => d * 2d, d => d);

            neuron.Calculate();

            Assert.Equal(4d, neuron.Output);
        }

        [Fact]
        public void CalculateError_Throws_IfCostDerivativeIsNull()
        {
            var neuron = CreateSimpleNeuron();

            Assert.Throws<ArgumentNullException>(() => neuron.CalculateError(1d, null));
        }

        [Theory]
        [InlineData(1d, 10d, 0d, 11d)]
        public void CalculateError_AppliesCostDerivative(double input, double weight, double bias, double expectedOutput)
        {
            double identity(double x) => x;
            double return1(double x) => 1d;
            double costDerivative(double y, double a) => a - y;
            var inputLayer = new INeuron[] { new InputNeuron { Value = input } };
            var neuron = new Neuron(inputLayer, new[] { weight }, bias, identity, return1);
            neuron.Calculate();

            neuron.CalculateError(expectedOutput, costDerivative);

            Assert.Equal(neuron.Output - expectedOutput, neuron.Error);
        }

        [Theory]
        [InlineData(1d, 10d, -1d)]
        public void CalculateError_AppliesActivationFunctionDerivative_ForOutputLayer(double input, double weight, double bias)
        {
            double identity(double x) => x;
            double times2(double x) => 2 * x;
            double return1(double x, double y) => 1d;
            var inputLayer = new INeuron[] { new InputNeuron { Value = input } };
            var neuron = new Neuron(inputLayer, new[] { weight }, bias, identity, times2);
            neuron.Calculate();

            neuron.CalculateError(1d, return1); // disregards cost derivative

            Assert.Equal(times2(input * weight + bias), neuron.Error);
        }

        [Fact]
        public void CalculateError_Throws_IfNextLayerErrorsAreNull()
        {
            var inputLayer = new INeuron[] { new InputNeuron { Value = 1d } };
            var neuron = new Neuron(inputLayer, new[] { 1d }, 1d, x => x, x => x);

            Assert.Throws<ArgumentNullException>(() => neuron.CalculateError(null, new[] { 1d }));
        }

        [Fact]
        public void CalculateError_Throws_IfNextLayerWeightsAreNull()
        {
            var neuron = CreateSimpleNeuron();

            Assert.Throws<ArgumentNullException>(() => neuron.CalculateError(new[] { 1d }, null));
        }

        [Theory]
        [InlineData(1, 2)]
        public void CalculateError_Throws_IfNextLayerErrorsAndWeightsDoNotMatch(int numberOfErrors, int numberOfWeights)
        {
            var neuron = CreateSimpleNeuron();
            var errors = new double[numberOfErrors];
            var weights = new double[numberOfWeights];

            Assert.Throws<ArgumentException>(() => neuron.CalculateError(errors, weights));
        }

        [Theory]
        [InlineData(new[] { 1d, 2d }, new[] { 2d, 2d })]
        public void CalculateError_AppliesSumOfWeightedErrorsInNextLayer(double[] errors, double[] weights)
        {
            // Activation function derivate is set to always return 1, so it will disregard this portion of error
            var neuron = new Neuron(new[] { new InputNeuron() }, new[] { 0d }, 0d, x => x, x => 1);
            var expectedError = errors.Zip(weights, (x, y) => x * y).Sum();

            neuron.CalculateError(errors, weights);

            Assert.Equal(expectedError, neuron.Error);
        }

        [Theory]
        [InlineData(1d, 10d, -1d)]
        public void CalculateError_AppliesActivationFunctionDerivative_ForHiddenLayer(double input, double weight, double bias)
        {
            double identity(double x) => x;
            double times2(double x) => 2 * x;
            var inputLayer = new INeuron[] { new InputNeuron { Value = input } };
            var neuron = new Neuron(inputLayer, new[] { weight }, bias, identity, times2);
            neuron.Calculate();

            neuron.CalculateError(new[] { 1d }, new[] { 1d });

            Assert.Equal(times2(input * weight + bias), neuron.Error);
        }

        [Theory]
        [InlineData(new[] { 1d, 2d }, 1d)]
        [InlineData(new[] { 1d, 2d }, 0.1d)]
        public void Learn_UpdatesBias(double[] inputs, double learningRate)
        {
            var inputNeuron = new InputNeuron();
            // Neuron always outputs zero and activation function derivative always = 1d
            // so error is always = expected output
            var neuron = new Neuron(new[] { inputNeuron }, new[] { 0d }, 0d, x => x, x => 1d);
            
            foreach(var input in inputs)
            {
                inputNeuron.Value = input;
                neuron.Calculate();
                neuron.CalculateError(input, (y, a) => y);
            }

            neuron.Learn(learningRate);
            
            Assert.Equal(inputs.Average() * learningRate * -1, neuron.Bias);
        }

        [Theory]
        [MemberData(nameof(InputsForLearningWeights))]
        public void Learn_UpdatesWeights(double[][] inputs, double learningRate)
        {
            var numberOfInputs = inputs[0].Length;
            var inputLayer = inputs[0].Select(i => new InputNeuron()).ToArray();
            // Neuron always outputs zero and activation function derivative always = 1d
            // so error is always = expected output
            var neuron = new Neuron(inputLayer, new double[numberOfInputs], 0d, x => x, x => 1d);

            foreach (var input in inputs)
            {
                for (var i = 0; i < input.Length; i++)
                    inputLayer[i].Value = input[i];

                neuron.Calculate();
                neuron.CalculateError(input.Sum(), (y, a) => y);
            }

            neuron.Learn(learningRate);

            for(var i = 0; i < numberOfInputs; i++)
            {
                // error is sum of all inputs so expect each weight to update by average of (error * input[i])
                var expectedWeight = inputs.Select(x => x[i] * x.Sum()).Average() * learningRate * -1;
                Assert.Equal(expectedWeight, neuron.Weights[i]);
            }
        }

        [Fact]
        public void Learn_ResetsError()
        {
            var neuron = CreateSimpleNeuron();
            neuron.Calculate();
            neuron.CalculateError(100d, (y, a) => a - y);
            var previousError = neuron.Error;

            neuron.Learn(1d);

            Assert.Equal(0d, neuron.Error);
            Assert.NotEqual(previousError, neuron.Error);
        }

        [Fact]
        public void Learn_IsBasedOnErrorsSinceLastLearnOnly()
        {
            double costDerivative(double expected, double actual) => actual - expected;
            var inputNeuron = new InputNeuron { Value = 1 };
            var neuron = new Neuron(new[] { inputNeuron }, new[] { 1d }, 0d, x => x, x => 1d);

            // Never calling Calculate() so output is always 0
            neuron.CalculateError(100d, costDerivative);
            neuron.Learn(1d);
            var previousBias = neuron.Bias;

            var extraExamples = new[] { 2d, 6d };
            foreach(var expectedOutput in extraExamples)
                neuron.CalculateError(expectedOutput, costDerivative);
            neuron.Learn(1d);

            // Expecting change in bias to be a function of extra examples only
            // (and not previous large error)
            var changeInBias = neuron.Bias - previousBias;
            Assert.Equal(extraExamples.Average(), changeInBias);
        }

        private Neuron CreateSimpleNeuron()
        {
            var inputLayer = new INeuron[] { new InputNeuron { Value = 1d } };
            return new Neuron(inputLayer, new[] { 1d }, 1d, x => x, x => x);
        }

        public static IEnumerable<object[]> InputsForLearningWeights()
        {
            var inputs = new double[][]
            {
                new[] { 1d, 2d },
                new[] { 2d, 4d }
            };

            var learningRates = new double[] { 1d, 0.1d };

            return learningRates.Select(r => new object[] { inputs, r });
        }
    }
}
