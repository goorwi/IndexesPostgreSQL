using System;
using System.Collections.Generic;
using System.Linq;

namespace IndexesPostgreSQL
{
    public class BTree<T> : BaseTree<T> where T : IComparable<T>
    {
        private readonly int Order;
        private readonly bool isUnique;
        public BTree(TreeConfiguration configuration, int order, bool isUnique) : base(configuration)
        {
            this.configuration = configuration;
            this.Order = order;
            this.root = new Node<T>(order);
            this.isUnique = isUnique;
        }

        #region InsertValue
        public override void Insert(T value)
        {
            if (isUnique && !GetAllKeys().Contains(value))
            {
                if (root.Keys.Count == Order && !CanInsert(root, value))
                {
                    Node<T> newRoot = new Node<T>(Order, false);
                    newRoot.Childs.Add(root);
                    SplitChild(newRoot, 0);
                    root = newRoot;
                }

                if (!root.IsLeaf)
                    UpdateNodeKeys(root);
                InsertNonFull(root, value);
                if (!root.IsLeaf)
                    UpdateNodeKeys(root);

                root = BalanceTree(root);

                if (!root.IsLeaf)
                    UpdateNodeKeys(root);

                while (!IsBalanced())
                {
                    RebuildTree();
                }

                if (!root.IsLeaf)
                    UpdateNodeKeys(root);
            }
            else if (!isUnique)
            {
                if (root.Keys.Count == Order && !CanInsert(root, value))
                {
                    Node<T> newRoot = new Node<T>(Order, false);
                    newRoot.Childs.Add(root);
                    SplitChild(newRoot, 0);
                    root = newRoot;
                }

                if (!root.IsLeaf)
                    UpdateNodeKeys(root);
                InsertNonFull(root, value);
                if (!root.IsLeaf)
                    UpdateNodeKeys(root);

                root = BalanceTree(root);

                if (!root.IsLeaf)
                    UpdateNodeKeys(root);

                while (!IsBalanced())
                {
                    RebuildTree();
                }

                if (!root.IsLeaf)
                    UpdateNodeKeys(root);
            }
        }

        private bool CanInsert(Node<T> node, T value)
        {
            if (node == root && node.Childs.Count == 0) return false;
            else if (node.IsLeaf && node.Keys.Count == Order) return false;
            else if (node.IsLeaf && node.Keys.Count < Order) return true;
            else
            {
                int i = node.Keys.Count - 1;
                while (i >= 0 && node.Keys[i].CompareTo(value) > 0)
                {
                    i--;
                }
                if (i < 0) i++;
                if (node.Childs[i].Keys.Count == Order && node.Childs[i].IsLeaf)
                {
                    if (node.Keys.Count == Order)
                        return false;
                    else
                        return true;
                }
                return CanInsert(node.Childs[i], value);
            }
        }

        private void InsertNonFull(Node<T> node, T value)
        {
            if (node.IsLeaf)
            {
                node.Keys.Add(value);
                node.Keys.Sort();
            }
            else
            {
                int i = node.Keys.Count - 1;
                while (i >= 0 && node.Keys[i].CompareTo(value) >= 0)
                {
                    i--;
                }
                if (i < 0) i++;
                while (i + 1 < node.Keys.Count && node.Keys[i].CompareTo(value) <= 0 && node.Keys[i + 1].CompareTo(value) == 0)
                {
                    if (i + 1 < node.Keys.Count && node.Keys[i].CompareTo(value) <= 0 && node.Keys[i + 1].CompareTo(value) == 0 && CanInsert(node.Childs[i], value) && node.Childs[i + 1].Keys.Count > 1) break;
                    else if (i + 1 < node.Keys.Count && node.Keys[i].CompareTo(value) <= 0 && node.Keys[i + 1].CompareTo(value) == 0) i++;
                }
                if (node.Childs[i].Keys.Count == Order && !CanInsert(node.Childs[i], value))
                {
                    SplitChild(node, i);
                    if (node.Keys[i].CompareTo(value) <= 0)
                    {
                        i++;
                    }
                }
                InsertNonFull(node.Childs[i], value);
            }
        }
        #endregion

        #region RemoveValue
        public override void Remove(T value)
        {

            Remove(root, value);
            if (root.Keys.Count == 0)
            {
                if (root.Childs.Count == 1)
                {
                    root = root.Childs[0];
                    root.IsLeaf = true;
                }
                else
                    return;
            }

            if (!root.IsLeaf)
                UpdateNodeKeys(root);

            // Проверяем баланс дерева после удаления элемента
            root = BalanceTree(root);

            if (!root.IsLeaf)
                UpdateNodeKeys(root);

            while (!IsBalanced())
            {
                RebuildTree();
            }

            if (!root.IsLeaf)
                UpdateNodeKeys(root);
        }

