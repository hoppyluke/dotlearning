using System.Collections.Generic;
using DotLearning.NeuralNetworks.Simple;

namespace DotLearning.Tests.NeuralNetworks.Simple
{
    internal class FakeNeuron : ICalculatingNeuron
    {
        public bool HasCalculated { get; private set; }
        public INeuron[] Inputs { get; set; }
        public double Output { get; set; }

        public double[] ActualInput { get; private set; }

        public void Calculate()
        {
            ActualInput = new double[Inputs.Length];
            for (var i = 0; i < Inputs.Length; i++)
                ActualInput[i] = Inputs[i].Output;

            HasCalculated = true;
        }
    }

    internal class FakeNeuronFactory
    {
        private List<FakeNeuron> _createdNeurons = new List<FakeNeuron>();

        public IReadOnlyList<FakeNeuron> Neurons => _createdNeurons;

        public int CreatedNeurons => _createdNeurons.Count;

        public ICalculatingNeuron Create(INeuron[] inputs)
        {
            var neuron = new FakeNeuron { Inputs = inputs };
            _createdNeurons.Add(neuron);
            return neuron;
        }
    } 
}