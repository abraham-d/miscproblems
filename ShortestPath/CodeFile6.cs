using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ShortestPath
{
    partial class Program
    {
        static void JeaniesRouteTest()
        {
            string filename = @"C:\projects\vs2017\ShortestPath\jrctestcase08.txt";
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

                var sw = new Stopwatch();
                sw.Start();
                int result = JeanisRoute(city, roads);
                sw.Stop();
                Console.WriteLine("Result : {0}  Elapsed time : {1}", result, sw.Elapsed);
            }
        }
        static int JeanisRoute(int[] k, int[][] roads)
        {
            bool[] status = new bool[roads.Length];
            FilterRoads(roads, k, status);
            var filtered = roads.Zip(status, (x, y) => new { edge = x, status = y })
                            .Where(x => x.status == false)
                            .Select(x => x.edge);            
                            //.OrderBy(x => x[2]);
            //Console.WriteLine("filtered edge count : {0}, Sum : {1}", filtered.Count(), filtered.Sum(x => x[2]));

            //var mst = Kruskal(filtered, roads.Length + 2);
            //var s = filtered.OrderByDescending(x => x[1]).First()[0];
            var mst = filtered;
            var n1 = BFS(mst, filtered.First()[0]);
            var n2 = BFS(mst, n1.Item1, true);
            //var n2 = BFS(mst, s, true);
            //int diameter = DFShortestDistance(filtered, n1.Item1, n2.Item1);
            //return n1.Item2 + n2.Item2 - diameter;
            return 2 * n1.Item2 - n2.Item2;
        }        

        static Tuple<int, int> BFS(IEnumerable<int[]> edges, int start, bool flag = false)
        {
            Dictionary<int, Tuple<int, int>> path = new Dictionary<int, Tuple<int, int>>();
            HashSet<int> visited = new HashSet<int>();
            Queue<int> queue = new Queue<int>();
            var current = start;
            int distance = 0;
            queue.Enqueue(start);
            while (queue.Any())
            {
                current = queue.Dequeue();
                if (!visited.Add(current)) continue;

                var neighbours = Neighbours(edges, current).Where(x => !visited.Contains(x.Item1));//.OrderBy(x => x.Item2);
                foreach (var neighbour in neighbours)
                {
                    queue.Enqueue(neighbour.Item1);
                    distance += neighbour.Item2;
                    path.Add(neighbour.Item1, new Tuple<int, int>(current, neighbour.Item2));
                }
            }
            if (flag)
            {
                int dist = 0;
                int c = current;
                while (path.ContainsKey(c))
                {
                    dist += path[c].Item2;
                    c = path[c].Item1;
                }
                distance = dist;
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
                .Concat(edges.Where(x => x[1] == current).Select(x => new Tuple<int, int>(x[0], x[2])))
                .OrderBy(x=>x.Item2);
        }

        private static IEnumerable<int[]> Kruskal(IEnumerable<int[]> edges, int n)
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

        static void FilterRoads(int[][] roads, int[] city, bool[] status)
        {
            var ct = new HashSet<int>(city);
            var roadsWithStatus = roads.Zip(status, (x, y) => new { edge = x, status = y });
            while (true)
            {
                //Console.WriteLine("Nodes to Exclude {0} -- {1}", status.Count(x => x == true), status.Count(x => x == false));
                //var roadsWithStatus = roads.Zip(status, (x, y) => new { edge = x, status = y });
                var nodesToExclude = roadsWithStatus.Select((x, i) => new { node = x.edge[0], index = i, status = x.status })
                                        .Concat(roadsWithStatus.Select((x, i) => new { node = x.edge[1], index = i, status = x.status }))
                                        .Where(x => (x.status == false && !ct.Contains(x.node)))
                                        .GroupBy(x => x.node)
                                        .Where(x => x.Count() == 1)
                                        .Select(x => x.First().index);
                                        //.Select(x => new { key = x.Key, index = x.Max(y => y.index), count = x.Count() })
                                        //.Where(x => x.count == 1)
                                        //.Select(x => x.index);
                if (nodesToExclude.Any())
                {
                    foreach (int index in nodesToExclude) status[index] = true;
                }
                else
                {
                    break;
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
        static bool IsConnected(int[] ids, int x, int y)
        {
            return Root(ids, x) == Root(ids, y);
        }
        private static IEnumerable<int> FindNodes(int[][] roads)
        {
            return roads.Select(x => x[0]).Union(roads.Select(x => x[1]));
        }

        private static IEnumerable<Tuple<int, int>> FindLeafNodes(IEnumerable<int[]> roads)
        {
            //return roads.Select(x => x[0]).Concat(roads.Select(x => x[1]))
            //    .GroupBy(x => x)
            //    .Select(x => new { key = x.Key, cnt = x.Count() })
            //    .Where(x => x.cnt == 1)
            //    .Select(x => x.key);
            return roads.Select((x, i) => new { node = x[0], index = i }).Concat(roads.Select((x, i) => new { node = x[1], index = i }))
                .GroupBy(x => x.node)
                .Select(x => new { key = x.Key, index = x.Max(y => y.index), cnt = x.Count() })
                .Where(x => x.cnt == 1)
                .Select(x => new Tuple<int, int>(x.key, x.index));
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