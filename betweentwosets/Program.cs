using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace betweentwosets
{
    class Program
    {
        static void Main(string[] args)
        {
            Summation();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void Summation()
        {
            var ar = Enumerable.Range(1, 2);
            int i = ar.Aggregate(0, (total, next) => total + 2 * next - 1);
            Console.WriteLine(i);
        }

        //static void Main(string[] args)
        //{
        //    int[] scores = { 10, 5, 20, 20, 4, 5, 2, 25, 1 };
        //    var max = scores.Zip(scores.Skip(0), (x, y) => x > y ? x : y).ToList();

        //    var n = scores.TakeWhile((x, indx) => (indx == 0 ? true : x > scores[indx - 1]));
        //    var m = scores.Select((x, indx) => (indx == 0 ? x : (x > scores[indx - 1] ? x : scores[indx - 1]))).ToList();

        //    int cma = 0, cmi = 0, ma = scores[0], mi = scores[0];
        //    for(int i = 1; i < scores.Length; i++)
        //    {
        //        if (scores[i] > ma)
        //        {
        //            ma = scores[i];
        //            cma++;
        //        }
        //        else if (scores[i] < mi)
        //        {
        //            mi = scores[i];
        //            cmi++;
        //        }
        //    }
        //    Console.WriteLine("{0} -- {1}", cma, cmi);

        //    //var fac = Enumerable.Range(1, 24).Where(x => 24 % x == 0 && x > 4).ToArray();
        //    //int[] num = { 16, 32, 96 };
        //    //int[] n = { 2 };
        //    //int[] b = { 72, 48 };
        //    ////Console.WriteLine(GCD(2, 4));
        //    //Console.WriteLine(num.Aggregate(GCD));
        //    //Console.WriteLine(n.Aggregate(LCM));
        //    //Console.WriteLine(b.Aggregate(GCD));
        //    Console.WriteLine("Press any key to continue...");
        //    Console.ReadKey();
        //}

        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static int LCM(int a, int b)
        {
            return a * b / GCD(a, b);
        }
    }
}
