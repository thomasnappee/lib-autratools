using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord;
using Accord.IO;
using Accord.Statistics.Kernels;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Core.StateMachine
{
    public class StateMachine
    {

        public class Node
        {
            private string mark;
            private Node parent;
            private List<Node> nodes = new();
            public Node(string mark) => this.mark = mark;

            public Node AddChild(string mark)
            {
                var node = new Node(mark)
                {
                    parent = this
                };
                nodes.Add(node);
                return node;
            }

            public List<string> BuildString()
            {
                var list = new List<string>();
                list = buildRecursive(list);
                return list;
            }

            private List<string> buildRecursive(List<string> list)
            {
                if (nodes.Count == 0)
                {
                    var node = this;
                    StringBuilder sb = new(node.mark);
                    while (node.parent != null)
                    {
                        node = node.parent;
                        sb.Append(node.mark);
                    }

                    list.Add(sb.ToString());
                }
                else
                {
                    foreach (var node in nodes)
                    {
                        node.buildRecursive(list);
                    }
                }

                return list;
            }
        }

        private int counter = 0;
        private List<Tuple<int, Func<int>>> functions = new();
        private Dictionary<string, int> stateByStringId = new();
        public int NumberOfStates => counter;

        private void BuildAllStringIdCombinations()
        {
            /*
             * Given : "abc", "ab", "a"
             * functions = { "abc", func }, { "ab", func }, ...
             * We compute all combinations of strings like :
             * - "aaa", "aba", "baa", "bba", "caa", "cba"
             * stateByStringId = { "aaa", 0 }, { "aba", 1 }, ...
             */
            
            var keys = functions.Select(t => GetStringFromNumber(t.Item1));
            var indexes = new int[keys.Count()];

            int numberOfCombinations = 1;
            foreach (var k in keys)
            {
                try
                {
                    numberOfCombinations = checked(numberOfCombinations * k.Length);
                }

                catch (OverflowException e)
                {
                    throw new OverflowException($"Trop d'état peuvent être calculés. Le nombre maximum d'états est {int.MaxValue}");
                }
            }

            Node node = new Node("\0");
            recursiveBuild(node, new Stack<string>(keys));

            var combinations = node.BuildString();
            foreach (var combination in combinations)
            {
                stateByStringId.Add(combination, counter++);
            }
            
        }

        public void Build() => BuildAllStringIdCombinations();

        public string GetStringFromNumber(int number)
        {
            StringBuilder sb = new();
            for (int i = 1; i <= number; i++) sb.Append(Char.ConvertFromUtf32(i));
            return sb.ToString();
        }
        
        public void Register(Func<int> func, int numberOfStates)
        {
            if (func == null || numberOfStates <= 0) return;
            functions.Add(new Tuple<int, Func<int>>(numberOfStates, func));
        }

        public int GetState()
        {
            var sb = new StringBuilder();
            foreach (var t in functions)
            {
                sb.Append(Char.ConvertFromUtf32(t.Item2.Invoke()));
            }
            return stateByStringId[sb.ToString()];
        }

        private void recursiveBuild(Node node, Stack<string> keys)
        {
            if (keys.Count == 0) return;
            foreach (var c in keys.Pop())
            {
                recursiveBuild(node.AddChild(c.ToString()), keys.DeepClone());
            }
        }
    }
}