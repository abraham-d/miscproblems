using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecialMultiple
{
    partial class Program
    {
        static void CombinationTest()
        {
            char[] data = { 'A', 'B', 'C', 'D', 'E', 'F' };
            Console.WriteLine("Data : {0}", string.Join(" ", data));
            foreach(var d in Combination<char>(data, 1))
            {
                Console.WriteLine(string.Join(" ", d));
            }
        }

        private static IEnumerable<T[]> Combination<T>(T[] data, int c)
        {
            int n = data.Length;
            
            var pattern = FetchPattern(n, c);

            foreach(var p in pattern)
            {
                T[] comb = new T[c];
                int i = 0;
                for (int j = 0; j < n; j++)
                {
                    if (p[j] == '1') { comb[i++] = data[j]; }
                }
                yield return comb;
            }
        }

        private static IEnumerable<string> FetchPattern(int length, int c)
        {
            var r = Enumerable.Range(0, Pow(2, length) - 1);
            foreach(int i in r)
            {
                string s = Convert.ToString(i, 2).PadLeft(length, '0');
                if (s.Count(x => x == '1') == c)
                {
                    yield return s;
                }
            }
        }

        private static int Pow(int r, int n)
        {
            int k = 1;
            for(int i = 0; i < n; i++)
            {
                k *= 2;
            }
            return k;
        }
    }
}