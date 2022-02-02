using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Algo
{
    class Tree
    {

        Node Root;
        public Node CurrentNode;
        List<Node> NodeList;

        public Tree()
        {

        }

        public Boolean CheckNodeExist(decimal v)
        {
            return NodeList.Select(x => x).Where(x => x.v.Equals(v)).Any();
        }

        public void AddChild(decimal v)
        {
            CurrentNode.Childs.Add(new Tuple<Node, Int64>(new Node() { v = v }, 1));
        }

    }

    public class Node
    {
        public decimal v;
        public List<Tuple<Node, Int64>> Childs;
        public List<Node> Parents;

        public Node()
        {
            Childs = new List<Tuple<Node, Int64>>();
            Parents = new List<Node>();
        }
    }
}
