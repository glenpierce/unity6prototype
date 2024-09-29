using System;
using System.Collections.Generic;

namespace DefaultNamespace {
    public class LevelGenerator {
        private int seed;
        private Random random = new Random();
        private CoordinatePair<int> mapSize;

        public Level GenerateBluePrint(int sizeX, int sizeY) {
            mapSize = new CoordinatePair<int>(sizeX, sizeY);
            seed = new Random().Next();
            Level level = new Level();
            var blueprint = new bool[(int) mapSize.x, (int) mapSize.y];

            // Step 1: Initialize the blueprint with all false values
            for (var x = 0; x < mapSize.x; x++) {
                for (var y = 0; y < mapSize.y; y++) {
                    blueprint[x, y] = false;
                }
            }

            // Step 2: Generate rooms and corridors
            generateRoomsAndCorridors(blueprint);

            // Convert the 2D array to a list of Vector2 positions for walls
            for (var x = 0; x < mapSize.x; x++) {
                for (var y = 0; y < mapSize.y; y++) {
                    if (!blueprint[x, y]) {
                        level.add(new CoordinatePair<int>(x, y));
                    }
                }
            }

            return level;
        }

        private void generateRoomsAndCorridors(bool[,] blueprint) {
            int roomCount = 5;
            int roomSize = 5;
            List<CoordinatePair<int>> roomPositions = new List<CoordinatePair<int>>();

            generateRooms(blueprint, roomCount, roomSize, roomPositions);
            generateCorridors(blueprint, roomPositions);
        }
        
        private void generateRooms(bool[,] blueprint, int roomCount, int roomSize, List<CoordinatePair<int>> roomPositions) {
            for (int i = 0; i < roomCount; i++) {
                int roomX = random.Next(0, (int) mapSize.x - roomSize);
                int roomY = random.Next(0, (int) mapSize.y - roomSize);

                roomPositions.Add(new CoordinatePair<int>(roomX, roomY));

                for (int x = roomX; x < roomX + roomSize; x++) {
                    for (int y = roomY; y < roomY + roomSize; y++) {
                        blueprint[x, y] = true;
                    }
                }
            }
        }

        private void generateCorridors(bool[,] blueprint, List<CoordinatePair<int>> roomPositions) {
            for (int i = 0; i < roomPositions.Count - 1; i++) {
                int startX = roomPositions[i].x;
                int startY = roomPositions[i].y;
                int endX = roomPositions[i + 1].x;
                int endY = roomPositions[i + 1].y;

                for (int x = Math.Min(startX, endX); x <= Math.Max(startX, endX); x++) blueprint[x, startY] = true;

                for (int y = Math.Min(startY, endY); y <= Math.Max(startY, endY); y++) blueprint[endX, y] = true;
            }
        }
    }
}