using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace IndexesPostgreSQL
{
    public partial class DrawBox : PictureBox
    {
        private IEnumerable<NodeInfo> treeNodes;
        private TreeConfiguration configuration;

        public DrawBox()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer,
                true);
        }

        public void Print<TTree>(ITree tree) where TTree : ITree
        {
            treeNodes = tree.GetAllNodes();
            configuration = tree.GetConfiguration();

            // Увеличиваем ширину блока для каждого узла дерева
            AdjustBlockWidth(treeNodes, configuration);

            Invalidate();
        }

        private void AdjustBlockWidth(IEnumerable<NodeInfo> nodes, TreeConfiguration config)
        {
            var font = new Font("Times New Roman", 12);
            int maxTextWidth = 0;

            using (Graphics g = CreateGraphics())
            {
                foreach (var node in nodes)
                {
                    string keys = string.Join(", ", node.Keys);
                    var stringSize = g.MeasureString(keys, font);
                    if (stringSize.Width > maxTextWidth)
                    {
                        maxTextWidth = (int)stringSize.Width;
                    }
                }
            }

            // Устанавливаем ширину блока, учитывая отступы
            config.BlockWidth = maxTextWidth + 20;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            if (treeNodes == null || !treeNodes.Any())
            {
                return;
            }

            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            base.OnPaint(pe);

            // Получаем самую левую и самую правую позиции узлов
            int minX = treeNodes.Min(n => n.Position.X);
            int maxX = treeNodes.Max(n => n.Position.X) + configuration.BlockWidth;

            // Вычисляем ширину дерева
            int treeWidth = maxX - minX;

            // Базовое смещение для центрирования дерева
            int baseOffset = (Width - treeWidth) / 2 - minX;

            foreach (var node in treeNodes)
            {
                if (node.ChildPositions != null && node.ChildPositions.Any())
                {
                    DrawConnectionArrow(node.Position, node.ChildPositions, baseOffset, pe.Graphics);
                }

                DrawNode(node, baseOffset, pe.Graphics);
            }
        }

        private void DrawConnectionArrow(Position fromNodePosition, List<Position> toNodePositions, int offset, Graphics graphics)
        {
            Pen linePen = new Pen(Color.Black, 1)
            {
                CustomEndCap = new AdjustableArrowCap(3, 3) // Настройка стрелочки на конце линии
            };

            var startPoint = new Point
            {
                X = fromNodePosition.X + offset + configuration.BlockWidth / 2,
                Y = fromNodePosition.Y + configuration.BlockHeight
            };

            foreach (var toNodePosition in toNodePositions)
            {

                var endPoint = new Point
                {
                    X = toNodePosition.X + offset + configuration.BlockWidth / 2,
                    Y = toNodePosition.Y
                };

                graphics.DrawLine(linePen, startPoint, endPoint);
            }
        }

        private void DrawNode(NodeInfo node, int offset, Graphics graphics)
        {
            var rect = new Rectangle(node.Position.X + offset, node.Position.Y, configuration.BlockWidth, configuration.BlockHeight);

            graphics.FillRectangle(Brushes.White, rect);
            graphics.DrawRectangle(Pens.Black, rect);

            string keys = string.Join(", ", node.Keys);
            var font = new Font("Times New Roman", 12);
            var stringSize = graphics.MeasureString(keys, font);

            graphics.DrawString(
                keys,
                font,
                Brushes.Black,
                node.Position.X + (configuration.BlockWidth / 2) - (stringSize.Width / 2) + offset,
                node.Position.Y + (configuration.BlockHeight / 2) - (stringSize.Height / 2)
            );
        }
    }
}
