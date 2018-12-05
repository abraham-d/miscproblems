using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecialMultiple
{
    partial class Program
    {
        static void AllCombinationTest()
        {
            char[] data = { 'A', 'B', 'C', 'D', 'E' };
            foreach (var s in AllCombination(data))//.Where(x => x.Count() == 3))
            {
                Console.WriteLine(string.Join(" ", s));
            }
        }

        static IEnumerable<T[]> AllCombination<T>(T[] data)
        {
            int n = data.Length;
            //List<T> sub = new List<T>();
            
            for (int i = 0; i < n; i++)
            {
                //sub.Add(data[i]);
                var s = new List<T>();
                for (int j = i; j < n; j++)
                {
                    s.Add(data[j]);
                    yield return s.ToArray();
                }
            }
        }
    }
}

