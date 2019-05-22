using System;

namespace DotLearning.Benchmarks
{
    class DataGenerator
    {
        private static Random random = new Random();

        public static double[] Array(int length)
        {
            var a = new double[length];

            for (var i = 0; i < length; i++)
                a[i] = random.NextDouble();

            return a;
        }

        public static double[,] Matrix(int rows, int columns)
        {
            var m = new double[rows, columns];

            for (var i = 0; i < rows; i++)
                for (var j = 0; j < columns; j++)
                    m[i, j] = random.NextDouble();

            return m;
        }
    }
}
