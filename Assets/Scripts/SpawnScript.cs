using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// My `using`s
using GridGeneration;
using CreationGeneral;

public class SpawnScript : MonoBehaviour {

  public WallManager manager;
  public float wallHeight = 1.0f;
  public float wallHorizontalDimension = 1.0f;
  public uint mazeRows = 39;
  public uint mazeCols = 39;

  public class InterpData {
    public float startHeightQuality;
    public float endHeightQuality;
    public float startTime;
    public float endTime;
    public uint wallRow;
    public uint wallCol;
  }

  public class WallManager {
    public GameObject[,] walls;
    public float wallHeight;
    public uint numRows;
    public uint numCols;
    public float wallHorizontalDimension;
    public List<InterpData> moveData;
    public GameObject mazeParent;

    public WallManager(GridGeneration.Grid grid, float wallHeight, float wallHorizontalDimension) {
      this.wallHeight = wallHeight;
      numCols = grid.numCols;
      numRows = grid.numRows;
      this.wallHeight = wallHeight;
      this.wallHorizontalDimension = wallHorizontalDimension;
      walls = new GameObject[numRows, numCols];
      moveData = new List<InterpData>();
      mazeParent = new GameObject();
      mazeParent.name = "Maze Parent";

      uint cellNum = 0;

      for (uint row = 0; row < numRows; row++) {
        for (uint col = 0; col < numCols; col++) {

          GameObject tempCell = GameObject.CreatePrimitive(PrimitiveType.Cube);
          tempCell.transform.parent = mazeParent.transform;
          tempCell.name = "Maze Part #" + cellNum.ToString();

          float cellQuality = 0.5f;
          if (grid.cells[row, col] == GridGeneration.Grid.wallValue) {
            cellQuality = 1.0f;
          } else if (grid.cells[row, col] == GridGeneration.Grid.mazeValue) {
            cellQuality = 0.0f;
          }

          tempCell.transform.localScale = new Vector3(wallHorizontalDimension,
            wallHeight, wallHorizontalDimension);
          tempCell.transform.localPosition = new Vector3(
            col * wallHorizontalDimension + wallHorizontalDimension / 2.0f,
            yPosFromHeightQuality(cellQuality),
            row * wallHorizontalDimension + wallHorizontalDimension / 2.0f);

          walls[row, col] = tempCell;
          cellNum++;
        }
      }
    }
    public float quality(float x, float a, float b) {
      return (x - a) / (b - a);
    }
    public float linterp(float quality, float a, float b) {
      return quality * (b - a) + a;
    }
    public float yPosFromHeightQuality(float heightQuality) {
      return linterp(heightQuality, -wallHeight / 2.0f, wallHeight / 2.0f);
    }
    public void Update() {
      int index = 0;
      while (index < moveData.Count) {
        InterpData data = moveData[index];
        float timeQuality = quality(Time.time, data.startTime, data.endTime);
        if (timeQuality < 0.0) {
          moveData.RemoveAt(index);
          Vector3 oldPos = walls[data.wallRow, data.wallCol].transform.localPosition;
          Vector3 newPos = new Vector3(oldPos.x, yPosFromHeightQuality(data.startHeightQuality), oldPos.z);
          walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
        } else if (timeQuality > 1.0) {
          moveData.RemoveAt(index);
          Vector3 oldPos = walls[data.wallRow, data.wallCol].transform.localPosition;
          Vector3 newPos = new Vector3(oldPos.x, yPosFromHeightQuality(data.endHeightQuality), oldPos.z);
          walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
        } else {
          Vector3 oldPos = walls[data.wallRow, data.wallCol].transform.localPosition;
          Vector3 newPos = new Vector3(oldPos.x, yPosFromHeightQuality(linterp(timeQuality, data.startHeightQuality, data.endHeightQuality)), oldPos.z);
          walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
          index++;
        }
      }
    }
  }

  // Use this for initialization
  void Start () {
    manager = new WallManager(GridGeneration.Grid.RandomizedPrim(mazeRows, mazeCols), wallHeight, wallHorizontalDimension);
    InterpData data = new InterpData();
    data.startTime = Time.time;
    data.endTime = data.startTime + 10.0f;
    data.startHeightQuality = 0.0f;
    data.endHeightQuality = 1.0f;
    data.wallRow = 0;
    data.wallCol = 0;

    manager.moveData.Add(data);
  }

  // Update is called once per frame
  void Update () {
    manager.Update();
  }
}
