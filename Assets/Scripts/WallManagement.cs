using UnityEngine;
using System;
using System.Collections.Generic;

namespace WallManagement
{
  public class InterpData
  {
    public float startHeightQuality;
    public float endHeightQuality;
    public float startTime;
    public float endTime;
    public uint wallRow;
    public uint wallCol;
  }

  public class WallManager
  {
    public GameObject[,] walls;
    public float[,] qualities;
    public float wallHeight;
    public uint numRows;
    public uint numCols;
    public float wallHorizontalDimension;
    public List<InterpData> moveData;
    public GameObject mazeParent;

    public WallManager(GridGeneration.Grid grid, float wallHeight,
                       float wallHorizontalDimension)
    {
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

      for (uint row = 0; row < numRows; row++)
      {
        for (uint col = 0; col < numCols; col++)
        {
          GameObject tempCell = GameObject.CreatePrimitive(PrimitiveType.Cube);
          tempCell.transform.parent = mazeParent.transform;
          tempCell.name = "Maze Part #" + cellNum.ToString();

          float cellQuality = 0.0f;

          if (grid.cells[row, col] == GridGeneration.Grid.wallValue)
          {
            cellQuality = 1.0f;
          }
          else if (grid.cells[row, col] == GridGeneration.Grid.mazeValue)
          {
            cellQuality = 0.0f;
            tempCell.GetComponent<Renderer>().enabled = false;
          }

          tempCell.transform.localScale = new Vector3(wallHorizontalDimension,
                                                      wallHeight,
                                                      wallHorizontalDimension);
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

    public float quality(float x, float a, float b)
    {
      if (b - a == 0.0f)
      {
        return 1.0f;
      }
      else
      {
        return (x - a) / (b - a);
      }
    }

    public float linterp(float quality, float a, float b)
    {
      return quality * (b - a) + a;
    }

    public float yPosFromHeightQuality(float heightQuality)
    {
      return linterp(heightQuality, -wallHeight / 2.0f, wallHeight / 2.0f);
    }

    public void BlockInterpolate(uint startingRow, uint endingRow,
                                 uint startingColumn, uint endingColumn,
                                 float timeStart, float timeTaken,
                                 float finalQuality)
    {
      for (uint i = startingRow; i <= endingRow; i++)
      {
        for (uint j = startingColumn; j <= endingColumn; j++)
        {
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

    public void CreateClearing(uint startingRow, uint endingRow,
                               uint startingColumn, uint endingColumn,
                               float startTime,
                               float timeTaken)
    {
      BlockInterpolate(startingRow, endingRow, startingColumn, endingColumn,
                       startTime, timeTaken,
                       0.0f);
    }

    public void CreateClearingNow(uint startingRow, uint endingRow,
                                  uint startingColumn, uint endingColumn,
                                  float timeTaken)
    {
      CreateClearing(startingRow, endingRow, startingColumn, endingColumn,
                     Time.time,
                     timeTaken);
    }

    public void OutlineInterpolate(uint startingRow, uint endingRow,
                                   uint startingColumn, uint endingColumn,
                                   float timeStart, float timeTaken,
                                   float finalQuality)
    {
      BlockInterpolate(startingRow, endingRow - 1, startingColumn,
                       startingColumn, timeStart, timeTaken,
                       finalQuality);
      BlockInterpolate(endingRow, endingRow, startingColumn, endingColumn - 1,
                       timeStart, timeTaken,
                       finalQuality);
      BlockInterpolate(startingRow + 1, endingRow, endingColumn, endingColumn,
                       timeStart, timeTaken,
                       finalQuality);
      BlockInterpolate(startingRow, startingRow, startingColumn + 1,
                       endingColumn, timeStart, timeTaken,
                       finalQuality);
    }

    // Does not error check. Please be careful.
    public void CreateWalledClearing(uint startingRow, uint endingRow,
                                     uint startingColumn, uint endingColumn,
                                     float timeStart,
                                     float timeTaken)
    {
      OutlineInterpolate(startingRow, endingRow, startingColumn, endingColumn,
                         timeStart, timeTaken,
                         1.0f);

      CreateClearing(startingRow + 1, endingRow - 1, startingColumn + 1,
                     endingColumn - 1, timeStart,
                     timeTaken);
    }

    public void CreateWalledClearingNow(uint startingRow, uint endingRow,
                                        uint startingColumn, uint endingColumn,
                                        float timeTaken)
    {
      CreateWalledClearing(startingRow, endingRow, startingColumn, endingColumn,
                           Time.time,
                           timeTaken);
    }

    public void CreateClearedWalledClearing(uint startingRow, uint endingRow,
                                            uint startingColumn,
                                            uint endingColumn, float timeStart,
                                            float timeTaken)
    {
      OutlineInterpolate(startingRow, endingRow, startingColumn, endingColumn,
                         timeStart, timeTaken,
                         0.0f);
      CreateWalledClearing(startingRow + 1, endingRow - 1, startingColumn + 1,
                           endingColumn - 1, timeStart,
                           timeTaken);
    }

    public void CreateClearedWalledClearingNow(uint startingRow, uint endingRow,
                                               uint startingColumn,
                                               uint endingColumn,
                                               float timeTaken)
    {
      CreateClearedWalledClearing(startingRow, endingRow, startingColumn,
                                  endingColumn, Time.time,
                                  timeTaken);
    }

    public bool IsEmpty(int row, int col)
    {
      // Invalid cells are not empty.
      if (row < 0 || row >= numRows || col < 0 || col >= numCols)
      {
        return false;
      }
      else
      {
        return qualities[row, col] <= 0.0f;
      }
    }

    public bool IsEmpty2by2(int row, int col)
    {
      return IsEmpty(row, col) &&
             IsEmpty(row + 1, col) &&
             IsEmpty(row, col + 1) &&
             IsEmpty(row + 1, col + 1);
    }

    public bool EmptyEdgeCellsHaveEmptyAdjacents(int row, int col, int subRow,
                                                 int subCol)
    {
      if (subRow == 0)
      {
        if (subCol == 0)
        {
          return (!IsEmpty(row, col - 1) ||
                  (IsEmpty(row - 1, col - 1) || IsEmpty(row + 1, col - 1))) &&
                 (!IsEmpty(row - 1, col) ||
                  (IsEmpty(row - 1, col - 1) || IsEmpty(row - 1, col + 1)));
        }
        else
        {
          return (!IsEmpty(row - 1, col + 1) ||
                  (IsEmpty(row - 1, col + 2) || IsEmpty(row - 1, col))) &&
                 (!IsEmpty(row, col + 2) ||
                  (IsEmpty(row - 1, col + 2) || IsEmpty(row + 1, col + 2)));
        }
      }
      else
      {
        if (subCol == 0)
        {
          return (!IsEmpty(row + 1, col - 1) ||
                  (IsEmpty(row, col - 1) || IsEmpty(row + 2, col - 1))) &&
                 (!IsEmpty(row + 2, col) ||
                  (IsEmpty(row + 2, col - 1) || IsEmpty(row + 2, col + 1)));
        }
        else
        {
          return (!IsEmpty(row + 2, col + 1) ||
                  (IsEmpty(row + 2, col) || IsEmpty(row + 2, col + 2))) &&
                 (!IsEmpty(row + 1, col + 2) ||
                  (IsEmpty(row + 2, col + 2) || IsEmpty(row, col + 2)));
        }
      }
    }

    public bool AllEmptiesReachable(int row, int col, int subRow, int subCol)
    {
      bool[,] isEmpty = new bool[4, 4];
      bool[,] isVisited = new bool[4, 4];

      for (int tempRow = row - 1; tempRow <= row + 2; tempRow++)
      {
        for (int tempCol = col - 1; tempCol <= col + 2; tempCol++)
        {
          bool isWall = (tempRow == row + subRow && tempCol == col + subCol) ||
                        !IsEmpty(tempRow, tempCol);
          isEmpty[tempRow - (row - 1), tempCol - (col - 1)] = !isWall;
          isVisited[tempRow - (row - 1), tempCol - (col - 1)] = false;
        }
      }

      Queue<GridGeneration.Cell> toBeVisited = new Queue<GridGeneration.Cell>();

      if (subRow == 0)
      {
        toBeVisited.Enqueue(new GridGeneration.Cell(row + subRow + 1, col +
                                                    subCol));
      }
      else
      {
        toBeVisited.Enqueue(new GridGeneration.Cell(row + subRow - 1, col +
                                                    subCol));
      }

      while (toBeVisited.Count > 0)
      {
        GridGeneration.Cell thisCell = toBeVisited.Dequeue();
        isVisited[thisCell.row - row + 1, thisCell.col - col + 1] = true;

        Func<int, int, bool> CellIsInArray = (int tRow, int tCol) =>
                                             tRow >= row - 1 && tRow <= row +
                                             2 &&
                                             tCol >= col - 1 && tCol <= col + 2;
        Func<int, int, bool> CellIsEmptyInArray = (int tRow, int tCol) =>
                                                  isEmpty[tRow - row + 1,
                                                          tCol - col + 1];
        Func<int, int, bool> IfInArrayAndEmptyInArrayAndNotVisitedThenEnqueue =
          (int tRow, int tCol) => {
          if (CellIsInArray(tRow, tCol) && CellIsEmptyInArray(tRow, tCol) &&
              !isVisited[tRow - row + 1, tCol - col + 1])
          {
            toBeVisited.Enqueue(new GridGeneration.Cell(tRow, tCol));
            return true;
          }
          else
          {
            return false;
          }
        };

        IfInArrayAndEmptyInArrayAndNotVisitedThenEnqueue(thisCell.row + 1,
                                                         thisCell.col);
        IfInArrayAndEmptyInArrayAndNotVisitedThenEnqueue(thisCell.row - 1,
                                                         thisCell.col);
        IfInArrayAndEmptyInArrayAndNotVisitedThenEnqueue(thisCell.row,
                                                         thisCell.col + 1);
        IfInArrayAndEmptyInArrayAndNotVisitedThenEnqueue(thisCell.row,
                                                         thisCell.col - 1);
      }

      for (int tempRow = 0; tempRow < 4; tempRow++)
      {
        for (int tempCol = 0; tempCol < 4; tempCol++)
        {
          if (isEmpty[tempRow, tempCol] && !isVisited[tempRow, tempCol])
          {
            return false;
          }
        }
      }

      return true;
    }

    public void Fix2by2(int row, int col, System.Random randomNumberGen,
                        float timeStart,
                        float timeTaken)
    {
      if (IsEmpty2by2(row, col))
      {
        // A cell can be blocked if every empty edge cell will still have an
        // adjacent empty cell afterwards.
        List<GridGeneration.Cell> blockableCells = new List<GridGeneration.Cell>();

        for (int cbRow = 0; cbRow <= 1; cbRow++)
        {
          for (int cbCol = 0; cbCol <= 1; cbCol++)
          {
            if (AllEmptiesReachable(row, col, cbRow, cbCol))
            {
              blockableCells.Add(new GridGeneration.Cell(row + cbRow, col +
                                                         cbCol));
            }
          }
        }

        if (blockableCells.Count > 0)
        {
          GridGeneration.Cell randomCell =
            blockableCells[randomNumberGen.Next(blockableCells.Count)];
          BlockInterpolate((uint)randomCell.row, (uint)randomCell.row,
                           (uint)randomCell.col, (uint)randomCell.col,
                           timeStart, timeTaken,
                           1.0f);
          qualities[randomCell.row, randomCell.col] = 1.0f;
        }
      }
    }

    public void FixWidePathways(uint startRow, uint endRow, uint startCol,
                                uint endCol, float timeStart,
                                float timeTaken)
    {
      System.Random randomNumberGen = new System.Random();

      for (uint row = startRow; row < endRow - 1; row++)
      {
        for (uint col = startCol; col < endCol - 1; col++)
        {
          Fix2by2((int)row, (int)col, randomNumberGen, timeStart, timeTaken);
        }
      }
    }

    public void Update()
    {
      int index = 0;

      while (index < moveData.Count)
      {
        InterpData data = moveData[index];
        float timeQuality = quality(Time.time, data.startTime, data.endTime);

        if (timeQuality < 0.0f)
        {
          // Do nothing
          index++;
        }
        else if (timeQuality >= 1.0f)
        {
          moveData.RemoveAt(index);
          Vector3 oldPos =
            walls[data.wallRow, data.wallCol].transform.localPosition;
          Vector3 newPos =
            new Vector3(oldPos.x, yPosFromHeightQuality(
                          data.endHeightQuality), oldPos.z);
          walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
          qualities[data.wallRow, data.wallCol] = data.endHeightQuality;

          if (data.endHeightQuality > 0.0f)
          {
            walls[data.wallRow,
                  data.wallCol].GetComponent<Renderer>().enabled = true;
          }
          else
          {
            walls[data.wallRow,
                  data.wallCol].GetComponent<Renderer>().enabled = false;
          }
        }
        else
        {
          float heightQuality = linterp(timeQuality,
                                        data.startHeightQuality,
                                        data.endHeightQuality);

          if (heightQuality > 0.0f)
          {
            walls[data.wallRow,
                  data.wallCol].GetComponent<Renderer>().enabled = true;
          }

          Vector3 oldPos =
            walls[data.wallRow, data.wallCol].transform.localPosition;
          Vector3 newPos =
            new Vector3(oldPos.x,
                        yPosFromHeightQuality(heightQuality),
                        oldPos.z);
          walls[data.wallRow, data.wallCol].transform.localPosition = newPos;
          index++;
        }
      }
    }
  }
}
