using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BenchMark
{
    public class BenchMark
    {
        string[] data = new string[10000];
        int c = 10;
        public BenchMark()
        {
            data = Enumerable.Range(100, 10000).Select(x => x.ToString()).ToArray();
        }

        [Benchmark]
        public void CombinationBitPatternTest()
        {
            var combs = CombinationBitPattern(data, c);
        }

        [Benchmark]
        public void CombinationTest()
        {
            var combs = Combination(data, c);
            //char[] data = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            //foreach (char[] d in Combination(data, 3))
            //{
            //    Console.WriteLine(string.Join(" ", d));
            //}
        }

        [Benchmark]
        public void CombMethod1()
        {
            var combs = Combinations(data, c);
            //char[] array = { 'A', 'B', 'C', 'D', 'E', 'F', 'G' };
            //foreach (char[] c in Combinations(array, 3))
            //{
            //    Console.WriteLine("{0}", string.Join(" ", c));
            //}
        }
        static IEnumerable<int[]> Combinations(int m, int n)
        {
            int[] result = new int[m];
            Stack<int> stack = new Stack<int>(m);
            stack.Push(0);

            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();

                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index == m)
                    {
                        yield return (int[])result.Clone();
                        break;
                    }
                }
            }
        }
        public static IEnumerable<T[]> Combinations<T>(T[] array, int m)
        {
            if (array.Length < m || m < 1)
            {
                throw new ArgumentException("Parameter");
            }

            T[] result = new T[m];
            foreach (int[] j in Combinations(m, array.Length))
            {
                for (int i = 0; i < m; i++)
                {
                    result[i] = array[j[i]];
                }
                yield return result;
            }
        }

        public static IEnumerable<T[]> Combination<T>(T[] data, int c)
        {
            int n = data.Length;

            var pattern = FetchPattern(n, c);

            foreach (var p in pattern)
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
            foreach (int i in r)
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
            for (int i = 0; i < n; i++)
            {
                k <<= 1;
            }
            return k;
        }

        //===================
        public static IEnumerable<T[]> CombinationBitPattern<T>(T[] data, int c)
        {
            int n = data.Length;

            var pattern = FetchPatternBit(n, c);

            foreach (var p in pattern)
            {
                T[] comb = new T[c];
                int i = 0;
                int bitpattern = p;
                for (int j = 0; j < n && bitpattern > 0; j++)
                {
                    if ((bitpattern & 1)==1)
                    {
                        comb[i++] = data[j];
                    }
                    bitpattern >>= 1;
                }
                yield return comb;
            }
        }
        private static IEnumerable<int> FetchPatternBit(int length, int c)
        {
            return Enumerable.Range(0, Pow(2, length) - 1).Where(x => CountSetBits(x) == c);
        }
        public static int CountSetBits(int n)
        {
            int c = 0;
            while (n > 0)
            {
                c += n & 1;
                n >>= 1;
            }
            return c;
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<BenchMark>();
            //char[] data = { 'A', 'B', 'C', 'D', 'E', 'F' };
            //foreach (char[] d in BenchMark.Combination(data, 4))
            //{
            //    Console.WriteLine(string.Join(" ", d));
            //}
            //Console.WriteLine("===============");
            //foreach (char[] d in BenchMark.CombinationBitPattern(data, 4))
            //{
            //    Console.WriteLine(string.Join(" ", d));
            //}
            //Console.WriteLine("===============");
            //foreach (char[] d in BenchMark.Combinations(data, 4))
            //{
            //    Console.WriteLine(string.Join(" ", d));
            //}
            //Console.WriteLine(BenchMark.CountSetBits(15));
            //Console.ReadKey();
        }
    }
}
