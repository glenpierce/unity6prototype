using DefaultNamespace;
using UnityEngine;

public class LevelGeneratorUnity : MonoBehaviour {
    public GameObject levelContainer;
    void Start() {
        LevelGenerator levelGenerator = new LevelGenerator();
        Level level = levelGenerator.GenerateBluePrint(100 ,100);
        foreach (var coordinatePair in level.getLevel()) {
            createCubeAtPosition(coordinatePair.x, coordinatePair.y);
        }
    }

    void createCubeAtPosition(int x, int y) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(x, 0, y);
        cube.transform.parent = levelContainer.transform;
    }
}
