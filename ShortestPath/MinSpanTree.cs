using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShortestPath
{
    partial class Program
    {
        static void MSTTest()
        {
            string filename = @"C:\projects\vs2017\ShortestPath\hcinput00.txt";
            using (StreamReader file = new StreamReader(filename))
            {
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
                RoadsInHackerland(n, roads);
            }
        }
        private static string RoadsInHackerland(int n, int[][] roads)
        {
            var sorted = roads.OrderBy(x => x[2]);
            List<int[]> minSpanTree = new List<int[]>();
            Dictionary<int, List<int>> trees = new Dictionary<int, List<int>>();
            foreach (var road in sorted)
            {
                AddRoad(trees, minSpanTree, roads, road);
            }
            foreach (var road in minSpanTree)
                Console.WriteLine(string.Join(" ", road));

            Console.WriteLine("==================");
            var p = FindPath(minSpanTree, 1, 5);
            Console.WriteLine(string.Join(" ", p));
            return "Not Implemented...";
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

        
        //private static IEnumerable<int> Traversal(List<int[]> tree, int start)
        //{
        //    HashSet<int> visited = new HashSet<int>();
        //    Stack<int> stack = new Stack<int>();

        //    stack.Push(start);
        //    while (stack.Any())
        //    {
        //        int current = stack.Pop();
        //        if (!visited.Add(current)) continue;

        //        yield return current;

        //        var neighbours = graph.Neighbours(current).Where(x => !visited.Contains(x.Item1));
        //        foreach (var neighbour in neighbours)
        //        {
        //            stack.Push(neighbour.Item1);
        //        }
        //    }
        //}
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
    }
}