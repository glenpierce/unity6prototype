using System;

namespace GameLogic {
    public class Node : IEquatable<Node> {
        public int X { get; }
        public int Y { get; }

        public Node(int x, int y) {
            X = x;
            Y = y;
        }

        public bool Equals(Node other) {
            return other != null && X == other.X && Y == other.Y;
        }

        public override int GetHashCode() {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}