        private void Remove(Node<T> node, T value)
        {
            if (node.IsLeaf)
            {
                _ = node.Keys.Remove(value);
                return;
            }

            int idx = 0;
            while (idx < node.Keys.Count && value.CompareTo(node.Keys[idx]) > 0)
            {
                idx++;
            }

            if (idx < node.Keys.Count && value.CompareTo(node.Keys[idx]) == 0)
            {
                // Нашли элемент для удаления
                Remove(node.Childs[idx], value);
            }
            else
            {
                idx = node.Keys.Count - 1;
                while (idx >= 0 && value.CompareTo(node.Keys[idx]) <= 0)
                    idx--;
                // Рекурсивно ищем элемент для удаления в нужном поддереве
                Remove(node.Childs[idx], value);
            }

            // Проверяем баланс текущего узла
            if (node.Childs[idx].Keys.Count == 0 || node.Childs[idx].Keys.Count >= Order)
            {
                Fill(node, idx);
            }
        }
        #endregion

        #region Spliting&Merging&etc.
        private void UpdateNodeKeys(Node<T> node)
        {
            if (!node.IsLeaf)
            {
                node.Keys.Clear();
                for (int i = 0; i < node.Childs.Count; i++)
                {
                    UpdateNodeKeys(node.Childs[i]);
                    node.Keys.Add(node.Childs[i].Keys[0]);
                }
            }
        }

        private void Fill(Node<T> node, int idx)
        {
            if (idx != 0 && node.Childs[idx - 1].Keys.Count >= Order)
            {
                // Перемещаем ключ из предыдущего узла
                node.Childs[idx].Keys.Insert(0, node.Keys[idx - 1]);
                node.Keys[idx - 1] = node.Childs[idx - 1].Keys.Last();
                node.Childs[idx - 1].Keys.RemoveAt(node.Childs[idx - 1].Keys.Count - 1);
            }
            else if (idx != node.Childs.Count - 1 && node.Childs[idx + 1].Keys.Count >= Order)
            {
                // Перемещаем ключ из следующего узла
                node.Childs[idx].Keys.Add(node.Keys[idx]);
                node.Keys[idx] = node.Childs[idx + 1].Keys.First();
                node.Childs[idx + 1].Keys.RemoveAt(0);
            }
            else
            {
                // Слияние узлов
                if (idx != node.Childs.Count - 1)
                {
                    Merge(node, idx);
                }
                else
                {
                    Merge(node, idx - 1);
                }
            }
        }

        private void Merge(Node<T> node, int idx)
        {
            Node<T> child = node.Childs[idx];
            Node<T> sibling = node.Childs[idx + 1];

            // Переносим ключи и детей из соседнего узла
            child.Keys.AddRange(sibling.Keys);
            child.Childs.AddRange(sibling.Childs);

            // Удаляем ключ из текущего узла
            node.Keys.RemoveAt(idx);

            // Удаляем соседний узел
            node.Childs.RemoveAt(idx + 1);
        }

        private void SplitChild(Node<T> node, int i)
        {
            int order = Order;
            Node<T> y = node.Childs[i];
            Node<T> z = new Node<T>(order, y.IsLeaf);

            z.Keys.AddRange(y.Keys.GetRange(order - 1, order - 2));
            y.Keys.RemoveRange(order - 1, order - 2);

            if (!y.IsLeaf)
            {
                z.Childs.AddRange(y.Childs.GetRange(order - 1, order - 2));
                y.Childs.RemoveRange(order - 1, order - 2);
            }

            node.Childs.Insert(i + 1, z);
        }
        #endregion

        #region Traverse
        public override IEnumerable<NodeInfo> GetAllNodes()
        {
            var nodeCollection = new List<Node<T>>();
            GetAllNodes(root, nodeCollection);

            var nodeInfos = nodeCollection.ToDictionary(
                node => node,
                node => new NodeInfo
                {
                    Keys = node.Keys.Select(x => x.ToString()).ToList(),
                    IsLeaf = node.IsLeaf
                });

            // Начальное смещение - 0, глубина - 0
            _ = CalculateNodePositions(root, nodeInfos, 0, 0);
            AggregateChildNodePosition(root, null, nodeInfos);

            foreach (var node in nodeCollection)
            {
                nodeInfos[node].Height = GetHeight(node);
            }

            return nodeInfos.Values;
        }

