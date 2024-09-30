namespace GameLogic {
    public class Arc {
        public Point Site { get; }
        public Point Left { get; set; }
        public Point Right { get; set; }
        public Breakpoint LeftBreakpoint { get; set; }
        public Breakpoint RightBreakpoint { get; set; }

        public Arc(Point site, Breakpoint leftBreakpoint, Breakpoint rightBreakpoint) {
            Site = site;
            LeftBreakpoint = leftBreakpoint;
            RightBreakpoint = rightBreakpoint;
        }
    }
}