using DotLearning.Mathematics;

namespace DotLearning.NeuralNetworks.Simple
{
    internal static class NeuronFactory
    {
        /// <summary>
        /// Creates a new sigmoid neuron with randomly initialised weights and bias in [-0.5, 0.5].
        /// </summary>
        /// <param name="inputs">Previous layer for this neuron.</param>
        /// <returns>Neuron.</returns>
        public static ICalculatingNeuron SigmoidNeuron(INeuron[] inputs)
        {
            var weights =  new double[inputs.Length];
            for (var i = 0; i < inputs.Length; i++)
                weights[i] = Initialiser.Weight();

            var bias = Initialiser.Bias();

            return new Neuron(inputs, weights, bias, MathematicalFunctions.Sigmoid, MathematicalFunctions.SigmoidPrime);
        }
    }
}
