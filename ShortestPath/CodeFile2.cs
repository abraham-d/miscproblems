using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace ShortestPath
{
    [ClrJob(true)]
    public class GraphBenchMark
    {
        [Params(5, 6, 8, 7)]
        public int dest;

        Graph graph;

        [GlobalSetup]
        public void Setup()
        {
            string filename = @"C:\projects\vs2017\ShortestPath\hcinput02.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                graph = new Graph();
                string[] nm = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nm[0]);
                int m = Convert.ToInt32(nm[1]);

                for (int roadsRowItr = 0; roadsRowItr < m; roadsRowItr++)
                {
                    var edge = Array.ConvertAll(file.ReadLine().Split(' '), roadsTemp => Convert.ToInt32(roadsTemp));
                    graph.AddEdge(new Node(edge[0]), new Node(edge[1]), edge[2]);
                }
            }
        }

        [Benchmark]
        public void DFShortest()
        {
            graph.DFShortestPath(new Node(1), new Node(dest));
        }

        [Benchmark]
        public void DFShortestOpt()
        {
            graph.DFShortestPathOptimized(new Node(1), new Node(dest));
        }
    }
    partial class Program
    {
        static void GraphTest()
        {
            string filename = @"C:\projects\vs2017\ShortestPath\hcinput02.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                Graph graph = new Graph();
                string[] nm = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nm[0]);
                int m = Convert.ToInt32(nm[1]);

                int longest = 0;
                for (int roadsRowItr = 0; roadsRowItr < m; roadsRowItr++)
                {
                    var edge = Array.ConvertAll(file.ReadLine().Split(' '), roadsTemp => Convert.ToInt32(roadsTemp));
                    graph.AddEdge(new Node(edge[0]), new Node(edge[1]), edge[2]);

                    if (edge[2] > longest) longest = edge[2];
                }

                //foreach(var c in graph.Nodes())
                //{
                //    Console.WriteLine("Children of {0} : {1}", c, string.Join(" ", graph.Neighbours(c)));
                //}

                //Node start = new Node(3);
                //Node dest = new Node(5);
                //var p = graph.DepthFirstTraversal(start);
                //Console.WriteLine(string.Join(" ", p));
                //string result = roadsInHackerland(n, roads);
                //var p = graph.DFShortestPath(start, dest);
                //Console.WriteLine("Shortest path from {0} to {1} : {2}", start, dest, string.Join(" ", p));
                //var p1 = graph.DFShortestPathOptimized(start, dest);
                //Console.WriteLine("Shortest path from {0} to {1} : {2}", start, dest, string.Join(" ", p1));

                bool[] result = new bool[longest + 3];

                var nodes = graph.Nodes().ToArray();
                for (int i = 0; i < nodes.Length; i++)
                {
                    for (int j = i + 1; j < nodes.Length; j++)
                    {
                        var p = graph.HackerLandShortest(nodes[i], nodes[j]);
                        Console.WriteLine("Shortest path from {0} to {1} : {2}", nodes[i], nodes[j], string.Join(" ", p));
                        //Console.WriteLine("Path {0}", string.Join(" ", p));
                        foreach(var c in p)
                        {
                            Add(result, c);
                        }
                    }
                }

                Console.Write("Result : ");
                foreach (var d in result.Reverse())
                {
                    Console.Write(d ? 1 : 0);
                }
                Console.WriteLine();
            }            
        }

        private static bool Add(bool[] result, int c)
        {
            //if (c > result.Length - 1) return false;
            if (result[c])
            {
                result[c] = false;
                return Add(result, c + 1);
            }
            result[c] = true;
            return false;
        }
    }
}