using System;
using System.IO;
using System.Linq;

namespace StatExperiments
{
    partial class Program
    {
        static void Prob01()
        {
            string fileName = @"C:\projects\vs2017\Data\prob01_00.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                int n = Convert.ToInt32(file.ReadLine());
                var data = Array.ConvertAll(file.ReadLine().Split(' '), temp => Convert.ToInt32(temp));
                double mean = Mean(data);
                double median = Median(data);
                double mode = Mode(data);
                Console.WriteLine(mean.ToString("F1"));                
                Console.WriteLine(median.ToString("F1"));
                Console.WriteLine(mode.ToString("F1"));
            }
        }

        private static double Mode(int[] data)
        {
            var mode = data.GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .ThenBy(x => x.Key)
                .First()
                .Key;
            return mode;
        }

        private static double Median(int[] data)
        {
            var sorted = data.OrderBy(x => x);
            double median = 0;
            if (sorted.Count() % 2 == 0)
            {
                int mid = sorted.Count() / 2;
                median = (double)(sorted.ElementAt(mid) + sorted.ElementAt(mid - 1)) / 2;
            }
            else
            {
                median = sorted.ElementAt(sorted.Count() / 2);
            }
            return median;
        }

        private static double Mean(int[] data)
        {
            return (double)data.Sum() / data.Length;
        }
    }
}