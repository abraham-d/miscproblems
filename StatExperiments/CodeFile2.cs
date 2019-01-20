using System;
using System.IO;
using System.Linq;

namespace StatExperiments
{
    partial class Program
    {
        static void Prob02()
        {
            string fileName = @"C:\projects\vs2017\Data\prob02_00.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                int n = Convert.ToInt32(file.ReadLine());
                var data = Array.ConvertAll(file.ReadLine().Split(' '), temp => Convert.ToInt32(temp));
                var weights = Array.ConvertAll(file.ReadLine().Split(' '), temp => Convert.ToInt32(temp));
                double weightedMean = WeightedMean(data, weights);
                Console.WriteLine(weightedMean.ToString("F1"));
            }
        }

        private static double WeightedMean(int[] data, int[] weights)
        {
            var dw = data.Zip(weights, (x, y) => new Tuple<int, int>(x * y, y));
            double sum = dw.Sum(x => x.Item1);
            var weight = dw.Sum(x => x.Item2);
            return sum / weight;
        }
    }
}