using System;
using System.Linq;
using System.Numerics;

namespace MiscExperiments
{
    partial class Program
    {
        static void PrimeCountTest()
        {
            Console.WriteLine(PrimeCount(1000000000000000000));
            Console.WriteLine(PrimeCount(614889782588491410));
        }
        static int PrimeCount(long n)
        {
            var primeTable = Enumerable.Range(1, 200).Where(x => IsPrime(x));
            int cnt = 0;
            BigInteger mult = 1;
            foreach(var i in primeTable)
            {
                mult *= i;
                if (mult <= n)
                    cnt++;
                else break;
                //if (mult == n) break;
                Console.WriteLine($"{cnt} {i} {mult}");
            }
            Console.WriteLine($"n : {n}  mult: {mult}");
            return cnt;
        }

        private static bool IsPrime(int x)
        {
            if (x < 2) return false;
            for (int i = 2; i * i <= x; i++)
            {
                if (x % i == 0) return false;
            }
            return true;
        }
    }
}