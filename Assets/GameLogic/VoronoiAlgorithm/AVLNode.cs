using System;

namespace GameLogic.VoronoiAlgorithm {
    public class AVLNode<T> where T : IComparable {
        public T value { get; set; }
        public AVLNode<T> leftAVLNode { get; set; }
        public AVLNode<T> rightAVLNode { get; set; }
        public int height { get; set; }

        public AVLNode(T value) {
            this.value = value;
            height = 1;
        }
    }
}