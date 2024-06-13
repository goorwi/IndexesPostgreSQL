using System;
using System.Collections.Generic;

namespace IndexesPostgreSQL
{
    public abstract class BaseTree<T> : ITree<T> where T : IComparable<T>
    {
        protected Node<T> root;
        protected TreeConfiguration configuration;

        public BaseTree(TreeConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public abstract List<T> Search(T value);

        public abstract void Insert(T value);

        public abstract void Remove(T value);

        public abstract IEnumerable<NodeInfo> GetAllNodes();

        protected int CalculateNodePositions(Node<T> root, IDictionary<Node<T>, NodeInfo> nodeInfos, int offset, int depth)
        {
            if (root == null)
            {
                return 0;
            }

            int nodeWidth = configuration.BlockWidth; // Ширина узла
            int totalWidth = 0; // Общая ширина поддерева

            // Рекурсивно вычисляем ширину для каждого дочернего узла
            List<int> childrenWidths = new List<int>();
            for (int i = 0; i < root.Childs.Count; i++)
            {
                int childWidth = CalculateNodePositions(root.Childs[i], nodeInfos, offset + totalWidth, depth + 1);
                childrenWidths.Add(childWidth);
                totalWidth += childWidth;
                if (i < root.Childs.Count - 1)
                {
                    totalWidth += configuration.HorizontalSpacing;
                }
            }

            // Если у узла нет дочерних узлов, его ширина равна ширине узла
            if (totalWidth == 0)
            {
                totalWidth = nodeWidth;
            }

            // Вычисляем и устанавливаем позицию текущего узла
            int nodeX = offset + (totalWidth - nodeWidth) / 2;
            int nodeY = depth * (configuration.BlockHeight + configuration.VerticalSpacing);

            nodeInfos[root].Position = new Position
            {
                X = nodeX,
                Y = nodeY
            };

            return totalWidth;
        }

        protected void AggregateChildNodePosition(Node<T> root, Node<T> parent, IDictionary<Node<T>, NodeInfo> nodeInfos)
        {
            if (root == null)
            {
                return;
            }

            if (parent != null)
            {
                nodeInfos[parent].ChildPositions.Add(nodeInfos[root].Position);
            }

            foreach (var child in root.Childs)
            {
                AggregateChildNodePosition(child, root, nodeInfos);
            }
        }

        protected void GetAllNodes(Node<T> root, ICollection<Node<T>> collection)
        {
            if (root == null)
            {
                return;
            }

            collection.Add(root);

            for (int i = 0; i < root.Childs.Count; i++)
            {
                GetAllNodes(root.Childs[i], collection);
            }
        }

        public TreeConfiguration GetConfiguration()
        {
            return configuration;
        }
    }
}
