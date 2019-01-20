using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MiscExperiments
{
    partial class Program
    {
        static void PairsTest()
        {
            string fileName = @"C:\projects\vs2017\Data\misc_pairs_00.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                string[] nk = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nk[0]);
                int k = Convert.ToInt32(nk[1]);
                int[] arr = Array.ConvertAll(file.ReadLine().Split(' '), first_rowTemp => Convert.ToInt32(first_rowTemp));
                Stopwatch sw = new Stopwatch();
                sw.Start();
                int result = Pairs1(k, arr);
                sw.Stop();
                Console.WriteLine($"Pairs1 : {result} -- Elapsed Time : {sw.Elapsed}");
                sw.Restart();
                result = Pairs2(k, arr);
                sw.Stop();
                Console.WriteLine($"Pairs2 : {result} -- Elapsed Time : {sw.Elapsed}");
                sw.Restart();
                result = Pairs3(k, arr);
                sw.Stop();
                Console.WriteLine($"Pairs3 : {result} -- Elapsed Time : {sw.Elapsed}");
            }
        }

        private static int Pairs1(int k, int[] arr)
        {
            int cnt = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                for(int j = i; j < arr.Length; j++)
                {
                    if (Math.Abs(arr[i] - arr[j]) == k)
                    //if (arr[j] - k == arr[i])
                    {
                        cnt++;
                        //break;
                    }
                    
                }
            }
            return cnt;
        }

        static int Pairs2(int k, int[] arr)
        {
            var s = new HashSet<int>(arr);
            return s.Where(x => s.Contains(x + k)).Count();
            //return arr.Where(x => arr.Contains(x - k)).Count();
        }

        static int Pairs3(int k, int[] arr)
        {
            int i = 0, j = 1, cnt = 0, n = arr.Length;
            while (i < n)
            {
                int d = arr[j] - arr[i];
                if (d == k)
                {
                    cnt++;
                    i++;
                }
                else if (d > k) { j++; }
                else if (d < k) { i++; }
            }
            return cnt;
        }
    }
}