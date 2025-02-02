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

        public AVLNode<T> predecessor() {
            if (leftAVLNode != null) {
                AVLNode<T> node = leftAVLNode;
                while (node.rightAVLNode != null) {
                    node = node.rightAVLNode;
                }

                return node;
            }

            return null;
        }
        
        public AVLNode<T> successor() {
            if (rightAVLNode != null) {
                AVLNode<T> node = rightAVLNode;
                while (node.leftAVLNode != null) {
                    node = node.leftAVLNode;
                }

                return node;
            }

            return null;
        }
    }
}