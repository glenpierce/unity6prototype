using System;

namespace GameLogic {
    public class Node : IEquatable<Node> {
        public Point<int> Position { get; }

        public Node(int x, int y) {
            Position = new Point<int>(x, y);
        }

        public bool Equals(Node other) {
            return other != null && Position.x == other.Position.x && Position.y == other.Position.y;
        }

        public override int GetHashCode() {
            return Position.x.GetHashCode() ^ Position.y.GetHashCode();
        }
    }
}