        private int GetHeight(Node<T> root)
        {
            int height = 0;
            if (root != null)
            {
                for (int i = 0; i < root.Childs.Count; i++)
                {
                    int maxHeight = GetHeight(root.Childs[i]);
                    height = maxHeight + 1;
                }
            }
            return height;
        }
        #endregion

        #region BalanceTree
        public bool IsBalanced()
        {
            if (root == null)
                return true;

            return CheckNode(root) && CheckLeafLevels(root, 0, out int _) && IsSorted();
        }

        private bool IsSorted()
        {
            var keys = GetAllKeys();
            for (var i = 0; i < keys.Count - 1; i++)
            {
                if (keys[i].CompareTo(keys[i + 1]) > 0)
                    return false;
            }
            return true;
        }

        private Node<T> BalanceTree(Node<T> node)
        {
            if (node == null)
                return null;

            // Если у узла только один ребенок, поднимаем его вверх
            while (node.Childs.Count == 1)
            {
                node = node.Childs[0];
            }

            if (node.IsLeaf && node.Keys.Count > Order)
            {
                var newRoot = new Node<T>(Order, false);
                newRoot.Childs.Add(node);
                SplitChild(newRoot, 0); // Изменяем на SplitChild для корректной работы
                node = newRoot;

                UpdateNodeKeys(node);
            }
            else
            {
                // Проверяем все поддеревья
                for (int i = 0; i < node.Childs.Count; i++)
                {
                    node.Childs[i] = BalanceTree(node.Childs[i]);
                    UpdateNodeKeys(node);

                    // Проверяем, если поддерево после балансировки стало слишком большим, разрезаем его
                    if (node.Childs[i].Keys.Count > Order)
                    {
                        SplitChild(node, i); // Изменяем на SplitChild для корректной работы
                        UpdateNodeKeys(node);
                    }
                }
            }

            return node;
        }

        private bool CheckNode(Node<T> node)
        {
            int minKeys = 1;
            int maxKeys = Order;

            if (node.Keys.Count < minKeys || node.Keys.Count > maxKeys)
                return false;

            if (!node.IsLeaf)
            {
                if (node.Childs.Count != node.Keys.Count)
                    return false;

                foreach (var child in node.Childs)
                {
                    if (!CheckNode(child))
                        return false;
                }
            }

            return true;
        }

        private bool CheckLeafLevels(Node<T> node, int level, out int leafLevel)
        {
            if (node.IsLeaf)
            {
                leafLevel = level;
                return true;
            }

            int firstLeafLevel = -1;

            for (int i = 0; i < node.Childs.Count; i++)
            {
                if (!CheckLeafLevels(node.Childs[i], level + 1, out int currentLeafLevel))
                {
                    leafLevel = -1;
                    return false;
                }

                if (firstLeafLevel == -1)
                {
                    firstLeafLevel = currentLeafLevel;
                }
                else if (firstLeafLevel != currentLeafLevel)
                {
                    leafLevel = -1;
                    return false;
                }
            }

            leafLevel = firstLeafLevel;
            return true;
        }

        public List<T> GetAllKeys()
        {
            List<T> keys = new List<T>();
            CollectKeys(root, keys);
            return keys;
        }

        private void CollectKeys(Node<T> node, List<T> keys)
        {
            if (node == null) return;

            if (!node.IsLeaf)
                for (int i = 0; i < node.Childs.Count; i++)
                {
                    CollectKeys(node.Childs[i], keys);
                }
            else
                node.Keys.ForEach(x => keys.Add(x));
        }

        public void Clear()
        {
            root = new Node<T>(Order, true);
        }

        public void RebuildTree()
        {
            List<T> keys = GetAllKeys();
            Clear();

            foreach (var key in keys)
            {
                Insert(key);
            }
        }
        #endregion

        #region SearchValue
        public override List<T> Search(T value)
        {
            List<T> results = new List<T>();
            Search(root, value, results);
            return results;
        }

        private void Search(Node<T> node, T value, List<T> results)
        {
            if (node == null) return;

            if (!node.IsLeaf)
            {
                for (int i = 0; i < node.Childs.Count; i++)
                    if (node.Keys[i].CompareTo(value) <= 0)
                        Search(node.Childs[i], value, results);
            }
            else
            {
                for (int i = 0; i < node.Keys.Count; i++)
                    if (node.Keys[i].CompareTo(value) == 0)
                        results.Add(node.Keys[i]);
            }
        }
        #endregion
    }
}
