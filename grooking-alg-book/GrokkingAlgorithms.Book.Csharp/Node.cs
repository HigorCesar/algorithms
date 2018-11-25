using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace GrokkingAlgorithms.Book.Csharp
{
    class Node<T>
    {
        public Node(T value)
        {
            Value = value;
        }

        public T Value { get; }
        public List<Node<T>> Neighbors { get; set; } = new List<Node<T>>();
    }

    public class Edge
    {
        public Edge(Vertex from, Vertex to, int weight)
        {
            From = new Vertex(from.Name);
            To = to;
            Weight = weight;
        }

        public Vertex From { get; }
        public Vertex To { get; }
        public int Weight { get; }
    }

    public class Vertex
    {
        public string Name { get; }
        public readonly ImmutableList<Edge> Connections;

        public Vertex(string name)
        {
            Name = name;
            Connections = ImmutableList<Edge>.Empty;
        }

        private Vertex(string name, IEnumerable<Edge> connections)
        {
            Name = name;
            Connections = connections.ToImmutableList();
        }

        public Vertex(string name,params Tuple<Vertex, int>[] connections)
        {
            Name = name;
            Connections = connections.Select(g => new Edge(this, g.Item1, g.Item2)).ToImmutableList();
        }


        public Vertex AddConnection(Vertex edge, int weight)
        {
            return new Vertex(Name, Connections.Add(new Edge(this, edge, weight)));
        }

        public Vertex AddConnections(params Tuple<Vertex, int>[] connections)
        {
            return new Vertex(Name, Connections.AddRange(connections.Select(g => new Edge(this, g.Item1, g.Item2))));
        }
    }
}