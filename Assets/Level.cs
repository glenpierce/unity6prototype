using System.Collections.Generic;

namespace DefaultNamespace {
    public class Level {
        private List<CoordinatePair<int>> levelMap = new List<CoordinatePair<int>>();

        public Level() {
            
        }
        
        public List<CoordinatePair<int>> getLevel() {
            return levelMap;
        }
        
        public void add(CoordinatePair<int> coordinatePair) {
            levelMap.Add(coordinatePair);
        }
    }
}