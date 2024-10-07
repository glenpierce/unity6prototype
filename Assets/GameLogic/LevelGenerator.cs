using System;
using System.Collections.Generic;
using DefaultNamespace;
using GameLogic.VoronoiAlgorithm;
using Random = System.Random;

namespace GameLogic {
    public class LevelGenerator {
        private int seed;
        private Random random;
        private CoordinatePair<int> mapSize;
        private LoggerInterface logger;
        
        public LevelGenerator(LoggerInterface logger) {
            this.logger = logger;
        }
        
        public List<Region> generateVoronoi(int sizeX, int sizeY, int numRegions) {
            seed = new Random().Next();
            random = new Random(seed);
            
            List<Point<double>> points = new List<Point<double>>();
            for (int i = 0; i < numRegions; i++) {
                for (int j = 0; j < numRegions; j++) {
                    Point<double> point = new Point<double>(random.Next(0, sizeX), random.Next(0, sizeY));
                    logger.Log("Point: " + point.x + ", " + point.y);
                    points.Add(point);
                }
            }
            
            
            Voronoi voronoi = new Voronoi(points, logger);
            List<Region> regions = voronoi.generate();
            return regions;
        }

        public Level generateBluePrint(int sizeX, int sizeY) {
            mapSize = new CoordinatePair<int>(sizeX, sizeY);
            seed = new Random().Next();
            random = new Random(seed);
            Level level = new Level(sizeX, sizeY);
            var blueprint = new bool[(int) mapSize.x, (int) mapSize.y];

            for (var x = 0; x < mapSize.x; x++) {
                for (var y = 0; y < mapSize.y; y++) {
                    blueprint[x, y] = false;
                }
            }

            generateRoomsAndCorridors(blueprint);

            for (var x = 0; x < mapSize.x; x++) {
                for (var y = 0; y < mapSize.y; y++) {
                    if (!blueprint[x, y]) {
                        level.add(x, y, 1);
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
            random = new Random(seed);
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