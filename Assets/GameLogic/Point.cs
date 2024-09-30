namespace GameLogic {
    public struct Point<T> where T : struct {
        public T x, y;

        public Point(T x, T y) {
            this.x = x;
            this.y = y;
        }
    }
}