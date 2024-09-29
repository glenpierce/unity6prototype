using DefaultNamespace;
using UnityEngine;

public class LevelGeneratorUnity : MonoBehaviour {
    void Start() {
        LevelGenerator levelGenerator = new LevelGenerator();
        Level level = levelGenerator.GenerateBluePrint(10 ,10);
        foreach (var coordinatePair in level.getLevel()) {
            Debug.Log("CoordinatePair: " + coordinatePair.x + ", " + coordinatePair.y);
        }
    }

    void Update() {
        
    }
}
