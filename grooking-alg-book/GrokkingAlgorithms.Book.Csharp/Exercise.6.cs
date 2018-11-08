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
        int? FindShortestPathLength(Node<char> graph)
        {
            var queue = new Queue<Node<char>>();
            var visited = new List<Node<char>>();
            var count = 0;
            queue.Enqueue(graph);
            var lastNode = graph;
            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (lastNode != node && !lastNode.Neighbors.Contains(node))
                    count++;

                if (node.Value == 'f')
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

            Assert.Equal(2, FindShortestPathLength(graph));
        }
    }
}