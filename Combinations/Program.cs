using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Combinations
{
    class Program
    {
        static void Main(string[] args)
        {
            char[] array = { 'A', 'B', 'C', 'D', 'E', 'F' };
            foreach(char[] c in Combinations(array, 4))
            {
                Console.WriteLine("{0}", string.Join(" ", c));
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static IEnumerable<int[]> Combinations(int m, int n)
        {
            int[] result = new int[m];
            Stack<int> stack = new Stack<int>(m);
            stack.Push(0);

            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();

                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index == m)
                    {
                        yield return (int[])result.Clone();
                        break;
                    }                    
                }
            }
        }
        public static IEnumerable<T[]> Combinations<T>(T[] array, int m)
        {
            if (array.Length<m || m < 1)
            {
                throw new ArgumentException("Parameter");
            }

            T[] result = new T[m];
            foreach(int[] j in Combinations(m, array.Length))
            {
                for (int i = 0; i < m; i++)
                {
                    result[i] = array[j[i]];
                }
                yield return result;
            }
        }
        
    }
}
