using System;

namespace GameLogic.VoronoiAlgorithm {
    public class Arc : IComparable {
        public Point<double> Site { get; set; }
        public Arc LeftArc { get; set; }
        public Arc RightArc { get; set; }
        public Breakpoint LeftBreakpoint { get; set; }
        public Breakpoint RightBreakpoint { get; set; }
        public Event CircleEvent { get; set; }
        public Region region { get; set; }

        public Arc(Point<double> site) {
            Site = site;
        }
        
        public Arc(Point<double> site, Breakpoint leftBreakpoint, Breakpoint rightBreakpoint) {
            Site = site;
            LeftBreakpoint = leftBreakpoint;
            RightBreakpoint = rightBreakpoint;
        }

        public int CompareTo(object obj) {
            if (obj == null) return 1;

            Arc otherArc = obj as Arc;
            if (otherArc != null) {
                return Site.x.CompareTo(otherArc.Site.x);
            } else {
                throw new ArgumentException("Object is not an Arc");
            }
        }
    }
}