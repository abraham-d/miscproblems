using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecialMultiple
{
    partial class Program
    {
        //15 7
        //278 576 496 727 410 124 338 149 209 702 282 718 771 575 436
        //11
        //10 4
        //1 2 3 4 5 6 7 8 9 10
        //5
        public static void NonDivisibleSubSet()
        {
            //int[] data = { 278, 576, 496, 727, 410, 124, 338, 149, 209, 702, 282, 718, 771, 575, 436 };
            int[] data = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int k = 4;
            bool flag = true;
            bool evenflag = k % 2 == 0;
            int half = k / 2;
            var r = data.Select(x => x % k).GroupBy(x => x).Select(g => new { key = g.Key, cnt = g.Count() }).OrderByDescending(x => x.cnt);
       
            List<int> compli = new List<int>();
            int cnt = 0;
            foreach (var c in r)
            {
                if(evenflag && c.key == half)
                {
                    cnt += 1;
                    continue;
                }
                if (c.key == 0 && flag)
                {
                    cnt += 1;
                    flag = false;
                    continue;
                }
                if (compli.Any(x => x == c.key)) continue;
                cnt += c.cnt;
                compli.Add(k - c.key);
            }
            Console.WriteLine(string.Join(" ", r));
            Console.WriteLine(cnt);
        }
    }
}