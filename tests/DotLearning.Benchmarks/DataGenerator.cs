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
    }
}
