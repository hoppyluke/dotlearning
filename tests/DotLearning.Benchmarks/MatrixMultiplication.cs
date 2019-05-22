using BenchmarkDotNet.Attributes;
using DotLearning.Mathematics.LinearAlgebra;

namespace DotLearning.Benchmarks
{
    [ShortRunJob]
    public class MatrixMultiplication
    {
        private Matrix m, n;

        [Params(10, 100, 500, 1000, 2000)]
        public int Size { get; set; }

        [GlobalSetup]
        public void CreateMatrices()
        {
            m = new Matrix(DataGenerator.Matrix(Size, Size));
            n = new Matrix(DataGenerator.Matrix(Size, Size));
        }

        [Benchmark(Baseline = true)]
        public Matrix BasicMultiplication()
        {
            return m * n;
        }
    }
}
