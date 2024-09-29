using System;

namespace GameLogic {
    public class Level {
        private int[,] levelMap;

        public Level(int sizeX, int sizeY) {
            levelMap = new int[sizeX, sizeY];
        }

        public int[,] getLevel() {
            return levelMap;
        }

        public void add(int x, int y, int value) {
            if (IsWithinBounds(x, y)) {
                levelMap[x, y] = value;
            } else {
                throw new ArgumentOutOfRangeException("Coordinates are out of bounds.");
            }
        }

        private bool IsWithinBounds(int x, int y) {
            return x >= 0 && x < levelMap.GetLength(0) && y >= 0 && y < levelMap.GetLength(1);
        }
    }
}