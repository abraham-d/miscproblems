using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StatExperiments
{
    public partial class Program
    {
        public static void Prob03()
        {
            string fileName = @"C:\projects\vs2017\Data\prob03_00.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                int n = Convert.ToInt32(file.ReadLine());
                var data = Array.ConvertAll(file.ReadLine().Split(' '), temp => Convert.ToInt32(temp));
                var sorted = data.OrderBy(x => x);
                int q2 = Median(sorted);
                int q1 = Median(sorted.Where(x => x < q2));
                int q3 = Median(sorted.Where(x => x > q2));
                Console.WriteLine(q1);
                Console.WriteLine(q2);
                Console.WriteLine(q3);
            }
        }

        private static int Median(IEnumerable<int> data)
        {
            int median;
            int count = data.Count();
            if (count % 2 == 0)
            {
                int mid = count / 2;
                median = (data.ElementAt(mid) + data.ElementAt(mid - 1)) / 2;
            }
            else
            {
                median = data.ElementAt(count / 2);
            }
            return median;
        }
    }
}