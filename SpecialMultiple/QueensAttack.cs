using System;
using System.IO;
using System.Linq;

namespace SpecialMultiple
{
    partial class Program
    {
        static void QueenTest()
        {
            string filename = @"C:\projects\vs2017\SpecialMultiple\q_input.txt";
            using (StreamReader fileReader = new StreamReader(filename))
            {


                //string line1 = "5 3";
                //string line2 = "4 3";
                //string[] obts = { "5 5", "4 2", "2 3" };
                //string line1 = "8 0";
                //string line2 = "5 4";
                //string line1 = "4 0";
                //string line2 = "4 4"; //9
                //string[] obts = { "5 5", "4 2", "2 3" };

                string[] nk = fileReader.ReadLine().Split(' ');
                int n = Convert.ToInt32(nk[0]);
                int k = Convert.ToInt32(nk[1]);
                string[] r_qC_q = fileReader.ReadLine().Split(' ');
                int r_q = Convert.ToInt32(r_qC_q[0]);
                int c_q = Convert.ToInt32(r_qC_q[1]);
                int[][] obstacles = new int[k][];
                for (int i = 0; i < k; i++)
                {
                    obstacles[i] = Array.ConvertAll(fileReader.ReadLine().Split(' '), obstaclesTemp => Convert.ToInt32(obstaclesTemp));
                }
                int result = queensAttack(n, k, r_q, c_q, obstacles);
                Console.WriteLine(result);
            }
        }

        private static int queensAttack(int n, int k, int r_q, int c_q, int[][] obstacles)
        {
            int[][] limits =
            {
                new int[]{r_q, n },
                new int[]{r_q + Math.Min(n - r_q, n - c_q), c_q + Math.Min(n - r_q, n - c_q) },
                new int[]{n, c_q },
                new int[]{r_q + Math.Min(c_q - 1, n - r_q), c_q - Math.Min(n - r_q, c_q - 1) },
                new int[]{r_q, 1 },
                new int[]{r_q - Math.Min(r_q - 1, c_q - 1), c_q - Math.Min(r_q - 1, c_q - 1) },
                new int[]{1, c_q },
                new int[]{r_q - Math.Min(n - c_q, r_q - 1), c_q + Math.Min(n - c_q, r_q - 1) }
            };

            foreach(var ob in obstacles)
            {
                if (ob[0] == r_q)
                {
                    if (ob[1] > c_q)
                    {
                        if (ob[1] < limits[0][1]) { limits[0][1] = ob[1]; }
                    }
                    else
                    {
                        if (ob[1] > limits[4][1]) { limits[4][1] = ob[1]; }
                    }
                }
                else if (ob[1] == c_q)
                {
                    if (ob[0] > r_q)
                    {
                        if (ob[0] < limits[2][0]) { limits[2][0] = ob[0]; }
                    }
                    else
                    {
                        if (ob[0] > limits[6][0]) { limits[6][0] = ob[0]; }
                    }
                }
                else if (Math.Abs(ob[0] - r_q) == Math.Abs(ob[1] - c_q))
                {
                    if (ob[0] > r_q && ob[1] > c_q)
                    {
                        if (ob[0] < limits[1][0]) { limits[1][0] = ob[0]; limits[1][1] = ob[1]; }
                    }
                    else if (ob[0] > r_q && ob[1] < c_q)
                    {
                        if (ob[0] < limits[3][0]) { limits[3][0] = ob[0]; limits[3][1] = ob[1]; }
                    }
                    else if (ob[0] < r_q && ob[1] < c_q)
                    {
                        if (ob[0] > limits[5][0]) { limits[5][0] = ob[0]; limits[5][1] = ob[1]; }
                    }
                    else if (ob[0] < r_q && ob[1] > c_q)
                    {
                        if (ob[0] > limits[7][0]) { limits[7][0] = ob[0]; limits[7][1] = ob[1]; }
                    }
                }
            }
            int a = limits[0][1] - c_q == 0 ? 0 : (limits[0][1] == n ? limits[0][1] - c_q : limits[0][1] - c_q - 1);
            int b = c_q - limits[4][1] == 0 ? 0 : (limits[4][1] == 1 ? c_q - limits[4][1] : c_q - limits[4][1] - 1);
            int c = limits[2][0] - r_q == 0 ? 0 : (limits[2][0] == n ? limits[2][0] - r_q : limits[2][0] - r_q - 1);
            int d = r_q - limits[6][0] == 0 ? 0 : (limits[6][0] == 1 ? r_q - limits[6][0] : r_q - limits[6][0] - 1);

            int e = limits[1][0] - r_q == 0 ? 0 : ((limits[1][0] == n || limits[1][1] == n) ? limits[1][0] - r_q : limits[1][0] - r_q - 1);
            int f = c_q - limits[5][1] == 0 ? 0 : ((limits[5][0] == 1 || limits[5][1] == 1) ? c_q - limits[5][1] : c_q - limits[5][1] - 1);
            int g = limits[3][0] - r_q == 0 ? 0 : ((limits[3][0] == n || limits[3][1] == 1) ? limits[3][0] - r_q : limits[3][0] - r_q - 1);
            int h = r_q - limits[7][0] == 0 ? 0 : ((limits[7][0] == 1 || limits[7][1] == n) ? r_q - limits[7][0] : r_q - limits[7][0] - 1);
            return a + b + c + d + e + f + g + h;
        }
    }
}