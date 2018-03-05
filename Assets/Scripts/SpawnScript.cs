using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// My `using`s
using GridGeneration;
using CreationGeneral;

public class SpawnScript : MonoBehaviour {

  public float mazeHeight = 3.0f;
  public float mazeHorizontalDimension = 1.0f;
  public uint mazeXSize = 39;
  public uint mazeZSize = 39;

  // Use this for initialization
  void Start () {
    GameObject parent = new GameObject();
    parent.name = "MazeParent";

    GridGeneration.Grid grid = GridGeneration.Grid.RandomizedPrim(mazeZSize, mazeXSize);
    uint cellNum = 0;

    for (int row = 0; row < grid.m_numRows; row++) {
      for (int col = 0; col < grid.m_numCols; col++) {
        if (grid.IsWall(row, col)) {
          cellNum++;
          GameObject tempCell = GameObject.CreatePrimitive(PrimitiveType.Cube);
          tempCell.transform.parent = parent.transform;
          tempCell.name = "MazePart" + cellNum.ToString();
          tempCell.transform.localScale = new Vector3(mazeHorizontalDimension,
            mazeHeight, mazeHorizontalDimension);
          tempCell.transform.localPosition = new Vector3(
            col * mazeHorizontalDimension + mazeHorizontalDimension / 2.0f,
            mazeHeight / 2.0f,
            row * mazeHorizontalDimension + mazeHorizontalDimension / 2.0f);
        }
      }
    }
  }

  // Update is called once per frame
  void Update () {
    //
  }
}
