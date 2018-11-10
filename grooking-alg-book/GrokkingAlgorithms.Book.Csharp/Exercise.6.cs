using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GrokkingAlgorithms.Book.Csharp
{
    class Node<T>
    {
        public Node()
        {
        }

        public Node(T value)
        {
            Value = value;
        }

        public T Value { get; }
        public List<Node<T>> Neighbors { get; set; } = new List<Node<T>>();
    }

    public class Exercise6Tests
    {
        int? FindShortestPathLength<T>(Node<T> graph, T value) where T : IComparable<T>
        {
            var queue = new Queue<Node<T>>();
            var visited = new List<Node<T>>();
            var count = 0;
            queue.Enqueue(graph);
            var lastNode = graph;
            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (lastNode != node && !lastNode.Neighbors.Contains(node))
                    count++;
                
                if (EqualityComparer<T>.Default.Equals(node.Value, value))
                    return count;

                lastNode = node;
                visited.Add(node);
                foreach (var neighbor in node.Neighbors.Where(x => !visited.Contains(x)))
                    queue.Enqueue(neighbor);
            }

            return null;
        }

        [Fact]
        public void Exercise_6_1()
        {
            var finish = new Node<char>('f');
            var thirdEdge = new Node<char>(' ');
            var graph = new Node<char>('s')
            {
                Neighbors = new List<Node<char>>
                {
                    new Node<char>(' ')
                    {
                        Neighbors = new List<Node<char>>
                        {
                            thirdEdge, finish
                        }
                    },
                    new Node<char>(' ')
                    {
                        Neighbors = new List<Node<char>>
                        {
                            thirdEdge,
                            new Node<char>(' ')
                            {
                                Neighbors = new List<Node<char>>
                                {
                                    finish
                                }
                            }
                        }
                    }
                }
            };

            Assert.Equal(2, FindShortestPathLength(graph, 'f'));
        }

        [Fact]
        public void Exercise_6_2()
        {
            var cab = new Node<string>("cab");
            var car = new Node<string>("car");
            var cat = new Node<string>("cat");
            var mat = new Node<string>("mat");
            var bat = new Node<string>("bat");
            var bar = new Node<string>("bar");

            cab.Neighbors.AddRange(new[]
            {
                cat, car
            });
            car.Neighbors.AddRange(new[]
            {
                cat, bar
            });
            cat.Neighbors.AddRange(new[]
            {
                mat, bat
            });
            mat.Neighbors.Add(bat);
            bar.Neighbors.Add(bat);

            Assert.Equal(2, FindShortestPathLength(cab, "bat"));
        }
    }
}