using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;

namespace ShortestPath
{
    partial class Program
    {
        static void EugeneTest()
        {
            string filename = @"..\..\euinput01.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                int t = Convert.ToInt32(file.ReadLine());
                var sw = new Stopwatch();
                for (int tItr = 0; tItr < t; tItr++)
                {
                    sw.Start();
                    string[] anm = file.ReadLine().Split(' ');
                    int a = Convert.ToInt32(anm[0]);
                    long n = Convert.ToInt64(anm[1]);
                    int m = Convert.ToInt32(anm[2]);
                    int result = Solve(a, n, m);
                    sw.Stop();
                    Console.WriteLine($"Result : {result}  Elapsed : {sw.Elapsed}");
                    Console.WriteLine($"Solve1 : {Solve1(a,n,m)}");
                }
            }
        }
        static void ETest()
        {
            //var sw = new Stopwatch();
            //sw.Start();
            //var str = new StringBuilder().Insert(0, "110", 200000).ToString();
            //var num = BigInteger.Parse(str);
            //sw.Stop();
            //Console.WriteLine($"Elapsed : {sw.Elapsed}");

            //for (int i = 1; i < 10; i++)
            //{
            //    var str = new StringBuilder().Insert(0, "12", i).ToString();
            //    var num = BigInteger.Parse(str);
            //    BigInteger mod = new BigInteger();
            //    BigInteger.DivRem(num, 17, out mod);
            //    Console.WriteLine($"Mod : {(int)mod}");
            //}

            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            ////for(long i=0;i< 883942356640; i++) { }
            //int a = 110;
            //long n = 883942356640;
            //int m = 570992238;
            //var x = BigInteger.ModPow(1000, n + 1, m);
            //var y = BigInteger.Multiply(x, 110);
            //var mod = new BigInteger();
            //BigInteger.DivRem(y, m, out mod);
            //var z = BigInteger.Subtract(mod, 110);
            //var final = new BigInteger();
            //BigInteger.DivRem(z, m, out final);
            //var f1 = (long)final;
            //var f2 = f1 / 999;
            //sw.Stop();
            //Console.WriteLine($"Elapsed : {sw.Elapsed}");

            //Console.WriteLine($"Inverse 999-956577277 {Inverse(3, 956577277)}");
            //Console.WriteLine($"Inverse 999-570992238 {Inverse(3, 570992238)}");
            Test1(883942356640);
        }
        static void Test1(long num)
        {
            var x = BigInteger.Pow(1000, 2130000000);
            while (num > int.MaxValue)
            {
                var j = num / int.MaxValue;
                Console.WriteLine($"{j}");
                num = num % int.MaxValue;
            }
            Console.WriteLine($"{num}");
        }
        static void Test(long num)
        {
            int max = int.MaxValue;
            while (true)
            {
                if (num > max)
                {
                    Console.Write($"{max} + ");
                    num -= max;
                }
                else
                {
                    Console.WriteLine($"{num}");
                    break;
                }
            }
        }
        private static int Solve1(int a, long n, int m)
        {
            int numDigits = NumDigits(a);
            int amodm = a % m;
            BigInteger mode = new BigInteger();
            long value = (long)Math.Pow(10, numDigits);
            var x = BigInteger.ModPow(value, n, m);
            var y = BigInteger.Multiply(x, amodm);
            BigInteger.DivRem(y, m, out mode);
            var z = BigInteger.Subtract(mode, amodm);
            BigInteger.DivRem(z, m, out mode);
            return (int)mode;
        }
        static long Inverse(int numDigits, int m)
        {
            int num = (int)Math.Pow(10, numDigits) - 1;
            long max = Math.Max(m, num);
            int min = Math.Min(m, num);
            int inverse = 1;
            while ((max * inverse) % min != 1)
            {
                inverse++;
            }
            return inverse;
        }
        private static int Solve(int a, long n, int m)
        {
            Dictionary<long, long> lookup = new Dictionary<long, long>();

            int numDigits = NumDigits(a);
            int amodm = a % m;
            long mod = 0;
            for(long i = 0; i < n; i++)
            {                
                long multmod = (long)BigInteger.ModPow(10, i * numDigits, m);
                long tmp = (amodm * multmod) % m;
                mod = (mod + tmp) % m;

                if (lookup.ContainsValue(mod))
                {
                    if (i <= n - 1)
                    {
                        mod = lookup[n % lookup.Count == 0 ? lookup.Count - 1 : (n % lookup.Count) - 1];
                    }
                    break;
                }
                lookup.Add(i, mod);
            }
            
            return (int)mod;
        }

        private static int NumDigits(int a)
        {
            int ret = 0;
            while (a > 0)
            {
                ret++;
                a /= 10;
            }
            return ret;
        }
    }
}