using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShortestPath
{
    public class GraphNew
    {
        private readonly List<Tuple<int, Tuple<int, int>>> edges;

        public GraphNew(IEnumerable<Tuple<int, Tuple<int, int>>> tuples)
        {
            edges = new List<Tuple<int, Tuple<int, int>>>();
            foreach (var e in tuples)
            {
                edges.Add(e);
            }
        }
        public GraphNew() => edges = new List<Tuple<int, Tuple<int, int>>>();
        public IEnumerable<Tuple<int, int>> Neighbours(int n)
        {
            return edges.Where(x => x.Item1 == n).Select(x => x.Item2)
                .Union(edges.Where(x => x.Item2.Item1 == n).Select(x => new Tuple<int, int>(x.Item1, x.Item2.Item2)));
        }
        public IEnumerable<Tuple<int, Tuple<int, int>>> Edges => edges.OrderBy(x => x.Item2.Item2);
        public IEnumerable<int> Nodes()
        {
            return edges.Select(x => x.Item1).Union(edges.Select(x => x.Item2.Item1));
        }
        public void AddEdge(int from, int to, int weight) => edges.Add(new Tuple<int, Tuple<int, int>>(from, new Tuple<int, int>(to, weight)));

        public static void GraphTest()
        {
            string filename = @"C:\projects\vs2017\ShortestPath\hcinput02.txt";
            GraphNew graph = new GraphNew();

            using (StreamReader file = new StreamReader(filename))
            {
                string[] nm = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nm[0]);
                int m = Convert.ToInt32(nm[1]);

                for (int roadsRowItr = 0; roadsRowItr < m; roadsRowItr++)
                {
                    var edge = Array.ConvertAll(file.ReadLine().Split(' '), roadsTemp => Convert.ToInt32(roadsTemp));
                    graph.AddEdge(edge[0], edge[1], edge[2]);
                }
            }

            Console.WriteLine("Nodes {0}", string.Join(" ", graph.Nodes()));
            foreach (var edge in graph.edges)
            {
                Console.WriteLine(edge);
            }
            foreach (var node in graph.Nodes())
            {
                Console.WriteLine("Neighbours of {0} : {1}", node, string.Join(" ", graph.Neighbours(node)));
            }

            foreach (var edge in Kruskal(graph))
            {
                Console.WriteLine(edge);
            }
            
            var g = new GraphNew(Kruskal(graph));
            foreach (var node in g.Nodes())
            {
                Console.WriteLine("Neighbours of {0} : {1}", node, string.Join(" ", g.Neighbours(node)));
            }

            var n1 = g.BFS(2);
            Console.WriteLine("BFS from node 2 - {0}", n1);
            var n2 = g.BFS(n1);
            Console.WriteLine("BFS from node {0} - {1}", n1, n2);
            Console.WriteLine("BFS from node {0} - {1}", n2, g.BFS(n2));
            Console.WriteLine("Shortest distance from {0} to {1} : {2}", n1, n2, g.DFShortestDistance(n1, n2));
        }
        static int Root(int[] id, int x)
        {
            while (id[x] != x)
            {
                id[x] = id[id[x]];
                x = id[x];
            }
            return x;
        }
        static void Union1(int[] id, int x, int y)
        {
            int p = Root(id, x);
            int q = Root(id, y);
            id[p] = id[q];
        }
        static IEnumerable<Tuple<int, Tuple<int, int>>> Kruskal(GraphNew graph)
        {
            int count = graph.Nodes().Count() + 1;
            int[] id = new int[count];
            for (int i = 0; i < count; i++) id[i] = i;
            foreach (var edge in graph.Edges)
            {
                if (Root(id, edge.Item1) != Root(id, edge.Item2.Item1))
                {
                    yield return edge;
                    Union1(id, edge.Item1, edge.Item2.Item1);
                }
            }

        }
        public int BFS(int start)
        {
            HashSet<int> visited = new HashSet<int>();
            Queue<int> queue = new Queue<int>();
            var current = start;
            queue.Enqueue(start);
            while (queue.Any())
            {
                current = queue.Dequeue();
                if (!visited.Add(current)) continue;

                var neighbours = Neighbours(current).Where(x => !visited.Contains(x.Item1));
                foreach (var neighbour in neighbours)
                {
                    queue.Enqueue(neighbour.Item1);
                }
            }
            return current;
        }

        public int DFShortestDistance(int start, int dest)
        {
            List<int> shortest = null;
            int distance = 0;

            Stack<Tuple<Tuple<int, int>, Tuple<List<int>, int>>> stack = new Stack<Tuple<Tuple<int, int>, Tuple<List<int>, int>>>();
            stack.Push(new Tuple<Tuple<int, int>, Tuple<List<int>, int>>(new Tuple<int, int>(start, 0), new Tuple<List<int>, int>(new List<int> { }, 0)));

            while (stack.Any())
            {
                var current = stack.Pop();
                int dist = current.Item1.Item2 + current.Item2.Item2;
                var path = new List<int>(current.Item2.Item1) { current.Item1.Item1 };

                if (current.Item1.Item1.Equals(dest))
                {
                    if (shortest == null || dist < distance)
                    {
                        shortest = path;
                        distance = dist;
                        continue;
                    }
                }

                var neighbours = Neighbours(current.Item1.Item1).Where(x => !path.Contains(x.Item1));
                foreach (var neighbour in neighbours)
                {
                    stack.Push(new Tuple<Tuple<int, int>, Tuple<List<int>, int>>(neighbour, new Tuple<List<int>, int>(path, dist)));
                }
            }

            return distance;
        }
    }
}