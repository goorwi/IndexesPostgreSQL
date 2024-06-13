using System.Collections.Generic;

namespace IndexesPostgreSQL
{
    public class NodeInfo
    {
        public List<string> Keys { get; set; } = new List<string>();
        public Position Position { get; set; }
        public List<Position> ChildPositions { get; set; } = new List<Position>();
        public bool IsLeaf { get; set; }
        public int Height { get; set; }
        public int ChildIndex { get; set; }
    }
}
