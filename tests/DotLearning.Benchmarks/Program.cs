using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace DotLearning.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmark = args.Length == 0 ? "neuralnets" : args[0].ToLowerInvariant();

            switch (benchmark)
            {
                case "manualnets":
                    ManualNeuralNetworkBenchmarks();
                    return;
                case "neuralnets":
                    BenchmarkRunner.Run<NeuralNetworks>();
                    return;
                case "matrix":
                    BenchmarkRunner.Run<MatrixMultiplication>();
                    return;
                default:
                    Console.WriteLine("Unknown benchmark: " + benchmark);
                    return;
            }
        }

        private static void ManualNeuralNetworkBenchmarks()
        {
            var s = new Stopwatch();
            var b = new NeuralNetworks();

            Time(() => b.CreateTrainingData());
            Console.WriteLine($"Setup: {s.ElapsedMilliseconds}ms");

            Time(() => b.Simple());
            Console.WriteLine($"Simple network: {s.ElapsedMilliseconds}ms");

            Time(() => b.Matrix());
            Console.WriteLine($"Matrix: {s.ElapsedMilliseconds}ms");

            void Time(Action a)
            {
                s.Reset();
                s.Start();

                a();

                s.Stop();
            }
        }
    }
}
