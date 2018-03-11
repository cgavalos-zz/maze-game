using UnityEngine;
using System.Collections.Generic;

namespace WallManagement {
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
    public float[,] qualities;
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
      qualities = new float[numRows, numCols];
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
          qualities[row, col] = cellQuality;
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
    public void BlockInterpolate(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float timeStart, float timeTaken, float finalQuality) {
      for (uint i = startingRow; i <= endingRow; i++) {
        for (uint j = startingColumn; j <= endingColumn; j++) {
          InterpData data = new InterpData();
          data.startHeightQuality = qualities[i, j];
          data.endHeightQuality = finalQuality;
          data.startTime = timeStart;
          data.endTime = timeStart + timeTaken;
          data.wallRow = i;
          data.wallCol = j;
          moveData.Add(data);
        }
      }
    }
    public void CreateClearing(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float startTime, float timeTaken) {
      BlockInterpolate(startingRow, endingRow, startingColumn, endingColumn, startTime, timeTaken, 0.0f);
    }
    public void CreateClearingNow(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float timeTaken) {
      CreateClearing(startingRow, endingRow, startingColumn, endingColumn, Time.time, timeTaken);
    }
    public void OutlineInterpolate(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float timeStart, float timeTaken, float finalQuality) {
      BlockInterpolate(startingRow, endingRow - 1, startingColumn, startingColumn, timeStart, timeTaken, finalQuality);
      BlockInterpolate(endingRow, endingRow, startingColumn, endingColumn - 1, timeStart, timeTaken, finalQuality);
      BlockInterpolate(startingRow + 1, endingRow, endingColumn, endingColumn, timeStart, timeTaken, finalQuality);
      BlockInterpolate(startingRow, startingRow, startingColumn + 1, endingColumn, timeStart, timeTaken, finalQuality);
    }
    // Does not error check. Please be careful.
    public void CreateWalledClearing(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float timeStart, float timeTaken) {
      OutlineInterpolate(startingRow, endingRow, startingColumn, endingColumn, timeStart, timeTaken, 1.0f);

      CreateClearing(startingRow + 1, endingRow - 1, startingColumn + 1, endingColumn - 1, timeStart, timeTaken);
    }
    public void CreateWalledClearingNow(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float timeTaken) {
      CreateWalledClearing(startingRow, endingRow, startingColumn, endingColumn, Time.time, timeTaken);
    }
    public void CreateClearedWalledClearing(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float timeStart, float timeTaken) {
      OutlineInterpolate(startingRow, endingRow, startingColumn, endingColumn, timeStart, timeTaken, 0.0f);
      CreateWalledClearing(startingRow + 1, endingRow - 1, startingColumn + 1, endingColumn - 1, timeStart, timeTaken);
    }
    public void CreateClearedWalledClearingNow(uint startingRow, uint endingRow, uint startingColumn, uint endingColumn, float timeTaken) {
      CreateClearedWalledClearing(startingRow, endingRow, startingColumn, endingColumn, Time.time, timeTaken);
    }
    public void Update() {
      int index = 0;
      while (index < moveData.Count) {
        InterpData data = moveData[index];
        float timeQuality = quality(Time.time, data.startTime, data.endTime);
        if (timeQuality < 0.0f) {
          // Do nothing
          //moveData.RemoveAt(index);
          //Vector3 oldPos = walls[data.wallRow, data.wallCol].transform.localPosition;
          //Vector3 newPos = new Vector3(oldPos.x, yPosFromHeightQuality(data.startHeightQuality), oldPos.z);
          //walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
        } else if (timeQuality > 1.0f) {
          moveData.RemoveAt(index);
          Vector3 oldPos = walls[data.wallRow, data.wallCol].transform.localPosition;
          Vector3 newPos = new Vector3(oldPos.x, yPosFromHeightQuality(data.endHeightQuality), oldPos.z);
          walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
          qualities[data.wallRow, data.wallCol] = data.endHeightQuality;
        } else {
          Vector3 oldPos = walls[data.wallRow, data.wallCol].transform.localPosition;
          Vector3 newPos = new Vector3(oldPos.x, yPosFromHeightQuality(linterp(timeQuality, data.startHeightQuality, data.endHeightQuality)), oldPos.z);
          walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
          index++;
        }
      }
    }
  }
}
