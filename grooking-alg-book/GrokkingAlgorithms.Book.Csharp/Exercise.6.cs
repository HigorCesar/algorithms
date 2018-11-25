using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GrokkingAlgorithms.Book.Csharp
{
    public class Exercise6Tests
    {
        int? FindShortestPathLength<T>(Node<T> graph, T value)
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

        IEnumerable<T> FindParentNodes<T>(Node<T> graph, T value)
        {
            var visited = new List<Node<T>>();
            var parentNodes = new Dictionary<T, T>();
            var queue = new Queue<Node<T>>();
            queue.Enqueue(graph);
            parentNodes.Add(graph.Value, default(T));
            while (queue.Any())
            {
                var current = queue.Dequeue();

                if (EqualityComparer<T>.Default.Equals(current.Value, value))
                    break;

                foreach (var neighbor in current.Neighbors.Where(n => !visited.Contains(n)))
                {
                    parentNodes.Add(neighbor.Value, current.Value);
                    queue.Enqueue(neighbor);
                }
            }

            if (!parentNodes.ContainsKey(value) || parentNodes.Count == 1)
                return Enumerable.Empty<T>();

            var key = parentNodes[value];
            var keyParents = new List<T>();
            while (!EqualityComparer<T>.Default.Equals(key, default(T)))
            {
                keyParents.Add(key);
                key = parentNodes[key];
            }

            return keyParents;
        }

        bool IsListValid(List<char> candidate)
        {
            var wakeUp = new Node<char>('w');
            var brushTeeth = new Node<char>('b');
            var shower = new Node<char>('s');
            var eatBreakfast = new Node<char>('e');

            wakeUp.Neighbors.AddRange(new[]
            {
                shower, brushTeeth
            });
            brushTeeth.Neighbors.Add(eatBreakfast);

            var isValid = true;
            for (var i = 0; i < candidate.Count && isValid; i++)
            {
                isValid &= FindParentNodes(wakeUp, candidate[i]).All(p => candidate.IndexOf(p) < i);
            }

            return isValid;
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

        [Fact]
        public void Exercise_6_3_1()
        {
            var given = new List<char>
            {
                'w', 's', 'e', 'b'
            };

            Assert.False(IsListValid(given));
        }

        [Fact]
        public void Exercise_6_3_2()
        {
            var given = new List<char>
            {
                'w', 'b', 'e', 's'
            };

            Assert.True(IsListValid(given));
        }

        [Fact]
        public void Exercise_6_3_3()
        {
            var given = new List<char>
            {
                's', 'w', 'b', 'e'
            };

            Assert.False(IsListValid(given));
        }
    }
}