using System;
using System.Collections.Generic;
using System.Linq;

namespace ShortestPath
{
    public static class GraphExtensions
    {
        public static IEnumerable<Node> DepthFirstTraversal(this IGraph graph, Node start)
        {
            HashSet<Node> visited = new HashSet<Node>();
            Stack<Node> stack = new Stack<Node>();

            stack.Push(start);
            while (stack.Any())
            {
                Node current = stack.Pop();
                if (!visited.Add(current)) continue;

                yield return current;

                var neighbours = graph.Neighbours(current).Where(x => !visited.Contains(x.Item1));
                foreach (var neighbour in neighbours)
                {
                    stack.Push(neighbour.Item1);
                }
            }
        }
        public static List<Node> DFShortestPath(this IGraph graph, Node start, Node dest)
        {
            List<Node> shortest = null;
            int distance = 0;

            Stack<Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>> stack = new Stack<Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>>();
            stack.Push(new Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>(new Tuple<Node, int>(start, 0), new Tuple<List<Node>, int>(new List<Node> { }, 0)));

            while (stack.Any())
            {
                var current = stack.Pop();
                int dist = current.Item1.Item2 + current.Item2.Item2;
                var path = new List<Node>(current.Item2.Item1) { current.Item1.Item1 };

                if (current.Item1.Item1.Equals(dest))
                {
                    if (shortest == null || dist < distance)
                    {
                        shortest = path;
                        distance = dist;
                        continue;
                    }
                }

                var neighbours = graph.Neighbours(current.Item1.Item1).Where(x => !path.Contains(x.Item1));
                foreach (var neighbour in neighbours)
                {
                    stack.Push(new Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>(neighbour, new Tuple<List<Node>, int>(path, dist)));
                }
            }
            //Console.WriteLine("Distance {0}", distance);
            return shortest;
        }
        public static List<Node> DFShortestPathOptimized(this IGraph graph, Node start, Node dest)
        {
            List<Node> shortest = null;
            int distance = 0;

            Stack<Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>> stack = new Stack<Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>>();
            stack.Push(new Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>(new Tuple<Node, int>(start, 0), new Tuple<List<Node>, int>(new List<Node> { }, 0)));

            while (stack.Any())
            {
                var current = stack.Pop();
                int dist = current.Item1.Item2 + current.Item2.Item2;
                var path = new List<Node>(current.Item2.Item1) { current.Item1.Item1 };

                if (current.Item1.Item1.Equals(dest))
                {
                    if (shortest == null || dist < distance)
                    {
                        shortest = path;
                        distance = dist;
                        continue;
                    }
                }

                var neighbours = graph.Neighbours(current.Item1.Item1).Where(x => !path.Contains(x.Item1));
                foreach (var neighbour in neighbours)
                {
                    if (shortest == null || dist + neighbour.Item2 < distance)
                        stack.Push(new Tuple<Tuple<Node, int>, Tuple<List<Node>, int>>(neighbour, new Tuple<List<Node>, int>(path, dist)));
                }
            }
            //Console.WriteLine("Distance {0}", distance);
            return shortest;
        }
        public static IEnumerable<int> HackerLandShortest(this IGraph graph, Node start, Node dest)
        {
            List<Tuple<Node, int>> shortest = null;
            int distance = 0;

            Stack<Tuple<Tuple<Node, int>, List<Tuple<Node, int>>>> stack = new Stack<Tuple<Tuple<Node, int>, List<Tuple<Node, int>>>>();
            stack.Push(new Tuple<Tuple<Node, int>, List<Tuple<Node, int>>>(new Tuple<Node, int>(start, 0), new List<Tuple<Node, int>> { }));

            while (stack.Any())
            {
                var current = stack.Pop();
                var path = new List<Tuple<Node, int>>(current.Item2) { current.Item1 };
                int dist = path.Select(x => x.Item2).Max();
                if (current.Item1.Item1.Equals(dest))
                {
                    if (shortest == null || dist < distance)
                    {
                        shortest = path;
                        distance = dist;
                        continue;
                    }
                }

                var neighbours = graph.Neighbours(current.Item1.Item1).Where(x => !path.Any(p => p.Item1.Equals(x.Item1)));
                foreach (var neighbour in neighbours)
                {
                    if (shortest == null || (Math.Max(dist, neighbour.Item2) < distance))
                    {
                        stack.Push(new Tuple<Tuple<Node, int>, List<Tuple<Node, int>>>(neighbour, path));
                    }
                }
            }
            //Console.WriteLine("Path : {0}", string.Join(" ", shortest));
            return shortest.Skip(1).Select(x => x.Item2).OrderBy(x => x);
        }
    }
}