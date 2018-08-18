using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;

namespace DotLearning.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 0 && args[0].ToLowerInvariant() == "manual")
            {
                ManualBenchmarks();
                return;
            }

            BenchmarkRunner.Run<NeuralNetworks>();
        }

        private static void ManualBenchmarks()
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
