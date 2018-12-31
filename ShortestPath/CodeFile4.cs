using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPath
{
    partial class Program
    {
        static void LCSTest()
        {
            //char[] s = { 'A', 'B', 'A', 'B' };
            //char[] t = { 'B', 'A', 'B', 'A' };
            //char[] s = { 'H', 'A', 'R', 'R', 'Y' };
            //char[] t = { 'S', 'A', 'L', 'L', 'Y' };  

            //Console.WriteLine(string.Join(" ", LongestCommonSubstring(s, t)));
            //Console.WriteLine(LCSLen("HARRY".ToCharArray(), "SALLY".ToCharArray()));
            //Console.WriteLine(LCSLen("ABAB".ToCharArray(), "BABA".ToCharArray()));
            //Console.WriteLine(LCSLenNew("SHINCHAN".ToCharArray(), "NOHARAAA".ToCharArray()));
            //Console.WriteLine(LCSLen("AA".ToCharArray(), "BB".ToCharArray()));
            //string s = "ELGGYJWKTDHLXJRBJLRYEJWVSUFZKYHOIKBGTVUTTOCGMLEXWDSXEBKRZTQUVCJNGKKRMUUBACVOEQKBFFYBUQEMYNENKYYGUZSP";
            //string t = "FRVIFOVJYQLVZMFBNRUTIYFBMFFFRZVBYINXLDDSVMPWSQGJZYTKMZIPEGMVOUQBKYEWEYVOLSHCMHPAZYTENRNONTJWDANAMFRX";
            //27
            //WEWOUCUIDGCGTRMEZEPXZFEJWISRSBBSYXAYDFEJJDLEBVHHKS
            //FDAGCXGKCTKWNECHMRXZWMLRYUCOCZHJRRJBOAJOQJZZVUYXIC
            //15
            string s = "WEWOUCUIDGCGTRMEZEPXZFEJWISRSBBSYXAYDFEJJDLEBVHHKS";
            string t = "FDAGCXGKCTKWNECHMRXZWMLRYUCOCZHJRRJBOAJOQJZZVUYXIC";
            Console.WriteLine(LCSLenNew(s.ToCharArray(), t.ToCharArray()));
            //Console.WriteLine(LongestCommonSubstring(s.ToCharArray(), t.ToCharArray()));

            //for (int j = 0; j < s.Length; j++)
            //    //Console.WriteLine(s.Skip(j).ToArray());
            //    Console.WriteLine(LCSLen(s.Skip(j).ToArray(), t.ToCharArray()));

            //for (int j = 0; j < t.Length; j++)
            //    Console.WriteLine(LCSLen(t.Skip(j).ToArray(), s.ToCharArray()));
        }
        static List<char> Backtrack(int[,] c, char[] x, char[] y, int i, int j)
        {
            if (i == 0 || j == 0)
                return new List<char> { };
            if (x[i] == y[j])
            {
                var ret = Backtrack(c, x, y, i - 1, j - 1);
                ret.Add(x[i]);
                return ret;
            }
            if (c[i, j - 1] > c[i - 1, j])
                return Backtrack(c, x, y, i, j - 1);
            return Backtrack(c, x, y, i - 1, j);
        }
        static int LCSLenNew(char[] s, char[] t)
        {
            int[,] table = new int[s.Length + 1, t.Length + 1];
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < t.Length; j++)
                {
                    if (s[i] == t[j])
                    {
                        table[i + 1, j + 1] = table[i, j] + 1;
                    }
                    else
                    {
                        table[i + 1, j + 1] = Math.Max(table[i, j + 1], table[i + 1, j]);
                    }
                }
            }
            for (int r = 0; r <= table.GetUpperBound(0); r++)
            {
                for (int c = 0; c <= table.GetUpperBound(1); c++)
                    Console.Write("{0} ", table[r, c]);
                Console.WriteLine();
            }
            //Console.WriteLine(string.Join("", Backtrack(table, s, t, s.Length - 1, t.Length - 1)));
            return table[s.Length, t.Length];
        }
        private static int CountDiag(int[,] m)
        {
            int rows = m.GetUpperBound(0);
            int cols = m.GetUpperBound(1);
            int count = 0;
            for (int i = cols; i > 0; i--)
            {
                int k = 0;
                for (int j = 0; j <= Math.Min(cols - i, rows); j++)
                {
                    if (m[j, i + j] > 0) k++;
                }
                if (k > count) count = k;
            }
            for (int i = 0; i <= rows; i++)
            {
                int k = 0;
                for (int j = 0; j <= Math.Min(rows - i, cols); j++)
                {
                    if (m[i + j, j] > 0) k++;
                }
                if (k > count) count = k;
            }
            return count;
        }

        static char[] LongestCommonSubstring(char[] s, char[] t)
        {
            int[,] table = new int[s.Length + 1, t.Length + 1];
            int z = 0;
            List<char> ret = new List<char>();

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    if (s[i - 1] == t[j - 1])
                    {
                        if (i == 1 || j == 1) table[i, j] = 1;
                        else table[i, j] = table[i - 1, j - 1] + 1;

                        if (table[i, j] > z)
                        {
                            z = table[i, j];
                            ret = new List<char>();
                            for (int k = i - z + 1; k < i; k++) ret.Add(s[k]);
                        }
                        else if (table[i, j] == z)
                        {
                            for (int k = i - z + 1; k < i; k++)
                            {
                                ret.Add(s[k]);
                            }
                        }
                    }
                    else
                    {
                        table[i, j] = 0;
                    }
                }
            }
            //for (int r = 0; r <= table.GetUpperBound(0); r++)
            //{
            //    for (int c = 0; c <= table.GetUpperBound(1); c++)
            //        Console.Write("{0} ", table[r, c]);
            //    Console.WriteLine();
            //}

            return ret.ToArray();
        }
        /*
 function LCSubstr(S[1..r], T[1..n])
    L := array(1..r, 1..n)
    z := 0
    ret := {}
    for i := 1..r
        for j := 1..n
            if S[i] == T[j]
                if i == 1 or j == 1
                    L[i,j] := 1
                else
                    L[i,j] := L[i-1,j-1] + 1
                if L[i,j] > z
                    z := L[i,j]
                    ret := {S[i-z+1..i]}
                else
                if L[i,j] == z
                    ret := ret ∪ {S[i-z+1..i]}
            else
                L[i,j] := 0
    return ret
         * 
         */
    }
}