using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace primenumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            ProcessPrimeAsync();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static async void ProcessPrimeAsync()
        {
            var sw = new Stopwatch();
            sw.Start();
            const int numThreads = 10;
            Task<List<int>>[] primes = new Task<List<int>>[numThreads];
            for(int i = 0; i < numThreads; i++)
            {
                primes[i] = GetPrimeNumbersAsync(i == 0 ? 2 : i * 10000000 + 1, (i + 1) * 10000000);
            }

            var results = await Task.WhenAll(primes);

            Console.WriteLine("Primes found: {0}\nTotal Time: {1}", results.Sum(p => p.Count), sw.ElapsedMilliseconds);
        }

        private static async Task<List<int>> GetPrimeNumbersAsync(int minimum,int maximum)
        {
            int count = maximum - minimum + 1;
            return await Task.Factory.StartNew(() => Enumerable.Range(minimum, count).Where(IsPrime).ToList());
        }

        static bool IsPrime(int p)
        {
            if (p % 2 == 0) return p == 2;
            int topLimit = (int)Math.Sqrt(p);
            for(int i = 3; i <= topLimit; i += 2)
            {
                if (p % i == 0) return false;
            }

            return true;
        }
    }
}
