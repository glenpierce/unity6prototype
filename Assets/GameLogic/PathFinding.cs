using System;
using System.Collections.Generic;

namespace GameLogic {
    public class PathFinding {

        public static List<Node> FindPath(int[,] levelMap, int startX, int startY, int endX, int endY) {
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
            fScore[startNode] = heuristicCostEstimate(startNode, endNode);

            while (openSet.Count > 0) {
                Node current = openSet.Dequeue();

                if (current.Equals(endNode)) {
                    return reconstructPath(cameFrom, current);
                }

                closedSet[current.X, current.Y] = true;

                foreach (Node neighbor in getNeighbors(current, levelMap)) {
                    processNeighbor(neighbor, current, endNode, closedSet, openSet, cameFrom, gScore, fScore, levelMap);
                }
            }

            return null; // Return null if no path is found
        }
        
        private static void processNeighbor(Node neighbor, Node current, Node endNode, bool[,] closedSet, PriorityQueue<Node> openSet, Dictionary<Node, Node> cameFrom, Dictionary<Node, float> gScore, Dictionary<Node, float> fScore, int[,] levelMap) {
            if (closedSet[neighbor.X, neighbor.Y]) return;

            float tentativeGScore = gScore[current] + distBetween(current, neighbor);

            if (!openSet.Contains(neighbor)) {
                openSet.Enqueue(neighbor, tentativeGScore);
            } else if (tentativeGScore >= gScore[neighbor]) {
                return;
            }

            cameFrom[neighbor] = current;
            gScore[neighbor] = tentativeGScore;
            fScore[neighbor] = gScore[neighbor] + heuristicCostEstimate(neighbor, endNode);
        }

        private static float heuristicCostEstimate(Node a, Node b) {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private static float distBetween(Node a, Node b) {
            return 1; // Assuming all edges have the same weight
        }

        private static IEnumerable<Node> getNeighbors(Node node, int[,] levelMap) {
            List<Node> neighbors = new List<Node>();
            int width = levelMap.GetLength(0);
            int height = levelMap.GetLength(1);

            if (node.X - 1 >= 0 && levelMap[node.X - 1, node.Y] == 0) neighbors.Add(new Node(node.X - 1, node.Y));
            if (node.X + 1 < width && levelMap[node.X + 1, node.Y] == 0) neighbors.Add(new Node(node.X + 1, node.Y));
            if (node.Y - 1 >= 0 && levelMap[node.X, node.Y - 1] == 0) neighbors.Add(new Node(node.X, node.Y - 1));
            if (node.Y + 1 < height && levelMap[node.X, node.Y + 1] == 0) neighbors.Add(new Node(node.X, node.Y + 1));

            return neighbors;
        }

        private static List<Node> reconstructPath(Dictionary<Node, Node> cameFrom, Node current) {
            List<Node> totalPath = new List<Node> { current };
            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                totalPath.Add(current);
            }
            totalPath.Reverse();
            return totalPath;
        }
    }
}
