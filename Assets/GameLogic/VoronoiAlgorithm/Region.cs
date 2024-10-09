using System.Collections.Generic;

namespace GameLogic.VoronoiAlgorithm {
    public struct Region {
        public Point<double> site; // The site that this region is based on
        public List<Point<double>> vertices; // The vertices of this region
        
        public Region(Point<double> site) {
            this.site = site;
            this.vertices = new List<Point<double>>();
        }
        
        public void addEdge(Point<double> start, Point<double> end) {
            vertices.Add(start);
            vertices.Add(end);
        }
    }
}