using System;
using System.Collections.Generic;

namespace IndexesPostgreSQL
{
    public interface ITree
    {
        IEnumerable<NodeInfo> GetAllNodes();
        TreeConfiguration GetConfiguration();
    }

    public interface ITree<T> : ITree where T : IComparable<T>
    {
        List<T> Search(T value);
        void Insert(T value);
        void Remove(T value);
    }
}
