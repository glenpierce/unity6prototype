using GameLogic;

namespace UnityInterface {
    public class UnityLogger : LoggerInterface {
        public void Log(string message) {
            UnityEngine.Debug.Log(message);
        }
    }
}