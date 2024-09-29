using System;

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
            GenerateRoomsAndCorridors(blueprint);

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

        private void GenerateRoomsAndCorridors(bool[,] blueprint) {
            var roomCount = 5;
            var roomSize = 5;

            for (var i = 0; i < roomCount; i++) {
                var roomX = random.Next(0, (int) mapSize.x - roomSize);
                var roomY = random.Next(0, (int) mapSize.y - roomSize);

                for (var x = roomX; x < roomX + roomSize; x++)
                for (var y = roomY; y < roomY + roomSize; y++)
                    blueprint[x, y] = true;
            }

            for (var i = 0; i < roomCount - 1; i++) {
                var startX = random.Next(0, (int) mapSize.x);
                var startY = random.Next(0, (int) mapSize.y);
                var endX = random.Next(0, (int) mapSize.x);
                var endY = random.Next(0, (int) mapSize.y);

                for (var x = Math.Min(startX, endX); x <= Math.Max(startX, endX); x++) blueprint[x, startY] = true;

                for (var y = Math.Min(startY, endY); y <= Math.Max(startY, endY); y++) blueprint[endX, y] = true;
            }
        }
    }
}