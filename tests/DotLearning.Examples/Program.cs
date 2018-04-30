using System;
using DotLearning.Examples.Iris;

namespace DotLearning.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Log("Iris data set - simple network");
            var example = new SimpleIris();
            example.LoadTrainingData();
            example.CreateNetwork();
            Log("Starting training");
            example.Train(1000, 100, 0.5d);
            Log("Training complete");
            example.LoadTestData();
            Log("Testing network");
            example.Test();
            example.ShowResults();

            Log("Iris data set - matrix network");
            var example2 = new MatrixIris();
            example.LoadTrainingData();
            example.CreateNetwork();
            Log("Starting training");
            example.Train(1000, 100, 0.5d);
            Log("Training complete");
            example.LoadTestData();
            Log("Testing network");
            example.Test();
            example.ShowResults();
        }

        private static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss.ff]} {message}");
        }
    }
}
