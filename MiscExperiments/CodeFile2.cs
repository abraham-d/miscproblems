using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiscExperiments
{
    partial class Program
    {
        static void XorMatrix()
        {
            string fileName = @"C:\projects\vs2017\Data\misc_xor01_01.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                string[] nm = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nm[0]);
                long m = Convert.ToInt64(nm[1]);
                int[] first_row = Array.ConvertAll(file.ReadLine().Split(' '), first_rowTemp => Convert.ToInt32(first_rowTemp));
                int[] last_row = XorMatrix(m, first_row);
                Console.WriteLine(string.Join(" ", last_row));

                last_row = XorMatrix(m - 1, first_row);
                Console.WriteLine(string.Join(" ", last_row));

                last_row = XorMatrix(m + 1, first_row);
                Console.WriteLine(string.Join(" ", last_row));
            }
        }

        private static int[] XorMatrix(long m, int[] first_row)
        {
            int n = first_row.Length;
            int[] next_row = new int[n];
            Array.Copy(first_row, next_row, n);
            int indx = 0;
            var b = NumToBin(m - 1);
            //Console.WriteLine(string.Join("", b));
            foreach (int i in b)
            {
                if (i == 1)
                {
                    long p = 1L << indx;
                    next_row = Calculate(next_row, n, p);
                }

                indx++;
            }
            return next_row;
        }

        private static int[] Calculate(int[] first_row, int n, long p)
        {
            int[] next_row = new int[n];
            for (int j = 0; j < n; j++)
            {
                next_row[j] = first_row[j] ^ first_row[(j + p) % n];
            }
            return next_row;
        }

        static IEnumerable<int> NumToBin(long m)
        {
            while (m > 0)
            {
                yield return (int)(m % 2);
                m >>= 1;
            }
        }
    }
}
