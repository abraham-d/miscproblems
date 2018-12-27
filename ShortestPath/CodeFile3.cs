using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPath
{
    partial class Program
    {
        static string Encryption(string s)
        {
            var ret = s.Where(c => !Char.IsWhiteSpace(c));
            int len = ret.Count();
            double sqrt = Math.Sqrt(len);
            int low = (int)Math.Floor(sqrt);
            int high = (int)Math.Ceiling(sqrt);
            var lines = Split(ret, len, high).ToArray();
            var r = new List<string>();
            for(int i = 0; i < high; i++)
            {
                r.Add(string.Concat(lines.Where(a => a.Length > i).Select(a => a[i]).ToArray()));
            }
            return string.Join(" ", r);
            //return null;
        }

        private static IEnumerable<char[]> Split(IEnumerable<char> ret, int len, int high)
        {
            for(int i = 0; i < len; i += high)
            {
                yield return ret.Skip(i).Take(high).ToArray();
            }
        }
    }
}