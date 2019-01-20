using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StatExperiments
{
    public partial class Program
    {
        public static void Prob05()
        {
            string fileName = @"C:\projects\vs2017\Data\prob05_00.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                int n = Convert.ToInt32(file.ReadLine());
                var d = Array.ConvertAll(file.ReadLine().Split(' '), temp => Convert.ToInt32(temp));
                var mu = (double)d.Sum() / n;
                var variance = d.Select(x => Math.Pow(x - mu, 2)).Sum() / n;
                var sigma = Math.Sqrt(variance);
                Console.WriteLine($"{sigma.ToString("F1")}");
            }
        }
    }
}