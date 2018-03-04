namespace DotLearning.NeuralNetworks.Simple
{
    /// <summary>
    /// A neuron in a network.
    /// </summary>
    public interface INeuron
    {
        /// <summary>
        /// Gets output/activation value of this neuron.
        /// </summary>
        double Output { get; }
    }
}
