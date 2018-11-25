using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Graph = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, int>>;

namespace GrokkingAlgorithms.Book.Csharp
{
    class FindShortestPath
    {
        private static Graph FromGraph(Vertex root)
        {
            var graph = new Dictionary<string, Dictionary<string, int>>();
            var yetToProcess = new Queue<Vertex>();
            var processed = new List<string>();
            yetToProcess.Enqueue(root);
            while (yetToProcess.Any())
            {
                var current = yetToProcess.Dequeue();
                if (processed.Contains(current.Name))
                    continue;

                graph.TryAdd(current.Name, new Dictionary<string, int>());
                foreach (var edge in current.Connections)
                {
                    graph[current.Name].Add(edge.To.Name, edge.Weight);
                    if (!yetToProcess.Contains(edge.To))
                        yetToProcess.Enqueue(edge.To);
                }

                processed.Add(current.Name);
            }

            return graph;
        }


        public static int Find(Vertex root)
        {
            var graphVertexCost = FromGraph(root);
            var costs = new Dictionary<string, int>();
            var parents = new Dictionary<string, string>();
            costs.Add(root.Name, 0);
            root.Connections.ForEach(e =>
            {
                costs.Add(e.To.Name, e.Weight);
                parents.Add(e.To.Name,root.Name);
            });
            foreach (var value in graphVertexCost)
            {
                costs.TryAdd(value.Key, int.MaxValue);
                parents.TryAdd(value.Key, null);
            }

            var closestNode = costs.OrderBy(x => x.Value).FirstOrDefault();
            var processed = new List<string>();
            while (!closestNode.Equals(default(KeyValuePair<string, int>)))
            {
                var cost = costs[closestNode.Key];
                foreach (var edge in graphVertexCost[closestNode.Key])
                {
                    var newCost = cost + edge.Value;
                    if (costs[edge.Key] > newCost)
                    {
                        costs[edge.Key] = newCost;
                        parents[edge.Key] = closestNode.Key;
                    }
                }

                processed.Add(closestNode.Key);
                closestNode = costs.OrderBy(x => x.Value).FirstOrDefault(x => !processed.Contains(x.Key));
            }

            return costs["finish"];
        }
    }

    public class Exercise7
    {
        [Fact]
        public void A()
        {
            var finish = new Vertex("finish");
            var vertex4 = new Vertex("4", new Tuple<Vertex, int>(finish, 1));
            var vertex5 = new Vertex("5", new Tuple<Vertex, int>(finish, 3), new Tuple<Vertex, int>(vertex4, 6));
            var vertex3 = new Vertex("3", new Tuple<Vertex, int>(vertex4, 2), new Tuple<Vertex, int>(vertex5, 4));
            var vertex2 = new Vertex("2", new Tuple<Vertex, int>(vertex3, 8), new Tuple<Vertex, int>(vertex4, 7));
            var start = new Vertex("start", new Tuple<Vertex, int>(vertex2, 2), new Tuple<Vertex, int>(vertex3, 5));

            Assert.Equal(8, FindShortestPath.Find(start));
        }
    }
}