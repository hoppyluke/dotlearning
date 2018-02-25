using Xunit;
using static DotLearning.Mathematics.MathematicalFunctions;

namespace DotLearning.Tests.Mathematics
{
    public class MathematicalFunctionsTests
    {
        [Theory]
        [InlineData(0d, 0.5d)]
        [InlineData(1d, 0.7311d)]
        public void Sigmoid_ReturnsCorrectValue(double z, double expected)
        {
            var result = Sigmoid(z);

            Assert.Equal(expected, result, 4);
        }

        [Theory]
        [InlineData(0d, 0.25d)]
        public void Sigmoid_PrimeReturnsCorrectValue(double z, double expected)
        {
            var result = SigmoidPrime(z);

            Assert.Equal(expected, result, 4);
        }
    }
}
