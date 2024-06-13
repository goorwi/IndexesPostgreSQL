using System;
using System.Collections.Generic;

namespace IndexesPostgreSQL
{
    public class Node<T> where T : IComparable<T>
    {
        public List<T> Keys { get; set; }
        public List<Node<T>> Childs { get; set; }
        public bool IsLeaf { get; set; }
        public int Order { get; set; }

        public Node(int order)
        {
            Keys = new List<T>(order);
            Childs = new List<Node<T>>(order);
            IsLeaf = true;
            Order = order;
        }

        public Node(int order, bool isLeaf)
        {
            Keys = new List<T>(order);
            Childs = new List<Node<T>>(order);
            IsLeaf = isLeaf;
            Order = order;
        }
    }
}
