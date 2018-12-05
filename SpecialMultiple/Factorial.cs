using System.Collections.Generic;
using System.Linq;

namespace SpecialMultiple
{
    partial class Program
    {
        static string Factorial(int n)
        {
            Queue<byte> q = new Queue<byte>();
            q.Enqueue(1);
            foreach (int i in Enumerable.Range(2, n - 1))
            {
                q = Mult(q, i);
            }

            string s = string.Join("", q.Reverse());
            return s;
        }

        static Queue<byte> Mult(Queue<byte> q, int n)
        {
            Queue<byte> r = new Queue<byte>();
            int k = 0;
            foreach (var i in q)
            {
                int j = k + i * n;
                r.Enqueue((byte)(j % 10));
                k = j / 10;
            }
            while (k > 0)
            {
                int m = k % 10;
                r.Enqueue((byte)m);
                k = k / 10;
            }
            return r;
        }
        static long SpecialMultiple(int n)
        {
            bool flag = true;
            int i = 1;
            while (flag)
            {
                var r = FetchSeries(i).Where(x => x % n == 0);
                i++;
                if (r.Count() > 0)
                    return r.Min();
            }
            return -1;
        }
    }
}
