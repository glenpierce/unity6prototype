using System.Collections.Generic;

namespace GameLogic {
    public class PriorityQueue<T> {
        private List<KeyValuePair<T, double>> elements = new List<KeyValuePair<T, double>>();

        public int Count => elements.Count;

        public void enqueue(T item, double priority) {
            elements.Add(new KeyValuePair<T, double>(item, priority));
        }

        public T dequeue() {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++) {
                if (elements[i].Value < elements[bestIndex].Value) {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Key;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }

        public bool contains(T item) {
            for (int i = 0; i < elements.Count; i++) {
                if (elements[i].Key.Equals(item)) {
                    return true;
                }
            }
            return false;
        }
    }
}