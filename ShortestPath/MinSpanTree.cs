using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace ShortestPath
{
    partial class Program
    {
        static void MSTTest()
        {
            string filename = @"..\..\hcinput07.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                Stopwatch sw = new Stopwatch();
                string[] nm = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nm[0]);
                int m = Convert.ToInt32(nm[1]);

                int[][] roads = new int[m][];
                for (int roadsRowItr = 0; roadsRowItr < m; roadsRowItr++)
                {
                    roads[roadsRowItr] = Array.ConvertAll(file.ReadLine().Split(' '), roadsTemp => Convert.ToInt32(roadsTemp));
                }

                //string result = RoadsInHackerland(n, roads);
                //foreach (var road in roads)
                //    Console.WriteLine(string.Join(" ", road));
                //Console.WriteLine("============");
                var sorted = roads.OrderBy(r => r[2]);
                foreach (var road in sorted)
                    Console.WriteLine(string.Join(" ", road));
                Console.WriteLine("============");
                var ret = RoadsInHackerland(n, roads);
                Console.WriteLine(ret);
            }
        }
        private static string RoadsInHackerland(int n, int[][] roads)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int longest = roads.Select(x => x[2]).Max();
            bool[] result = new bool[longest + 5];

            var sorted = roads.OrderBy(x => x[2]);
            List<int[]> minSpanTree = new List<int[]>();
            Dictionary<int, List<int>> trees = new Dictionary<int, List<int>>();
            foreach (var road in sorted)
            {
                AddRoad(trees, minSpanTree, roads, road);
            }
            //foreach (var road in minSpanTree)
            //    Console.WriteLine(string.Join(" ", road));

            //Console.WriteLine("==================");

            sw.Stop();
            var ts = sw.Elapsed;
            Console.WriteLine("RunTime " + ts);

            return AnotherWay(minSpanTree, longest);

            sw.Start();
            for (int i = 1; i <= n; i++)
            {
                for(int j = i + 1; j <= n; j++)
                {
                    var p = FindPath(minSpanTree, i, j);
                    //Console.WriteLine(string.Join(" ", p));
                    foreach(int d in p)
                    {
                        ConvertToBin(result, d);
                    }
                }
            }
            var reversed = result.Reverse();
            var ret = reversed.SkipWhile(x => x == false);

            sw.Stop();
            ts = sw.Elapsed;
            Console.WriteLine("RunTime " + ts);

            StringBuilder sb = new StringBuilder();
            foreach (var r in ret)
                sb.Append(r ? "1" : "0");
            return sb.ToString();
        }

        private static string AnotherWay(List<int[]> minSpanTree, int longest)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            bool[] result = new bool[longest + 5];
            foreach(var edge in minSpanTree)
            {
                int numLeft = Traversal(minSpanTree, edge[0], edge[1]).Count();
                int numRight = Traversal(minSpanTree, edge[1], edge[0]).Count();
                for(int i = 0; i < numLeft * numRight; i++)
                {
                    ConvertToBin(result, edge[2]);
                }
            }
            var reversed = result.Reverse().SkipWhile(x => x == false);
            StringBuilder sb = new StringBuilder();
            foreach (var r in reversed)
                sb.Append(r ? "1" : "0");
            //Console.WriteLine(sb.ToString());

            sw.Stop();
            var ts = sw.Elapsed;
            Console.WriteLine(ts);
            return sb.ToString();
        }

        private static void ConvertToBin(bool[] result, int d)
        {
            if (result[d])
            {
                result[d] = false;
                ConvertToBin(result, d + 1);
            }
            else
            {
                result[d] = true;
            }
            return;
        }

        private static void AddRoad(Dictionary<int, List<int>> trees, List<int[]> minSpanTree, int[][] roads, int[] road)
        {
            var min = Math.Min(road[0], road[1]);
            var max = Math.Max(road[0], road[1]);
            var minKey = trees.FirstOrDefault(x => x.Value.Contains(min));
            var maxKey = trees.FirstOrDefault(x => x.Value.Contains(max));

            if (minKey.Value == null && maxKey.Value == null)
            {
                minSpanTree.Add(road);
                trees.Add(min, new List<int> { road[0], road[1] });
            }
            else if (minKey.Value != null && maxKey.Value == null)
            {
                if (!minKey.Value.Contains(max))
                {
                    minSpanTree.Add(road);
                    trees[minKey.Key].Add(max);
                }
            }
            else if (minKey.Value == null && maxKey.Value != null)
            {
                if (!maxKey.Value.Contains(min))
                {
                    minSpanTree.Add(road);
                    trees[maxKey.Key].Add(min);
                }
            }
            else
            {
                if (minKey.Value.Contains(max) || maxKey.Value.Contains(min)) return;
                minSpanTree.Add(road);
                trees[minKey.Key].AddRange(new List<int>(trees[maxKey.Key]) { max });
                trees.Remove(maxKey.Key);
            }         
        }


        private static IEnumerable<int> Traversal(List<int[]> tree, int start, int other)
        {
            HashSet<int> visited = new HashSet<int>();
            Stack<int> stack = new Stack<int>();

            stack.Push(start);
            visited.Add(other);
            while (stack.Any())
            {
                var current = stack.Pop();
                if (!visited.Add(current)) continue;

                yield return current;

                var p1 = tree.Where(x => x[0] == current).Select(x => x[1]);
                var p2 = tree.Where(x => x[1] == current).Select(x => x[0]);
                var neighbours = p1.Concat(p2).Where(x => !visited.Contains(x));
                foreach (var neighbour in neighbours)
                {
                    stack.Push(neighbour);
                }
            }
        }

        private static IEnumerable<int> FindPath(List<int[]> tree, int start, int dest)
        {
            Stack<Tuple<Tuple<int, int>, List<Tuple<int, int>>>> stack = new Stack<Tuple<Tuple<int, int>, List<Tuple<int, int>>>>();
            List<int> visited = new List<int>();
            stack.Push(new Tuple<Tuple<int, int>, List<Tuple<int, int>>>(new Tuple<int, int>(start, 0), new List<Tuple<int, int>>()));
            while (stack.Any())
            {
                var current = stack.Pop();
                visited.Add(current.Item1.Item1);
                var path = current.Item2;
                path.Add(current.Item1);
                if (current.Item1.Item1 == dest)
                {
                    return path.Select(x => x.Item2).Skip(1).OrderBy(x => x);
                }
                var n1 = tree.Where(x => x[0] == current.Item1.Item1).Select(x =>new Tuple<int,int>( x[1],x[2]));
                var n2 = tree.Where(x => x[1] == current.Item1.Item1).Select(x =>new Tuple<int,int>( x[0],x[2]));
                var neighbours = n1.Concat(n2).Where(x => !visited.Contains(x.Item1));
                foreach(var neighbour in neighbours)
                {
                    stack.Push(new Tuple<Tuple<int, int>, List<Tuple<int, int>>>(neighbour, new List<Tuple<int, int>>(path)));
                }
            }

            return null;
        }

        static void RankTest()
        {
            var edge1 = Enumerable.Repeat(5, 6);
            var edge2 = Enumerable.Repeat(9, 6);
            var edge3 = Enumerable.Repeat(0, 12);
            var edge4 = Enumerable.Repeat(1, 6);
            var edge5 = Enumerable.Repeat(2, 6);
            var edge6 = Enumerable.Repeat(4, 6);
            bool[] result = new bool[12];
            foreach (var w in edge1) ConvertToBin(result, w);
            foreach (var w in edge2) ConvertToBin(result, w);
            foreach (var w in edge3) ConvertToBin(result, w);
            foreach (var w in edge4) ConvertToBin(result, w);
            foreach (var w in edge5) ConvertToBin(result, w);
            foreach (var w in edge6) ConvertToBin(result, w);
            var rev = result.Reverse().SkipWhile(x => x == false);
            StringBuilder sb = new StringBuilder();
            foreach (var r in rev)
                sb.Append(r ? "1" : "0");
            Console.WriteLine(sb.ToString());
        }
    }
}