using System;
using System.Collections.Generic;
using System.IO;

namespace MiscExperiments
{
    partial class Program
    {
        static void CountingValleys()
        {
            string fileName = @"C:\projects\vs2017\Data\misc01_00.txt";
            using (StreamReader file = new StreamReader(fileName))
            {
                int n = Convert.ToInt32(file.ReadLine());
                string s = file.ReadLine();
                int c = Count(n, s);
                Console.WriteLine(c);
            }
        }

        private static int Count(int n, string s)
        {
            Dictionary<char, int> directions = new Dictionary<char, int>() { { 'U', 1 }, { 'D', -1 } };
            int position = 0;
            int numValies = 0;
            foreach(var step in s)
            {
                if (position == 0)
                {
                    if (position + directions[step] < 0) { numValies++; }
                }
                position += directions[step];
            }
            return numValies;
        }
    }
}