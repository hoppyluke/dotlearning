using System;

namespace DotLearning.Mathematics
{
    /// <summary>
    /// Functional equivalents of numerical operators.
    /// </summary>
    public static class FunctionalOperator
    {
        /// <summary>
        /// Addition operator as a function.
        /// </summary>
        public static Func<double, double, double> Add => (x, y) => x + y;

        /// <summary>
        /// Subtraction operator as a function.
        /// </summary>
        public static Func<double, double, double> Subtract => (x, y) => x - y;

        /// <summary>
        /// Multiplication operator as a function.
        /// </summary>
        public static Func<double, double, double> Multiply => (x, y) => x * y;
    }
}
