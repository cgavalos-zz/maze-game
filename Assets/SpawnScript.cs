using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// My `using`s
using GridGeneration;
using CreationGeneral;

public class SpawnScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    GridGeneration.Grid grid = GridGeneration.Grid.RandomizedPrim(39, 39);
    uint cellNum = 0;

    for (int row = 0; row < grid.m_numRows; row++) {
      for (int col = 0; col < grid.m_numCols; col++) {
        if (grid.IsWall(row, col)) {
          cellNum++;
          GameObject tempCell = GameObject.CreatePrimitive(PrimitiveType.Cube);
          tempCell.name = "MazePart" + cellNum.ToString();
          tempCell.transform.localPosition = new Vector3(col * 1.0f + 0.5f, 0.5f, row * 1.0f + 0.5f);
        }
      }
    }
  }

  // Update is called once per frame
  void Update () {
    //
  }
}
