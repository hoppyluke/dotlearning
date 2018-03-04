using System;

namespace DotLearning.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            Log("Iris data set");
            var example = new Iris();
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
