using DotLearning.Mathematics;

namespace DotLearning.NeuralNetworks.Simple
{
    internal static class NeuronFactory
    {
        public static ICalculatingNeuron SigmoidNeuron(INeuron[] inputs)
            => new Neuron(inputs, new double[inputs.Length], 0d, MathematicalFunctions.Sigmoid);
    }
}
