using System;
using System.Linq;
using DotLearning.NeuralNetworks.Simple;
using Xunit;

namespace DotLearning.Tests.NeuralNetworks.Simple
{
    public class NeuralNetworkTests
    {
        [Fact]
        public void Constructor_Throws_IfNeuronFactoryIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NeuralNetwork(null, 1, 1));
        }

        [Fact]
        public void Constructor_Throws_IfLayerSizesAreNull()
        {
            Assert.Throws<ArgumentNullException>(() => new NeuralNetwork(input => new FakeNeuron(), null));
        }

        [Theory]
        [InlineData(new int[0])]
        [InlineData(new int[] { 1 })]
        public void Constructor_Throws_IfFewerThan2Layers(int[] layerSizes)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new NeuralNetwork(layerSizes));
        }

        [Fact]
        public void Constructor_CreatesNeuronsFromFactory_ForNonInputLayers()
        {
            var neuronFactory = new FakeNeuronFactory();
            
            var hiddenLayer1Size = 3;
            var hiddenLayer2Size = 2;
            var outputLayerSize = 4;

            var network = new NeuralNetwork(neuronFactory.Create, 1, hiddenLayer1Size, hiddenLayer2Size, outputLayerSize);

            Assert.Equal(hiddenLayer1Size + hiddenLayer2Size + outputLayerSize, neuronFactory.CreatedNeurons);
        }

        [Fact]
        public void Constructor_CreatesInputLayer()
        {
            var neuronFactory = new FakeNeuronFactory();

            var inputLayerSize = 5;
            
            var network = new NeuralNetwork(neuronFactory.Create, inputLayerSize, 1);

            var outputNeuron = neuronFactory.Neurons[0];
            Assert.Equal(inputLayerSize, outputNeuron.Inputs.Length);
            Assert.All(outputNeuron.Inputs, i => Assert.IsType<InputNeuron>(i));
        }

        [Fact]
        public void Constructor_ConnectsNeuronsToAllNeuronsInPreviousLayer()
        {
            var neuronFactory = new FakeNeuronFactory();

            var inputLayerSize = 5;
            var hiddenLayerSize = 3;
            var outputLayerSize = 2;

            var network = new NeuralNetwork(neuronFactory.Create, inputLayerSize, hiddenLayerSize, outputLayerSize);

            var hiddenLayer = neuronFactory.Neurons.Take(hiddenLayerSize);
            var outputLayer = neuronFactory.Neurons.Skip(hiddenLayerSize).Take(outputLayerSize);

            Assert.All(hiddenLayer, n => Assert.Equal(inputLayerSize, n.Inputs.Length));
            Assert.All(outputLayer, n => Assert.Equal(hiddenLayerSize, n.Inputs.Length));
        }

        [Fact]
        public void Calculate_Throws_IfInputsAreNull()
        {
            var network = new NeuralNetwork(1, 1);
            Assert.Throws<ArgumentNullException>(() => network.Calculate(null));
        }

        [Fact]
        public void Calculate_Throws_IfWrongNumberOfInputs()
        {
            var network = new NeuralNetwork(1, 1);
            Assert.Throws<ArgumentException>(() => network.Calculate(new double[] { 1, 2 }));
        }

        [Fact]
        public void Calculate_SetsInput()
        {
            var inputs = new double[] { 1, 2, 3 };
            var neuronFactory = new FakeNeuronFactory();
            var network = new NeuralNetwork(neuronFactory.Create, inputs.Length, 1);
            var outputNeuron = neuronFactory.Neurons[0];
            
            network.Calculate(inputs);

            Assert.Equal(inputs, outputNeuron.ActualInput);
        }

        [Fact]
        public void Calculate_CallsCalculateOnAllNonInputNeurons()
        {
            var neuronFactory = new FakeNeuronFactory();
            var network = new NeuralNetwork(neuronFactory.Create, 1, 2, 3, 4);
            
            network.Calculate(new[] { 1d });

            Assert.All(neuronFactory.Neurons, n => Assert.True(n.HasCalculated));
        }

        [Theory]
        [InlineData(new double[] { 1d })]
        [InlineData(new double[] { 1d, 2d, 3d })]
        public void Calculate_ReturnsOutput(double[] expectedOutput)
        {
            var neuronFactory = new FakeNeuronFactory();
            var network = new NeuralNetwork(neuronFactory.Create, 1, expectedOutput.Length);
            for (var i = 0; i < expectedOutput.Length; i++)
                neuronFactory.Neurons[i].Output = expectedOutput[i];

            var result = network.Calculate(new[] { 1d });

            Assert.Equal(expectedOutput, result);
        }
    }
}
