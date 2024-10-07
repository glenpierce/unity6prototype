namespace GameLogic.VoronoiAlgorithm {
    public class Breakpoint {
        public Point<double> LeftPoint { get; }
        public Point<double> RightPoint { get; }

        public Breakpoint(Point<double> leftPoint, Point<double> rightPoint) {
            LeftPoint = leftPoint;
            RightPoint = rightPoint;
        }
    }
}