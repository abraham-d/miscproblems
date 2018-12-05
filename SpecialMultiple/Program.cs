using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialMultiple
{
    partial class Program
    {
        static void Main(string[] args)
        {
            GraphTest();
            //HLRTest();
            //HackerLandRoad();
            //QueenTest();
            //ClimbingLadder();
            //NonDivisibleSubSet();
            //AllCombinationTest();
            //Console.WriteLine(Factorial(50));
            //Console.WriteLine("{0} -- {1}", 469, SpecialMultiple(469));
            //Console.WriteLine("{0} -- {1}", 7, SpecialMultiple(7));
            //Console.WriteLine("{0} -- {1}", 1, SpecialMultiple(1));
            //TestMethod();

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        
        private static void TestMethod()
        {
            var nums = FetchSeries(10);
            foreach (var n in nums)
                Console.Write("{0} ", n);
            Console.WriteLine();
        }

        private static IEnumerable<long> FetchSeries(int n)
        {
            if (n == 1)
            {
                yield return 9;
                yield break;
            }

            foreach (long j in FetchSeries(n - 1))
            {
                yield return j * 10;
                yield return j * 10 + 9;                
            }
            
        }

        private static int Pow(int n)
        {
            int x = 1;
            for(int i = 0; i < n; i++) { x *= 10; }
            return x;
        }

        private static void Combination(int n)
        {
            int[] d = { 0, 9 };
            int[] r = new int[n];
            Stack<int> stack = new Stack<int>();
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Pop();
            }
        }
    }
}
