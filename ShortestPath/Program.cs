using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortestPath
{
    class Program
    {
        static void Main(string[] args)
        {
            FindShortestPath01();
            FindShortestPath();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
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

                    int[][] weights = Enumerable.Repeat(Enumerable.Repeat(-1, n).ToArray(), n).ToArray();

                    for (int a1 = 0; a1 < m; a1++)
                    {
                        string[] tokens_x = file.ReadLine().Split(' ');
                        int x = int.Parse(tokens_x[0]) - 1;
                        int y = int.Parse(tokens_x[1]) - 1;
                        int r = int.Parse(tokens_x[2]);

                        weights[x][y] = weights[x][y] == -1 ? r : Math.Min(weights[x][y], r);
                        weights[y][x] = weights[y][x] == -1 ? r : Math.Min(weights[y][x], r);
                    }

                    int s = int.Parse(file.ReadLine()) - 1;

                    //printWeights(weights, n, edges);
                    sw.Restart();
                    int[] result = Evaluate(weights, s, n);
                    Console.WriteLine(string.Join(" ", result));
                    Console.WriteLine("Elapsed Time : {0} ms", sw.ElapsedMilliseconds);                    
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
            //int[] result = Enumerable.Repeat(-1, n - 1).ToArray();
            Queue<Tuple<int, int>> worklist = new Queue<Tuple<int, int>>();
            //HashSet<int> visited = new HashSet<int>();
            worklist.Enqueue(new Tuple<int, int>(0, s));
            while (worklist.Count > 0)
            {
                var current = worklist.Dequeue();
                foreach(int node in edges[current.Item2])
                {
                    if (node != s)
                    {
                        int weigt = current.Item1 + weights[current.Item2, node];
                        int indx = node > s ? node - 1 : node;
                        if (result[indx] == -1 || result[indx] > weigt)
                        {
                            result[indx] = weigt;
                            //if (!visited.Any(x => x == node))
                                worklist.Enqueue(new Tuple<int, int>(weigt, node));
                        }
                    }
                }
                //visited.Add(current.Item2);
            }

            return result;
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
