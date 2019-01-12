using System;

namespace ShortestPath
{
    partial class Program
    {
        static void CircularBuffertest()
        {
            int[] a = { 1, 2, 3 };
            int k = 2;
            int[] queries = { 0, 1, 2 };
            foreach(var indx in queries)
            {
                var i = (indx + a.Length - k % a.Length) % a.Length;
                Console.WriteLine($"Item : {a[i]}");
            }
        }
    }
}