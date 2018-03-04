using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotLearning.Examples
{
    internal static class CsvDataLoader
    {
        /// <summary>
        /// Read numeric only data from a CSV file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="skipRows">Number of rows to skip (skipped rows can contain non-numeric data).</param>
        /// <returns>Array of lines, where each line is an array of columns.</returns>
        public static double[][] GetData(string path, int skipRows = 0)
        {
            IEnumerable<string> lines = File.ReadAllLines(path);

            if (skipRows > 0)
                lines = lines.Skip(skipRows);

            return lines.Select(l => l.Split(',').Select(d => double.Parse(d)).ToArray())
                .ToArray();
        }
    }
}
