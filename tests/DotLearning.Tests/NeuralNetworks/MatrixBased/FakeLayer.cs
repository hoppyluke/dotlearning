using System;
using DotLearning.Mathematics.LinearAlgebra;
using DotLearning.NeuralNetworks.MatrixBased;

namespace DotLearning.Tests.NeuralNetworks.MatrixBased
{
    internal class FakeLayer : Layer
    {
        public Matrix Weights { get; }
        public Vector Biases { get; }

        public int Neurons => Weights.Rows;
        public int Inputs => Weights.Columns;
        public int TimesCalculated { get; private set; }
        public Vector Input { get; private set; }

        private Vector _output;

        public FakeLayer(Matrix weights, Vector biases, Func<double, double> activationFunction, Func<double, double> activationFunctionDerivative)
            : base(weights, biases, activationFunction, activationFunctionDerivative)
        {
            Weights = weights;
            Biases = biases;
        }

        public override Vector Calculate(Vector input)
        {
            Input = input;
            TimesCalculated++;

            return _output ?? base.Calculate(input);
        }

        public void OverrideOutput(Vector output)
        {
            _output = output;
        }
    }
}
