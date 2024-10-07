namespace GameLogic.VoronoiAlgorithm {
    public class Event {
        public Point<double> Point { get; }
        public EventType Type { get; }
        
        public Arc Arc { get; set; }

        public Event(Point<double> point, EventType type) {
            Point = point;
            Type = type;
        }

        public Event(Point<double> point, EventType type, Arc arc) {
            Point = point;
            Type = type;
            Arc = arc;
        }

    }
}