using DefaultNamespace;
using GameLogic;
using UnityEngine;

public class LevelGeneratorUnity : MonoBehaviour {
    public GameObject levelContainer;
    void Start() {
        LevelGenerator levelGenerator = new LevelGenerator();
        Level level = levelGenerator.GenerateBluePrint(100 ,100);
        int[,] levelMap = level.getLevel();
        for (int x = 0; x < levelMap.GetLength(0); x++) {
            for (int y = 0; y < levelMap.GetLength(1); y++) {
                if (levelMap[x, y] == 1) {
                    createCubeAtPosition(x, y);
                }
            }
        }
    }

    void createCubeAtPosition(int x, int y) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(x, 0, y);
        cube.transform.parent = levelContainer.transform;
    }
}
