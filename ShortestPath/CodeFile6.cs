using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShortestPath
{
    partial class Program
    {
        static void JeaniesRouteTest()
        {
            string filename = @"C:\projects\vs2017\ShortestPath\jrctestcase02.txt";
            using (StreamReader file = new StreamReader(filename))
            {
                string[] nk = file.ReadLine().Split(' ');
                int n = Convert.ToInt32(nk[0]);
                int k = Convert.ToInt32(nk[1]);
                int[] city = Array.ConvertAll(file.ReadLine().Split(' '), cityTemp => Convert.ToInt32(cityTemp));

                int[][] roads = new int[n - 1][];
                for (int roadsRowItr = 0; roadsRowItr < n - 1; roadsRowItr++)
                {
                    roads[roadsRowItr] = Array.ConvertAll(file.ReadLine().Split(' '), roadsTemp => Convert.ToInt32(roadsTemp));
                }

                int result = JeanisRoute(city, roads);
                Console.WriteLine("Result : {0}", result);
            }
        }
        static int JeanisRoute(int[] k, int[][] roads)
        {
            bool[] status = new bool[roads.Length];
            while (true)
            {
                var filtered = FilterRoads(roads, status);
                var leafNodes = FindLeafNodes(filtered);
                var leaftodiscard = leafNodes.Where(x => !k.Contains(x));
                if (leaftodiscard.Any())
                {
                    foreach(var discard in leaftodiscard)
                    {
                        var indx = filtered
                            .Select((x, i) => new { item = x, index = i })
                            .First(x => (x.item[0] == discard || x.item[1] == discard))?
                            .index;
                        if (indx.HasValue)
                            status[indx.Value] = true;
                    }
                    //var indx = roads
                    //    .Select((x, i) => new { item = x, index = i })
                    //    .First(x => (leaftodiscard.Contains(x.item[0]) || leaftodiscard.Contains(x.item[1])))
                    //    .index;
                    //status[indx] = true;
                }
                else
                {
                    break;
                }
            }
            var edges = FilterRoads(roads, status).OrderBy(x => x[2]);
            foreach (var edge in edges)
            {
                Console.WriteLine(string.Join(" ", edge));
            }

            var mst = Kruskal(edges, roads.Length + 2);
            var n1 = BFS(mst, 1);
            var n2 = BFS(mst, n1.Item1);
            int diameter = DFShortestDistance(edges, n1.Item1, n2.Item1);
            return n1.Item2 + n2.Item2 - diameter;
        }
        static Tuple<int, int> BFS(IEnumerable<int[]> edges, int start)
        {
            HashSet<int> visited = new HashSet<int>();
            Queue<int> queue = new Queue<int>();
            var current = start;
            int distance = 0;
            queue.Enqueue(start);
            while (queue.Any())
            {
                current = queue.Dequeue();
                if (!visited.Add(current)) continue;

                var neighbours = Neighbours(edges, current).Where(x => !visited.Contains(x.Item1)).OrderBy(x => x.Item2);
                foreach (var neighbour in neighbours)
                {
                    queue.Enqueue(neighbour.Item1);
                    distance += neighbour.Item2;
                }
            }
            return new Tuple<int, int>(current, distance);
        }
        static int DFShortestDistance(IEnumerable<int[]> edges, int start, int dest)
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

                var neighbours = Neighbours(edges, current.Item1.Item1).Where(x => !path.Contains(x.Item1));
                foreach (var neighbour in neighbours)
                {
                    stack.Push(new Tuple<Tuple<int, int>, Tuple<List<int>, int>>(neighbour, new Tuple<List<int>, int>(path, dist)));
                }
            }

            return distance;
        }
        private static IEnumerable<Tuple<int, int>> Neighbours(IEnumerable<int[]> edges, int current)
        {
            return edges.Where(x => x[0] == current).Select(x => new Tuple<int, int>(x[1], x[2]))
                .Union(edges.Where(x => x[1] == current).Select(x => new Tuple<int, int>(x[0], x[2])));
        }

        private static IEnumerable<int[]> Kruskal(IOrderedEnumerable<int[]> edges, int n)
        {
            var nodes = edges.Select(x => x[0]).Union(edges.Select(x => x[1]));
            int[] ids = new int[n];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = i;
            }
            foreach (var edge in edges)
            {
                if (Root(ids, edge[0]) != Root(ids, edge[1]))
                {
                    yield return edge;
                    Union(ids, edge[0], edge[1]);
                }
            }
        }

        static IEnumerable<int[]> FilterRoads(int[][] roads, bool[] status)
        {
            for (int i = 0; i < roads.Length; i++)
            {
                if (!status[i])
                {
                    yield return roads[i];
                }
            }
        }
        static int Root(int[] ids, int x)
        {
            while (ids[x] != x)
            {
                ids[x] = ids[ids[x]];
                x = ids[x];
            }
            return x;
        }
        static void Union(int[] ids, int x, int y)
        {
            int p = Root(ids, x);
            int q = Root(ids, y);
            ids[p] = ids[q];
        }
        private static IEnumerable<int> FindNodes(int[][] roads)
        {
            return roads.Select(x => x[0]).Union(roads.Select(x => x[1]));
        }

        private static IEnumerable<int> FindLeafNodes(IEnumerable<int[]> roads)
        {
            return roads.Select(x => x[0]).Concat(roads.Select(x => x[1]))
                .GroupBy(x => x)
                .Select(x => new { key = x.Key, cnt = x.Count() })
                .Where(x => x.cnt == 1)
                .Select(x => x.key);
        }
        private static IEnumerable<int> FindLeafNodes(int[][] roads)
        {
            return roads.Select(x => x[0]).Concat(roads.Select(x => x[1]))
                .GroupBy(x => x)
                .Select(x => new { key = x.Key, cnt = x.Count() })
                .Where(x => x.cnt == 1)
                .Select(x => x.key);
        }
    }
}