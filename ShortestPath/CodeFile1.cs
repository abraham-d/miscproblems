using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ShortestPath
{
    partial class Program
    {
        private static int[] ShortestPathMatrix(int[,] weights, int start, int n)
        {
            /*
             * 
             procedure FloydWarshallWithPathReconstruction ()
   for each edge (u,v)
      dist[u][v] ← w(u,v)  // the weight of the edge (u,v)
      next[u][v] ← v
   for k from 1 to |V| // standard Floyd-Warshall implementation
      for i from 1 to |V|
         for j from 1 to |V|
            if dist[i][j] > dist[i][k] + dist[k][j] then
               dist[i][j] ← dist[i][k] + dist[k][j]
               next[i][j] ← next[i][k]

             * 
             */
            int[] result = new int[n - 1];
            //for(int k = 0; k < n; k++)
            int k = start;
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (weights[i, j] > weights[i, k] + weights[k, j])
                            weights[i, j] = weights[i, k] + weights[k, j];
                    }
                }
            }
            for (int indx = 0; indx < n; indx++)
            {
                if (indx == start) continue;
                else if (indx > start)
                {
                    result[indx - 1] = weights[start, indx] == 0 ? -1 : weights[start, indx];
                }
                else
                {
                    result[indx] = weights[start, indx] == 0 ? -1 : weights[start, indx];
                }
            }
            return result;
        }

        static void FindShortestPath01()
        {
            var sw = new Stopwatch();
            sw.Start();
            string filename = @"C:\projects\vs2017\ShortestPath\input07.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                string line;
                line = file.ReadLine();
                int t = int.Parse(line);
                for (int i = 0; i < t; i++)
                {
                    string[] tokens_n = file.ReadLine().Split(' ');
                    int n = Convert.ToInt32(tokens_n[0]);
                    int m = Convert.ToInt32(tokens_n[1]);

                    HashSet<int>[] edges = new HashSet<int>[n];
                    int[,] weights = new int[n, n];

                    for (int a1 = 0; a1 < m; a1++)
                    {
                        string[] tokens_x = file.ReadLine().Split(' ');
                        int x = int.Parse(tokens_x[0]) - 1;
                        int y = int.Parse(tokens_x[1]) - 1;
                        int r = int.Parse(tokens_x[2]);

                        if (edges[x] == null) edges[x] = new HashSet<int>();
                        if (edges[y] == null) edges[y] = new HashSet<int>();
                        edges[x].Add(y);
                        edges[y].Add(x);
                        //weights[x, y] = weights[x, y] == 0 ? r : Math.Min(weights[x, y], r);
                        //weights[y, x] = weights[x, y] == 0 ? r : Math.Min(weights[x, y], r);
                        weights[x, y] = r;
                        weights[y, x] = r;
                    }
                    int s = int.Parse(file.ReadLine()) - 1;

                    sw.Restart();
                    int[] result = Evaluate(weights, s, n);
                    //int[] result = Evaluate(edges, weights, s, n);
                    Console.WriteLine(string.Join(" ", result));
                    Console.WriteLine("Elapsed Time : {0} ms", sw.ElapsedMilliseconds);
                    Console.WriteLine();
                }
            }
        }

        private static int[] Evaluate(int[][] weights, int s, int n)
        {
            int[] result = Enumerable.Repeat(-1, n - 1).ToArray();
            Queue<Tuple<int, int>> worklist = new Queue<Tuple<int, int>>();
            worklist.Enqueue(new Tuple<int, int>(0, s));
            HashSet<int> visited = new HashSet<int>();
            while (worklist.Count > 0)
            {
                var current = worklist.Dequeue();
                for (int i = 0; i < n; i++)
                {
                    int wt = weights[current.Item2][i];
                    if (wt != -1)
                    {
                        int weigt = current.Item1 + wt;
                        if (i == s)
                        {
                            int indx = i > s ? i - 1 : i;
                            if (result[indx] == -1 || result[indx] > weigt)
                            {
                                result[indx] = weigt;
                                if (!visited.Any(x => x == i))
                                {
                                    worklist.Enqueue(new Tuple<int, int>(weigt, i));
                                }
                            }
                        }
                        else
                        {
                            if (!visited.Any(x => x == i))
                            {
                                worklist.Enqueue(new Tuple<int, int>(weigt, i));
                            }
                        }
                    }
                }
                visited.Add(current.Item2);
            }

            return result;
        }

        static void FindShortestPath()
        {
            var sw = new Stopwatch();
            sw.Start();
            string filename = @"C:\projects\vs2017\ShortestPath\testcase04.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                string line;
                line = file.ReadLine();
                int t = int.Parse(line);
                for (int i = 0; i < t; i++)
                {
                    string[] tokens_n = file.ReadLine().Split(' ');
                    int n = Convert.ToInt32(tokens_n[0]);
                    int m = Convert.ToInt32(tokens_n[1]);

                    HashSet<int>[] edges = new HashSet<int>[n];
                    int[,] weights = new int[n, n];

                    for (int a1 = 0; a1 < m; a1++)
                    {
                        string[] tokens_x = file.ReadLine().Split(' ');
                        int x = int.Parse(tokens_x[0]) - 1;
                        int y = int.Parse(tokens_x[1]) - 1;
                        int r = int.Parse(tokens_x[2]);

                        if (edges[x] == null) edges[x] = new HashSet<int>();
                        if (edges[y] == null) edges[y] = new HashSet<int>();
                        edges[x].Add(y);
                        edges[y].Add(x);
                        weights[x, y] = weights[x, y] == 0 ? r : Math.Min(weights[x, y], r);
                        weights[y, x] = weights[x, y] == 0 ? r : Math.Min(weights[x, y], r);
                    }

                    int s = int.Parse(file.ReadLine()) - 1;

                    //printWeights(weights, n, edges);
                    sw.Restart();
                    int[] result = Evaluate(edges, weights, s, n);
                    Console.WriteLine(string.Join(" ", result));
                    Console.WriteLine("Elapsed Time : {0} ms", sw.ElapsedMilliseconds);
                    Console.WriteLine();

                    //sw.Restart();
                    //result = ShortestPaths(edges, weights, s, n);
                    //Console.WriteLine(string.Join(" ", result));
                    //Console.WriteLine("Elapsed Time : {0} ms", sw.ElapsedMilliseconds);
                    //Console.WriteLine();

                }
            }

            //Console.WriteLine("Elapsed Time : {0} ms", sw.ElapsedMilliseconds);

        }
        private static int[] ShortestPaths(HashSet<int>[] edges, int[,] weights, int start, int n)
        {
            int[] result = new int[n - 1];
            for (int i = 0; i < n; i++)
            {
                if (i == start) continue;
                int indx = i > start ? i - 1 : i;
                result[indx] = ShortestPath(edges, weights, start, i, n);
                //Console.WriteLine("Start {0} Dest {1} dist {2}", start, i, result[indx]);
            }
            return result;
        }

        private static int ShortestPath(HashSet<int>[] edges, int[,] weights, int start, int dest, int n)
        {
            int currentDist = -1;
            Queue<Tuple<int, int>> worklist = new Queue<Tuple<int, int>>();
            List<int> visited = new List<int>();
            worklist.Enqueue(new Tuple<int, int>(0, start));

            while (worklist.Count > 0)
            {
                var current = worklist.Dequeue();
                foreach (int node in edges[current.Item2])
                {
                    //if (node != start)
                    {
                        int weight = current.Item1 + weights[current.Item2, node];
                        if (node == dest)
                        {
                            if (currentDist == -1 || weight < currentDist)
                            {
                                currentDist = weight;
                            }
                        }
                        else if (!visited.Any(x => x == node))
                        {
                            worklist.Enqueue(new Tuple<int, int>(weight, node));
                        }
                    }
                }
                visited.Add(current.Item2);
            }
            return currentDist;
        }

        private static int[] Evaluate(HashSet<int>[] edges, int[,] weights, int s, int n)
        {
            int[] result = new int[n - 1];
            for (int i = 0; i < n - 1; i++) { result[i] = -1; }
            Stack<Tuple<int, int>> worklist = new Stack<Tuple<int, int>>();

            worklist.Push(new Tuple<int, int>(0, s));
            while (worklist.Any())
            {
                var current = worklist.Pop();
                foreach (int node in edges[current.Item2])
                {
                    if (node != s)
                    {
                        int weigt = current.Item1 + weights[current.Item2, node];
                        int indx = node > s ? node - 1 : node;
                        if (result[indx] == -1 || result[indx] > weigt)
                        {
                            result[indx] = weigt;
                            worklist.Push(new Tuple<int, int>(weigt, node));
                        }
                    }
                }
            }

            return result;
        }
        private static int[] Evaluate(int[,] weights, int s, int n)
        {
            int[] result = new int[n - 1];
            for (int i = 0; i < n - 1; i++) { result[i] = -1; }
            Stack<Tuple<int, int>> worklist = new Stack<Tuple<int, int>>();

            worklist.Push(new Tuple<int, int>(0, s));
            while (worklist.Any())
            {
                var current = worklist.Pop();
                foreach (int node in ConnectedNodes(weights, current.Item2, n))
                {
                    if (node != s)
                    {
                        int weigt = current.Item1 + weights[current.Item2, node];
                        int indx = node > s ? node - 1 : node;
                        if (result[indx] == -1 || result[indx] > weigt)
                        {
                            result[indx] = weigt;
                            worklist.Push(new Tuple<int, int>(weigt, node));
                        }
                    }
                }
            }

            return result;
        }
        static IEnumerable<int> ConnectedNodes(int[,] weights, int node, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (weights[node, i] > 0)
                    yield return i;
            }
        }
        private static void printWeights(int[,] weights, int n, HashSet<int>[] edges)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(" {0:d2}", weights[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine("===================");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("{0} -- {1}", i + 1, edges[i] == null ? "" : string.Join(" ", edges[i]));
            }
        }
    }
}