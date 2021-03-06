﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpecialMultiple
{
    public class Node
    {
        private readonly int node;
        public Node(int node) => this.node = node;
        public override string ToString()
        {
            return node.ToString("D2");
        }
        public override int GetHashCode()
        {
            return node;
        }
        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }
    }

    public class Graph
    {
        readonly HashSet<Node> nodes;
        readonly Dictionary<Node, List<Tuple<Node, int>>> children;
        public Graph()
        {
            nodes = new HashSet<Node>();
            children = new Dictionary<Node, List<Tuple<Node, int>>>();
        }
        public HashSet<Node> Nodes { get { return nodes; } }
        public void AddNode(Node node) => this.nodes.Add(node);
        public void AddEdge(Node src, Node dst, int weight)
        {
            if (children.ContainsKey(src))
                children[src].Add(new Tuple<Node, int>(dst, weight));
            else
            {
                nodes.Add(src);
                nodes.Add(dst);
                children.Add(src, new List<Tuple<Node, int>> { new Tuple<Node, int>(dst, weight) });
            }

            if (children.ContainsKey(dst))
                children[dst].Add(new Tuple<Node, int>(src, weight));
            else
            {
                nodes.Add(src);
                nodes.Add(dst);
                children.Add(dst, new List<Tuple<Node, int>> { new Tuple<Node, int>(src, weight) });
            }
        }
        public List<Tuple<Node, int>> Children(Node node)
        {
            if (children.ContainsKey(node))
                return children[node];
            return null;
        }
        int Weight(List<Node> path)
        {
            int wt = 0;
            for (int i = 1; i < path.Count; i++)
            {
                wt += Children(path[i - 1]).Where(x => x.Item1.Equals(path[i])).First().Item2;
            }
            return wt;
        }
        public List<Node> Recurse(Node src, Node dst, List<Node> path, List<Node> shortest)
        {
            Console.WriteLine("Source : {0} Dest : {1}", src, dst);
            path.Add(src);
            if (src.Equals(dst)) return path;
            
            foreach (var c in Children(src))
            {
                var p = new List<Node>(path);
                if (!path.Any(x => x.Equals(c.Item1)))
                {
                    var newPath = Recurse(c.Item1, dst, p, shortest);
                    if (shortest == null || Weight(newPath) < Weight(shortest))
                        shortest = newPath;
                }                
            }
            
            return shortest;
        }

        public List<Tuple<Node, int>> NonRecurse(Node src, Node dest)
        {
            Stack<Tuple<Node, int>> stack = new Stack<Tuple<Node, int>>();
            stack.Push(new Tuple<Node, int>(src, 0));
            List<Tuple<Node, int>> shortest = null;
            List<Tuple<Node, int>> path = new List<Tuple<Node, int>>();
            List<Node> visited = new List<Node>();
            while (stack.Any())
            {
                var current = stack.Pop();
                Console.WriteLine("Visiting {0}", current);

                path.Add(current);
                visited.Add(current.Item1);
                if (current.Item1.Equals(dest))
                {
                    if (shortest == null || shortest.Select(x => x.Item2).Sum() < path.Select(x => x.Item2).Sum())
                    {
                        shortest = path;
                        path = new List<Tuple<Node, int>>();
                    }
                }
                foreach (var c in Children(current.Item1))
                {
                    if (!visited.Any(x => x.Equals(c.Item1))) stack.Push(c);
                }
            }

            return shortest;
        }
        public Func<Node, IEnumerable<Node>> ShortestPathFunction(Node start)
        {
            var previous = new Dictionary<Node, Node>();
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(start);
            while (queue.Any())
            {
                var current = queue.Dequeue();
                foreach(var c in Children(current))
                {
                    if (previous.ContainsKey(c.Item1)) continue;
                    previous[c.Item1] = current;
                    queue.Enqueue(c.Item1);
                }
            }

            IEnumerable<Node> ShortestPath(Node v)
            {
                var current = v;
                while (!current.Equals(start))
                {
                    yield return current;
                    current = previous[current];
                }
                yield return start;
            };

            return ShortestPath;
        }
    }
    /*public class Node
    {
        public Node(string name) => Name = name;
        public string Name { get; }
        public override string ToString() => Name;
    }

    public class Edge
    {
        public Edge(Node src,Node dest)
        {
            Src = src;
            Dest = dest;
        }

        public Node Src { get; }
        public Node Dest { get; }
        public override string ToString() => string.Format("{0} -> {1}", Src.Name, Dest.Name);
    }

    public class WeightedEdge : Edge
    {
        public WeightedEdge(Node src, Node dest, int weight) : base(src, dest) => Weight = weight;
        public int Weight { get; }
        public override string ToString() => string.Format("{0}->({1}){2}", Src.Name, Weight, Dest.Name);
    }

    public class Digraph
    {
        private HashSet<Node> nodes;
        private Dictionary<Node,List<Edge>> edges;
        public Digraph()
        {
            nodes = new HashSet<Node>();
            edges = new Dictionary<Node, List<Edge>>();
        }

        public void AddNode(Node node)
        {
            nodes.Add(node);
        }
        public void AddEdge(WeightedEdge edge)
        {
            if (nodes.Any(n => n == edge.Src) && nodes.Any(n => n == edge.Dest))
            {

            }
        }
    }*/
}