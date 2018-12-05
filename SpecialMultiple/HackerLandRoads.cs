using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpecialMultiple
{
    partial class Program
    {
        static void HLRTest()
        {
            var l = Enumerable.Repeat(new { src = 1, dest = 2, weight = 3 }, 0).ToList();
            l.Add(new { src = 1, dest = 3, weight = 3 });

            Console.WriteLine(l.Count);
        }
        static void HackerLandRoad()
        {
            string filename = @"C:\projects\vs2017\SpecialMultiple\hrinput00.txt";
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

                string result = RoadsInHackerland(n, roads);
                Console.WriteLine(result);
            }
        }

        private static string RoadsInHackerland(int n, int[][] roads)
        {
            string result = "";
            HashSet<int> nodes = new HashSet<int>();
            Dictionary<int, List<Tuple<int, int>>> edges = new Dictionary<int, List<Tuple<int, int>>>();
            foreach(var e in roads)
            {
                nodes.Add(e[0]);
                nodes.Add(e[1]);
                if (edges.ContainsKey(e[0])) edges[e[0]].Add(new Tuple<int, int>(e[1], e[2]));
                else edges.Add(e[0], new List<Tuple<int, int>> { new Tuple<int, int>(e[1], e[2]) });
                if (edges.ContainsKey(e[1])) edges[e[1]].Add(new Tuple<int, int>(e[0], e[2]));
                else edges.Add(e[1], new List<Tuple<int, int>> { new Tuple<int, int>(e[0], e[2]) });
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    Console.WriteLine("{0} -- {1}", nodes.ElementAt(i), nodes.ElementAt(j));
                    List<int> path = new List<int>();
                    List<int> sp = null;
                    var p = FindShortestPath(nodes.ElementAt(i), nodes.ElementAt(j), edges, path, sp);
                    foreach (var k in p)
                    {
                        Console.Write("{0} ", k);
                    }
                    Console.WriteLine();
                }
            }
            return result;
        }

        private static List<int> FindShortestPath(int start, int end, Dictionary<int, List<Tuple<int, int>>> edges, List<int> path, List<int> shortest)
        {
            //path = new List<int>();
            path.Add(start);
            if (start == end) return path;
            foreach (var n in edges[start])
            {
                if (!path.Any(x => x == n.Item1))
                {
                    if (shortest == null || path.Count < shortest.Count)
                    {
                        var newPath = FindShortestPath(n.Item1, end, edges, path, shortest);
                        if (newPath != null)
                        {
                            shortest = newPath;
                            //return shortest;
                        }
                    }
                }
            }
            return shortest;
        }
        private static string RoadsInHackerland1(int n, int[][] roads)
        {
            int[,] g = new int[n, n];
            foreach(var p in roads)
            {
                g[p[0] - 1, p[1] - 1] = 1 << p[2];
                g[p[1] - 1, p[0] - 1] = g[p[0] - 1, p[1] - 1];
            }
            for(int i = 0; i < n; i++)
            {
                for(int j = i + 1; j < n; j++)
                {
                    //Console.WriteLine("({0},{1})", i, j);
                    EvaluateShortest(g, n, i, j);
                }
            }
            return "NotImplementedException";
        }

        private static void EvaluateShortest(int[,] g, int n, int i, int j)
        {
            throw new NotImplementedException();
        }
    }
}