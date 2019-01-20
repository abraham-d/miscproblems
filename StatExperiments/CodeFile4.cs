using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StatExperiments
{
    public partial class Program
    {
        public static void Prob04()
        {
            string fileName = @"C:\projects\vs2017\Data\prob04_01.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                int n = Convert.ToInt32(file.ReadLine());
                var d = Array.ConvertAll(file.ReadLine().Split(' '), temp => Convert.ToInt32(temp));
                var freq = Array.ConvertAll(file.ReadLine().Split(' '), temp => Convert.ToInt32(temp));
                var data = d.Zip(freq, (x, y) => Enumerable.Repeat(x, y))
                            .SelectMany(x => x)
                            .OrderBy(x => x);
                double q2 = FindMedian(data);
                int lower = data.Count() / 2;
                int upper = data.Count() % 2 == 0 ? data.Count() / 2 : data.Count() / 2 + 1;

                double q1 = FindMedian(data.Take(lower));
                double q3 = FindMedian(data.Skip(upper));
                //double q1 = FindMedian(data.Where((x, i) => i < indx));
                //double q3 = FindMedian(data.Where((x, i) => i > indx));
                Console.WriteLine(q3 - q1);
            }
        }

        private static double FindMedian(IEnumerable<int> data)
        {
            double median;
            int count = data.Count();
            if (count % 2 == 0)
            {
                int mid = count / 2;
                median = (data.ElementAt(mid) + data.ElementAt(mid - 1)) / 2.0;
            }
            else
            {
                median = data.ElementAt(count / 2);
            }
            return median;
        }
    }
}
