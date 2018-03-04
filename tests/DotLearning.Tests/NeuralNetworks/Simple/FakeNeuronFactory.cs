using System.Collections.Generic;
using DotLearning.NeuralNetworks.Simple;

namespace DotLearning.Tests.NeuralNetworks.Simple
{
    internal class FakeNeuronFactory
    {
        private List<FakeNeuron> _createdNeurons = new List<FakeNeuron>();

        public IReadOnlyList<FakeNeuron> Neurons => _createdNeurons;

        public int CreatedNeurons => _createdNeurons.Count;

        public ICalculatingNeuron Create(INeuron[] inputs)
        {
            var neuron = new FakeNeuron
            {
                Inputs = inputs,
                Weights = new double[inputs.Length]
            };

            _createdNeurons.Add(neuron);

            return neuron;
        }
    }
}
