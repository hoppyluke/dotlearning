namespace DotLearning.NeuralNetworks.Simple
{
    public class InputNeuron : INeuron
    {
        public double Value { get; set; }

        public double Output => Value;
    }
}
