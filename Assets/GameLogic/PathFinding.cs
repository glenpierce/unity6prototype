using System;
using System.Collections.Generic;

namespace GameLogic {
    public class PathFinding {

        public static void FindPath(int[,] levelMap, int startX, int startY, int endX, int endY) {
            int width = levelMap.GetLength(0);
            int height = levelMap.GetLength(1);
            bool[,] closedSet = new bool[width, height];
            PriorityQueue<Node> openSet = new PriorityQueue<Node>();
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, float> gScore = new Dictionary<Node, float>();
            Dictionary<Node, float> fScore = new Dictionary<Node, float>();

            Node startNode = new Node(startX, startY);
            Node endNode = new Node(endX, endY);

            openSet.Enqueue(startNode, 0);
            gScore[startNode] = 0;
            fScore[startNode] = HeuristicCostEstimate(startNode, endNode);

            while (openSet.Count > 0) {
                Node current = openSet.Dequeue();

                if (current.Equals(endNode)) {
                    ReconstructPath(cameFrom, current);
                    return;
                }

                closedSet[current.X, current.Y] = true;

                foreach (Node neighbor in GetNeighbors(current, levelMap)) {
                    if (closedSet[neighbor.X, neighbor.Y]) continue;

                    float tentativeGScore = gScore[current] + DistBetween(current, neighbor);

                    if (!openSet.Contains(neighbor)) {
                        openSet.Enqueue(neighbor, tentativeGScore);
                    } else if (tentativeGScore >= gScore[neighbor]) {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, endNode);
                }
            }

            return;
        }

        private static float HeuristicCostEstimate(Node a, Node b) {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private static float DistBetween(Node a, Node b) {
            return 1; // Assuming all edges have the same weight
        }

        private static IEnumerable<Node> GetNeighbors(Node node, int[,] levelMap) {
            List<Node> neighbors = new List<Node>();
            int width = levelMap.GetLength(0);
            int height = levelMap.GetLength(1);

            if (node.X - 1 >= 0 && levelMap[node.X - 1, node.Y] == 0) neighbors.Add(new Node(node.X - 1, node.Y));
            if (node.X + 1 < width && levelMap[node.X + 1, node.Y] == 0) neighbors.Add(new Node(node.X + 1, node.Y));
            if (node.Y - 1 >= 0 && levelMap[node.X, node.Y - 1] == 0) neighbors.Add(new Node(node.X, node.Y - 1));
            if (node.Y + 1 < height && levelMap[node.X, node.Y + 1] == 0) neighbors.Add(new Node(node.X, node.Y + 1));

            return neighbors;
        }

        private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current) {
            List<Node> totalPath = new List<Node> { current };
            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                totalPath.Add(current);
            }
            totalPath.Reverse();
            return totalPath;
        }
    }

    public class PriorityQueue<T> {
        private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

        public int Count => elements.Count;

        public void Enqueue(T item, float priority) {
            elements.Add(new KeyValuePair<T, float>(item, priority));
        }

        public T Dequeue() {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++) {
                if (elements[i].Value < elements[bestIndex].Value) {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Key;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }

        public bool Contains(T item) {
            for (int i = 0; i < elements.Count; i++) {
                if (elements[i].Key.Equals(item)) {
                    return true;
                }
            }
            return false;
        }
    }
}
