using System.Collections.Generic;
using DotLearning.Mathematics.LinearAlgebra;
using DotLearning.NeuralNetworks.MatrixBased;

namespace DotLearning.Tests.NeuralNetworks.MatrixBased
{
    internal class FakeLayerFactory
    {
        private readonly List<FakeLayer> _layers = new List<FakeLayer>();

        public IReadOnlyList<FakeLayer> CreatedLayers => _layers;
        
        public Layer Create(int numberOfNeurons, int numberOfInputs)
        {
            var layer = new FakeLayer(new Matrix(numberOfNeurons, numberOfInputs), new Vector(numberOfNeurons), x => x, x => x);
            _layers.Add(layer);
            return layer;
        }
    }
}
