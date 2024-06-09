using System;
using System.Collections.Generic;

namespace IndexesPostgreSQL
{
    public class Node<T> where T : IComparable<T>
    {
        public int KeyCount { get; set; }
        public List<T> Keys { get; set; }
        public List<Node<T>> Childs { get; set; }
        public bool IsLeaf { get; set; }
        public int Order { get; set; }

        public Node(int order)
        {
            KeyCount = 0;
            Keys = new List<T>(order);
            Childs = new List<Node<T>>(order);
            IsLeaf = true;
            Order = order;
        }

        public Node(int order, bool isLeaf)
        {
            KeyCount = 0;
            Keys = new List<T>(order);
            Childs = new List<Node<T>>(order);
            IsLeaf = isLeaf;
            Order = order;
        }

        public void Clear()
        {
            if (this.Keys != null)
                for (int i = KeyCount; i < Order; i++)
                    this.Keys[i] = default(T);
        }
    }
}
