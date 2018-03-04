namespace DotLearning.NeuralNetworks.Simple
{
    /// <summary>
    /// A neuron in an input layer which simply returns the input value assigned.
    /// </summary>
    public class InputNeuron : INeuron
    {
        /// <summary>
        /// Gets or sets the input value of this neuron.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets the input value assigned to this neuron.
        /// </summary>
        public double Output => Value;
    }
}
