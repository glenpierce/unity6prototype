namespace GameLogic {
    public class Arc {
        public Point<double> Site { get; }
        public Point<double> Left { get; set; }
        public Point<double> Right { get; set; }
        public Breakpoint LeftBreakpoint { get; set; }
        public Breakpoint RightBreakpoint { get; set; }

        public Arc(Point<double> site, Breakpoint leftBreakpoint, Breakpoint rightBreakpoint) {
            Site = site;
            LeftBreakpoint = leftBreakpoint;
            RightBreakpoint = rightBreakpoint;
        }
    }
}