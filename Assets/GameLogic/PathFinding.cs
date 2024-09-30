using System;
using System.Collections.Generic;

namespace GameLogic {
    public class PathFinding {

        public static List<Node> findPath(int[,] levelMap, int startX, int startY, int endX, int endY) {
            int width = levelMap.GetLength(0);
            int height = levelMap.GetLength(1);
            bool[,] closedSet = new bool[width, height];
            PriorityQueue<Node> openSet = new PriorityQueue<Node>();
            Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
            Dictionary<Node, float> gScore = new Dictionary<Node, float>();
            Dictionary<Node, float> fScore = new Dictionary<Node, float>();

            Node startNode = new Node(startX, startY);
            Node endNode = new Node(endX, endY);

            openSet.enqueue(startNode, 0);
            gScore[startNode] = 0;
            fScore[startNode] = heuristicCostEstimate(startNode, endNode);

            while (openSet.Count > 0) {
                Node current = openSet.dequeue();

                if (current.Equals(endNode)) {
                    return reconstructPath(cameFrom, current);
                }

                closedSet[current.Position.x, current.Position.y] = true;

                foreach (Node neighbor in getNeighbors(current, levelMap)) {
                    processNeighbor(neighbor, current, endNode, closedSet, openSet, cameFrom, gScore, fScore, levelMap);
                }
            }

            return null; // Return null if no path is found
        }
        
        private static void processNeighbor(Node neighbor, Node current, Node endNode, bool[,] closedSet, PriorityQueue<Node> openSet, Dictionary<Node, Node> cameFrom, Dictionary<Node, float> gScore, Dictionary<Node, float> fScore, int[,] levelMap) {
            if (closedSet[neighbor.Position.x, neighbor.Position.y]) return;

            float tentativeGScore = gScore[current] + distBetween(current, neighbor);

            if (!openSet.contains(neighbor)) {
                openSet.enqueue(neighbor, tentativeGScore);
            } else if (tentativeGScore >= gScore[neighbor]) {
                return;
            }

            cameFrom[neighbor] = current;
            gScore[neighbor] = tentativeGScore;
            fScore[neighbor] = gScore[neighbor] + heuristicCostEstimate(neighbor, endNode);
        }

        private static float heuristicCostEstimate(Node a, Node b) {
            return Math.Abs(a.Position.x - b.Position.x) + Math.Abs(a.Position.y - b.Position.y);
        }

        private static float distBetween(Node a, Node b) {
            return 1; // Assuming all edges have the same weight
        }

        private static IEnumerable<Node> getNeighbors(Node node, int[,] levelMap) {
            List<Node> neighbors = new List<Node>();
            int width = levelMap.GetLength(0);
            int height = levelMap.GetLength(1);

            if (node.Position.x - 1 >= 0 && levelMap[node.Position.x - 1, node.Position.y] == 0) neighbors.Add(new Node(node.Position.x - 1, node.Position.y));
            if (node.Position.x + 1 < width && levelMap[node.Position.x + 1, node.Position.y] == 0) neighbors.Add(new Node(node.Position.x + 1, node.Position.y));
            if (node.Position.y - 1 >= 0 && levelMap[node.Position.x, node.Position.y - 1] == 0) neighbors.Add(new Node(node.Position.x, node.Position.y - 1));
            if (node.Position.y + 1 < height && levelMap[node.Position.x, node.Position.y + 1] == 0) neighbors.Add(new Node(node.Position.x, node.Position.y + 1));

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
