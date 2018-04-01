using System;
using DotLearning.Mathematics.LinearAlgebra;
using DotLearning.NeuralNetworks.MatrixBased;
using Xunit;

namespace DotLearning.Tests.NeuralNetworks.MatrixBased
{
    public class NeuralNetworkTests
    {
        [Fact]
        public void Constructor_ThrowsIfLayerSizesNull()
        {
            int[] layerSizes = null;
            Assert.Throws<ArgumentNullException>(() => new NeuralNetwork(layerSizes));
        }

        [Fact]
        public void Constructor_ThrowsIfFewerThan2Layers()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new NeuralNetwork(10));
        }

        [Fact]
        public void Constructor_ThrowsIfLayerFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NeuralNetwork(null, 1, 1));
        }

        [Fact]
        public void Constructor_CreatesLayerForAllNonInputLayers()
        {
            var layerSizes = new[] { 1, 1, 1 };
            var factory = new FakeLayerFactory();

            var network = new NeuralNetwork(factory.Create, layerSizes);

            Assert.Equal(layerSizes.Length - 1, factory.CreatedLayers.Count);
        }

        [Theory]
        [InlineData(new int[] { 1, 1 })]
        [InlineData(new int[] { 4, 10, 10, 3 })]
        public void Constructor_PassesCorrectNumberOfNeuronsAndInputsToLayers(int[] layerSizes)
        {
            var factory = new FakeLayerFactory();

            var network = new NeuralNetwork(factory.Create, layerSizes);

            for(var i = 1; i < layerSizes.Length; i++)
            {
                var expectedInputs = layerSizes[i - 1];
                var expectedNeurons = layerSizes[i];
                var layer = factory.CreatedLayers[i - 1];
                Assert.Equal(expectedNeurons, layer.Neurons);
                Assert.Equal(expectedInputs, layer.Inputs);
            }
        }

        [Fact]
        public void Calculate_ThrowsIfInputNull()
        {
            var network = new NeuralNetwork(4, 10, 3);

            Assert.Throws<ArgumentNullException>(() => network.Calculate(null));
        }

        [Fact]
        public void Calculate_ThrowsIfWrongNumberOfInputs()
        {
            var network = new NeuralNetwork(4, 3);
            var input = new Vector(new[] { 1d, 2d, 3d });

            Assert.Throws<ArgumentException>(() => network.Calculate(input));
        }

        [Fact]
        public void Calculate_CallsCalculatePerLayer()
        {
            var factory = new FakeLayerFactory();
            var network = new NeuralNetwork(factory.Create, 1, 1, 1);

            network.Calculate(new Vector(new double[] { 1 }));

            Assert.All(factory.CreatedLayers, l => Assert.Equal(1, l.TimesCalculated));
        }

        [Fact]
        public void Calculate_FeedsforwardActivations()
        {
            var factory = new FakeLayerFactory();
            var network = new NeuralNetwork(factory.Create, 1, 1, 1, 1);
            var input = new Vector(new[] { 1d });
            var activation1 = new Vector(new[] { 2d });
            var activation2 = new Vector(new[] { 3d });
            factory.CreatedLayers[0].OverrideOutput(activation1);
            factory.CreatedLayers[1].OverrideOutput(activation2);

            network.Calculate(input);

            Assert.Equal(input, factory.CreatedLayers[0].Input);
            Assert.Equal(activation1, factory.CreatedLayers[1].Input);
            Assert.Equal(activation2, factory.CreatedLayers[2].Input);
        }

        [Fact]
        public void Calculate_ReturnsFinalLayerOutput()
        {
            var factory = new FakeLayerFactory();
            var network = new NeuralNetwork(factory.Create, 1, 1, 1);
            var expectedOutput = new Vector(new[] { 1d, 2d, 3d });
            factory.CreatedLayers[factory.CreatedLayers.Count - 1].OverrideOutput(expectedOutput);

            var output = network.Calculate(new Vector(new[] { 1d }));

            Assert.Equal(expectedOutput, output);
        }
    }
}
