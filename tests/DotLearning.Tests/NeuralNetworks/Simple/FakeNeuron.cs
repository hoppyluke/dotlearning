using System;
using System.Collections.Generic;
using DotLearning.NeuralNetworks.Simple;

namespace DotLearning.Tests.NeuralNetworks.Simple
{
    internal class FakeNeuron : ICalculatingNeuron
    {
        double INeuron.Output => Output;

        double ICalculatingNeuron.Bias => 0d;
        IReadOnlyList<double> ICalculatingNeuron.Weights => Weights;
        double ICalculatingNeuron.Error => Error;
        
        public double Output { get; set; }

        public int TimesCalculated { get; private set; }
        public bool HasCalculated => TimesCalculated > 0;
        public INeuron[] Inputs { get; set; }        
        public double[] ReceivedInput { get; private set; }
        
        public int TimesCalculatedError { get; private set; }
        public bool HasCalculatedError => TimesCalculatedError > 0;
        public double Error { get; private set; }
        public double[] ErrorsReceivedFromNextLayer { get; private set; }
        public double[] WeightsReceivedFromNextLayer { get; private set; }
        public int TimesLearnt { get; private set; }
        public bool HasLearnt => TimesLearnt > 0;

        public double[] Weights { get; set; }

        void ICalculatingNeuron.Calculate()
        {
            ReceivedInput = new double[Inputs.Length];
            for (var i = 0; i < Inputs.Length; i++)
                ReceivedInput[i] = Inputs[i].Output;

            TimesCalculated++;
        }

        void ICalculatingNeuron.CalculateError(double expectedOutput, Func<double, double, double> costDerivative)
        {
            Error = costDerivative(expectedOutput, Output);
            TimesCalculatedError++;
        }

        void ICalculatingNeuron.CalculateError(double[] nextLayerErrors, double[] nextLayerWeights)
        {
            ErrorsReceivedFromNextLayer = nextLayerErrors;
            WeightsReceivedFromNextLayer = nextLayerWeights;
            TimesCalculatedError++;
        }
        
        void ICalculatingNeuron.Learn(double learningRate)
        {
            TimesLearnt++;
        }

        public void SetWeight(int totalWeights, int inputNeuronIndex, double weight)
        {
            if (Weights == null || Weights.Length < totalWeights)
                Weights = new double[totalWeights];

            Weights[inputNeuronIndex] = weight;
        }
    } 
}