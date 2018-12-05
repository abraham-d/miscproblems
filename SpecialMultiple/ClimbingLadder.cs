using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecialMultiple
{
    partial class Program
    {
        static void ClimbingLadder()
        {
            string sc = "100 90 90 80 75 60";//"100 100 50 40 40 20 10";
            string al = "70 102 103 104";//"5 25 50 120";
            //6 4 2 1
            int[] scores = Array.ConvertAll(sc.Split(' '), scoresTemp => Convert.ToInt32(scoresTemp));
            
            int[] alice = Array.ConvertAll(al.Split(' '), aliceTemp => Convert.ToInt32(aliceTemp));

            int[] result = ClimbingLeaderboard(scores, alice);
            //int[] result1 = ClimbingLeaderboard1(scores, alice);
            //int[] result2 = ClimbingLeaderboard2(scores, alice);
            int[] result3 = ClimbingLeaderboard3(scores, alice);
            Console.WriteLine(string.Join(" ", result));
            //Console.WriteLine(string.Join(" ", result1));
            //Console.WriteLine(string.Join(" ", result2));
            Console.WriteLine(string.Join(" ", result3));
        }

        private static int[] ClimbingLeaderboard3(int[] scores, int[] alice)
        {
            int[] result = new int[alice.Length];
            int rank = 1;
            int scindx = 0;
            int cur = scores[scindx];
            int indx = alice.Length - 1;
            //sc = "100 90 90 80 75 60"
            //al = "10 20 70 102 103 104"
            while (true)
            {
                if (scindx < scores.Length - 1)
                {
                    if (alice[indx] >= scores[scindx])
                    {
                        result[indx] = rank;
                        if (indx == 0) { return result; }
                        else { --indx; }
                        continue;
                    }
                    else
                    {
                        if (cur > scores[++scindx])
                        {
                            rank++;
                            cur = scores[scindx];
                        }
                        if (alice[indx] >= cur)
                        {
                            result[indx] = rank;
                            if (indx == 0) { return result; }
                            else { --indx; }
                            continue;
                        }
                    }
                    continue;
                }
                if (alice[indx] < cur) { rank++;cur = -1; }
                result[indx] = rank;
                if (indx == 0) { return result; }
                else { --indx; }
            }            
        }
        private static int[] ClimbingLeaderboard2(int[] scores, int[] alice)
        {
            int[] result = new int[alice.Length];
            var g = scores.GroupBy(x => x).Select(x=>x.Key);
            for(int i = 0; i < alice.Length; i++)
            {
                result[i] = g.Count(x => x > alice[i]) + 1;
            }
            return result;
        }
        private static int[] ClimbingLeaderboard1(int[] scores, int[] alice)
        {
            int[] result = new int[alice.Length];
            int j = alice.Length - 1;
            int rank = 1;
            int last = scores[0];
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] < last)
                {
                    rank++;
                    last = scores[i];
                }
                if (scores[i] <= alice[j])
                {
                    result[j--] = rank;
                }                
            }
            if (j >= 0)
            {
                do
                {
                    result[j] = rank + 1;
                } while (j-- > 0);
            }


            return result;
        }

        private static int[] ClimbingLeaderboard(int[] scores, int[] alice)
        {
            int[] ret = new int[alice.Length];
            var sortedSet = new SortedSet<int>(scores);
            int last = alice[0];
            bool inserted = false;
            for(int i = 0; i < alice.Length; i++)
            {
                if (inserted)
                {
                    sortedSet.Remove(last);
                    inserted = false;
                }
                if (!sortedSet.Any(x => x == last))
                {
                    sortedSet.Add(alice[i]);
                    last = alice[i];
                    inserted = true;
                }
                ret[i] = sortedSet.Count(x => x > alice[i]) + 1;
            }

            return ret;
        }
    }
}