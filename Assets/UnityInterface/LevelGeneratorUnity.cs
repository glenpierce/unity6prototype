using System.Collections.Generic;
using DefaultNamespace;
using GameLogic;
using UnityEngine;

public class LevelGeneratorUnity : MonoBehaviour {
    public GameObject levelContainer;
    public GameObject npc;
    private bool npcIsInititalized = false;
    private Level level;
    private List<Node> path;
    private bool pathIsCalculated = false;
    void Start() {
        LevelGenerator levelGenerator = new LevelGenerator();
        level = levelGenerator.GenerateBluePrint(100 ,100);
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
    
    void Update() {
        handleMouseInput();
        moveNPCIfPathExists();
    }

    private void handleMouseInput() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                int x = Mathf.RoundToInt(hit.point.x);
                int y = Mathf.RoundToInt(hit.point.z);
                Debug.Log("Clicked on: " + x + ", " + y);
                handleMouseClick(x, y);
            }
        }
    }
    
    private void handleMouseClick(int x, int y) {
        if (npcIsInititalized == false) {
            npc = GameObject.CreatePrimitive(PrimitiveType.Cube);
            npc.transform.position = new Vector3(x, 0, y);
            npcIsInititalized = true;
        } else {
            path = PathFinding.findPath(level.getLevel(), (int) npc.transform.position.x, (int) npc.transform.position.z, x, y);
            pathIsCalculated = true;
        }
    }

    private void moveNPCIfPathExists() {
        if (pathIsCalculated) {
            moveNPC(path);
        }
    }

    private void moveNPC(List<Node> nodes) {
        if (npcIsInititalized == false) return;
        if (nodes.Count == 0) return;
        Node nextNode = nodes[0];
        Vector3 target = new Vector3(nextNode.Position.x, 0, nextNode.Position.y);
        npc.transform.position = Vector3.MoveTowards(npc.transform.position, target, 0.1f);
        if (Vector3.Distance(npc.transform.position, target) < 0.1f) {
            nodes.RemoveAt(0);
        }
    }
}
