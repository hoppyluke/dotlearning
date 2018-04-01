using System;
using System.Collections.Generic;
using DotLearning.Mathematics.LinearAlgebra;
using DotLearning.NeuralNetworks.MatrixBased;
using Xunit;

namespace DotLearning.Tests.NeuralNetworks.MatrixBased
{
    public class LayerTests
    {
        [Fact]
        public void Constructor_ThrowsIfParametersAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Layer(null, new Vector(1), x => x));
            Assert.Throws<ArgumentNullException>(() => new Layer(new Matrix(1, 1), null, x => x));
            Assert.Throws<ArgumentNullException>(() => new Layer(new Matrix(1, 1), new Vector(1), null));
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(3, 2, 2)]
        public void Constructor_ValidatesWeightsAndBiasesSizes(int weightRows, int weightColumns, int biasesLength)
        {
            var weights = new Matrix(weightRows, weightColumns);
            var biases = new Vector(biasesLength);

            Assert.Throws<ArgumentException>(() => new Layer(weights, biases, x => x));
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(10, 3, 10)]
        public void Constructor_Accepts_ValidParameters(int weightRows, int weightColumns, int biasesLength)
        {
            var weights = new Matrix(weightRows, weightColumns);
            var biases = new Vector(biasesLength);

            var layer = new Layer(weights, biases, x => x);

            Assert.NotNull(layer);
        }

        [Fact]
        public void Calculate_ThrowsIfInputIsNull()
        {
            var layer = new Layer(new Matrix(1, 1), new Vector(1), x => x);
            Assert.Throws<ArgumentNullException>(() => layer.Calculate(null));
        }
        
        [Theory]
        [MemberData(nameof(CalculateWeightsExamples))]
        public void Calculate_AppliesWeights(Matrix weights, Vector input, Vector expected)
        {
            var biases = new Vector(weights.Rows);
            var layer = new Layer(weights, biases, x => x);
            
            var result = layer.Calculate(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(CalculateBiasesExamples))]
        public void Calculate_AppliesBiases(Vector biases, Vector input, Vector expected)
        {
            var weights = new Matrix(biases.Count, input.Count); // all zero so results are bias only
            var layer = new Layer(weights, biases, x => x);

            var result = layer.Calculate(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(CalculateActivationFunctionExamples))]
        public void Calculate_AppliesActivationFunction(Func<double, double> activationFunction, Vector input, Vector expected)
        {
            var weights = new Matrix(expected.Count, input.Count);
            for (var i = 0; i < weights.Rows; i++)
                for (var j = 0; j < weights.Columns; j++)
                    weights[i, j] = 1d;

            var biases = new Vector(expected.Count);
            var layer = new Layer(weights, biases, activationFunction);

            var result = layer.Calculate(input);

            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> CalculateWeightsExamples()
        {
            yield return new[]
            {
                new Matrix(new double[,]
                {
                    { 2 }
                }),
                new Vector(new[]{ 3d}),
                new Vector(new[]{ 6d})
            };

            yield return new[]
            {
                new Matrix(new double[,]
                {
                    { 1, 2 },
                    { 3, 4 }
                }),
                new Vector(new[]{ 1d, 1d}),
                new Vector(new[]{ 3d, 7d })
            };
        }

        public static IEnumerable<object[]> CalculateBiasesExamples()
        {
            yield return new[]
            {
                new Vector(new[] { 1d }),
                new Vector(new[] { 0d }),
                new Vector(new[] { 1d })
            };

            yield return new[]
            {
                new Vector(new[] { 1d, 2d, 3d }),
                new Vector(new[] { 0d, 0d }),
                new Vector(new[] { 1d, 2d, 3d })
            };
        }

        public static IEnumerable<object[]> CalculateActivationFunctionExamples()
        {
            Func<double, double> @double = x => 2d * x;

            yield return new object[]
            {
                @double,
                new Vector(new[] { 1d, 1d, 1d }),
                new Vector(new[] { 6d, 6d })
            };
        }
    }
}
