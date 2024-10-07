using System;

namespace GameLogic.VoronoiAlgorithm {
    public class AVLTree<T> where T : IComparable {
        public AVLNode<T> root { get; set; }

        public void Insert(T value) {
            root = Insert(root, value);
        }

        public void Delete(T value) {
            root = Delete(root, value);
        }

        public AVLNode<T> Find(T value) {
            return Find(root, value);
        }

        private AVLNode<T> Insert(AVLNode<T> node, T value) {
            if (node == null) return new AVLNode<T>(value);

            int cmp = value.CompareTo(node.value);
            if (cmp < 0) {
                node.leftAVLNode = Insert(node.leftAVLNode, value);
            } else if (cmp > 0) {
                node.rightAVLNode = Insert(node.rightAVLNode, value);
            } else {
                return node;
            }

            node.height = 1 + Math.Max(Height(node.leftAVLNode), Height(node.rightAVLNode));
            return balance(node);
        }

        private AVLNode<T> Delete(AVLNode<T> node, T value) {
            if (node == null) return node;

            int cmp = value.CompareTo(node.value);
            if (cmp < 0) {
                node.leftAVLNode = Delete(node.leftAVLNode, value);
            } else if (cmp > 0) {
                node.rightAVLNode = Delete(node.rightAVLNode, value);
            } else {
                if (node.leftAVLNode == null || node.rightAVLNode == null) {
                    node = (node.leftAVLNode ?? node.rightAVLNode);
                } else {
                    AVLNode<T> temp = minValueNode(node.rightAVLNode);
                    node.value = temp.value;
                    node.rightAVLNode = Delete(node.rightAVLNode, temp.value);
                }
            }

            if (node == null) return node;

            node.height = 1 + Math.Max(Height(node.leftAVLNode), Height(node.rightAVLNode));
            return balance(node);
        }

        private AVLNode<T> Find(AVLNode<T> node, T value) {
            if (node == null) return null;

            int cmp = value.CompareTo(node.value);
            if (cmp < 0) {
                return Find(node.leftAVLNode, value);
            } else if (cmp > 0) {
                return Find(node.rightAVLNode, value);
            } else {
                return node;
            }
        }

        private AVLNode<T> minValueNode(AVLNode<T> node) {
            AVLNode<T> current = node;
            while (current.leftAVLNode != null) {
                current = current.leftAVLNode;
            }
            return current;
        }

        private int Height(AVLNode<T> node) {
            return node?.height ?? 0;
        }

        private int balanceFactor(AVLNode<T> node) {
            return node == null ? 0 : Height(node.leftAVLNode) - Height(node.rightAVLNode);
        }

        private AVLNode<T> balance(AVLNode<T> node) {
            int balance = balanceFactor(node);

            if (balance > 1) {
                if (balanceFactor(node.leftAVLNode) < 0) {
                    node.leftAVLNode = rotateLeft(node.leftAVLNode);
                }
                return rotateRight(node);
            }

            if (balance < -1) {
                if (balanceFactor(node.rightAVLNode) > 0) {
                    node.rightAVLNode = rotateRight(node.rightAVLNode);
                }
                return rotateLeft(node);
            }

            return node;
        }

        private AVLNode<T> rotateRight(AVLNode<T> y) {
            AVLNode<T> x = y.leftAVLNode;
            AVLNode<T> T2 = x.rightAVLNode;

            x.rightAVLNode = y;
            y.leftAVLNode = T2;

            y.height = Math.Max(Height(y.leftAVLNode), Height(y.rightAVLNode)) + 1;
            x.height = Math.Max(Height(x.leftAVLNode), Height(x.rightAVLNode)) + 1;

            return x;
        }

        private AVLNode<T> rotateLeft(AVLNode<T> x) {
            AVLNode<T> y = x.rightAVLNode;
            AVLNode<T> T2 = y.leftAVLNode;

            y.leftAVLNode = x;
            x.rightAVLNode = T2;

            x.height = Math.Max(Height(x.leftAVLNode), Height(x.rightAVLNode)) + 1;
            y.height = Math.Max(Height(y.leftAVLNode), Height(y.rightAVLNode)) + 1;

            return y;
        }
    }
}