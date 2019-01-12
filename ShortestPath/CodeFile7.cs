using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ShortestPath
{
    partial class Program
    {
        static void BigIntTest()
        {
            Dictionary<int, string> lookup = new Dictionary<int, string>();
            for (int i = 0; i <= byte.MaxValue; i++)
            {
                lookup.Add(i, Convert.ToString(i, 2).PadLeft(8, '0'));
            }
            var sw = new Stopwatch();
            var nums = new Int64[] { 8, 128, 256, 1024, 1025, 65536, Int64.MaxValue };
            foreach (var num in nums)
            {
                sw.Start();
                var str1 = ConvertToBin(num);
                sw.Stop();
                Console.WriteLine("{0} Time {1}", str1, sw.Elapsed);
                sw.Start();
                var str2 = Bignum(lookup, num);
                sw.Stop();
                Console.WriteLine("{0} Time {1}", str2, sw.Elapsed);
                Console.WriteLine();
            }
        }

        private static string Bignum(Dictionary<int, string> lookup, long num)
        {
            BigInteger n = new BigInteger(num);
            var bytes = n.ToByteArray();
            var sb = new StringBuilder();
            for (int ctr = bytes.GetUpperBound(0); ctr >= bytes.GetLowerBound(0); ctr--)
            {
                sb.Append(lookup[bytes[ctr]]);
            }
            return sb.ToString().TrimStart('0');            
        }

        private static string ConvertToBin(long num)
        {
            return Convert.ToString(num, 2);            
        }
        
    }
}