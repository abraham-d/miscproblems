using System;
using System.Collections.Generic;
using System.IO;

namespace SpecialMultiple
{
    partial class Program
    {
        static void GraphTest()
        {
            string filename = @"C:\projects\vs2017\SpecialMultiple\hrinput00.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                string[] nm = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nm[0]);
                int m = Convert.ToInt32(nm[1]);

                Graph graph = new Graph();
                for (int roadsRowItr = 0; roadsRowItr < m; roadsRowItr++)
                {
                    var e = Array.ConvertAll(file.ReadLine().Split(' '), roadsTemp => Convert.ToInt32(roadsTemp));
                    graph.AddEdge(new Node(e[0]), new Node(e[1]), e[2]);
                }
                List<Node> visited = null;
                List<Node> path = new List<Node>();
                var start = new Node(1);
                var dest = new Node(4);
                var r = graph.Recurse(start, dest, path, visited);
                Console.WriteLine("Start : {0} Dest : {1}", start, dest);
                foreach (var p in r)
                    Console.Write("{0} ", p);
                Console.WriteLine();

                var s = graph.NonRecurse(start, dest);
                Console.WriteLine("Start : {0} Dest : {1}", start, dest);
                foreach (var p in s)
                    Console.Write("{0} ", p);
                Console.WriteLine();

                var sh = graph.ShortestPathFunction(start);
                foreach(var c in graph.Nodes)
                {
                    Console.WriteLine("Shortest Path to {0}: {1}", c, string.Join(" ", sh(c)));
                }

                //foreach(var node in graph.Nodes)
                //{
                //    Console.WriteLine(node);
                //    foreach (var c in graph.Children(node))
                //        Console.Write("{0} ", c);
                //    Console.WriteLine();
                //}
                //Console.WriteLine(result);
            }
        }
    }
}