using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShortestPath
{
    public interface IGraph
    {
        IEnumerable<Node> Nodes();
        IEnumerable<Tuple<Node, int>> Neighbours(Node n);
    }
    public class Node
    {
        private readonly int node;

        public Node(int node) => this.node = node;
        public override string ToString() => node.ToString("D2");
        public override bool Equals(object obj) => GetHashCode() == obj.GetHashCode();
        public override int GetHashCode() => node;
    }

    public class Graph : IGraph
    {
        readonly HashSet<Node> nodes;
        readonly Dictionary<Node, List<Tuple<Node, int>>> children;
        public Graph()
        {
            nodes = new HashSet<Node>();
            children = new Dictionary<Node, List<Tuple<Node, int>>>();
        }
        public IEnumerable<Node> Nodes()
        {
            return new List<Node>(nodes).AsReadOnly();
        }

        public IEnumerable<Tuple<Node, int>> Neighbours(Node node)
        {
            if (children.ContainsKey(node)) return children[node].AsReadOnly();
            return null;
        }
        public void AddNode(Node node) => nodes.Add(node);

        public void AddEdge(Node src, Node dst, int weight)
        {
            nodes.Add(src);
            nodes.Add(dst);

            if (children.ContainsKey(src))
                children[src].Add(new Tuple<Node, int>(dst, weight));
            else
                children.Add(src, new List<Tuple<Node, int>> { new Tuple<Node, int>(dst, weight) });

            if (children.ContainsKey(dst))
                children[dst].Add(new Tuple<Node, int>(src, weight));
            else
                children.Add(dst, new List<Tuple<Node, int>> { new Tuple<Node, int>(src, weight) });

        }

        public IEnumerable<int> HackerLandShortestN(Node start, Node dest)
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

                var neighbours = Neighbours(current.Item1.Item1).Where(x => !path.Any(p => p.Item1.Equals(x.Item1)));
                foreach (var neighbour in neighbours)
                {
                    if (shortest == null || (Math.Max(dist, neighbour.Item2) < distance))
                    {
                        stack.Push(new Tuple<Tuple<Node, int>, List<Tuple<Node, int>>>(neighbour, path));
                    }
                }
            }

            return shortest.Skip(1).Select(x => x.Item2).OrderBy(x => x);
        }

    }
}