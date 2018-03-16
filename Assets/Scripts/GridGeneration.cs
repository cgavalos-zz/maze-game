using System.Collections.Generic;
using System;
using UnityEngine;

namespace GridGeneration
{
  /// @brief Holds the position of a cell. Used instead of a Tuple due to Unity
  /// using an earlier version of C#.
  ///
  public class Cell
  {
    /// The row of the cell.
    ///
    public int row;
    /// The column of the cell.
    ///
    public int col;

    public Cell(int row, int col)
    {
      this.row = row;
      this.col = col;
    }
  }

  public class Grid
  {
    /// Holds the value that is used to denote a wall in the cells array.
    ///
    public const int wallValue = 0;
    /// Holds the value that is used to denote a maze section (floor) in the
    /// cells array.
    ///
    public const int mazeValue = 1;

    /// Holds the wall/floor status of each cell.
    ///
    public int[,] cells;
    /// The number of rows of cells.
    ///
    public uint numRows;
    /// The number of columns of cells.
    ///
    public uint numCols;

    public Grid(uint numRows, uint numCols)
    {
      this.numRows = numRows;
      this.numCols = numCols;
      cells = new int[numRows, numCols];

      for (int row = 0; row < numRows; row++)
      {
        for (int col = 0; col < numCols; col++)
        {
          cells[row, col] = wallValue;
        }
      }
    }

    /// @brief Tests whether the cell is inside the grid.
    /// @param row [in] The row of the cell.
    /// @param col [in] The column of the cell.
    ///
    public bool IsValidCell(int row, int col)
    {
      return row >= 0 && col >= 0 && row < numRows && col < numCols;
    }

    /// @brief Tests whether the cell is inside the grid.
    /// @param c [in] The tested Cell.
    ///
    public bool IsValidCell(Cell c)
    {
      return IsValidCell(c.row, c.col);
    }

    /// @brief Marks a cell as a floor (part of the maze).
    /// @param row [in] The row of the cell.
    /// @param col [in] The column of the cell.
    ///
    public bool MarkCell(int row, int col)
    {
      if (IsValidCell(row, col))
      {
        cells[row, col] = mazeValue;
        return true;
      }
      else
      {
        return false;
      }
    }

    // @brief Marks a cell as a floor (part of the maze).
    /// @param c [in] The targetted Cell.
    ///
    public bool MarkCell(Cell c)
    {
      return MarkCell(c.row, c.col);
    }

    /// @brief Gets the walls adjacent to the given cell.
    /// @param row [in] The row of the cell.
    /// @param col [in] The column of the cell.
    ///
    public List<Cell> GetWalls(int row, int col)
    {
      List<Cell> walls = new List<Cell>();

      for (int wallRow = row - 1; wallRow <= row + 1; wallRow++)
      {
        for (int wallCol = col - 1; wallCol <= col + 1; wallCol++)
        {
          // Valid cell above, below, to the left, or the right of the current
          // cell that is a wall.
          if (IsValidCell(wallRow, wallCol) &&
              (wallRow == row || wallCol == col) &&
              !(wallRow == row && wallRow == col) && cells[wallRow, wallCol] ==
              wallValue)
          {
            walls.Add(new Cell(wallRow, wallCol));
          }
        }
      }

      return walls;
    }

    /// @brief Gets the walls adjacent to the given cell.
    /// @param c [in] The given Cell.
    ///
    public List<Cell> GetWalls(Cell c)
    {
      return GetWalls(c.row, c.col);
    }

    /// @brief Tests if the cell is "visited" by the criteria of Prim's
    /// algorithm.
    /// @param row [in] The row of the cell.
    /// @param col [in] The column of the cell.
    ///
    public bool IsVisited(int row, int col)
    {
      return !IsValidCell(row, col) || cells[row, col] == mazeValue;
    }

    /// @brief Tests if the cell is "visited" by the criteria of Prim's
    /// algorithm.
    /// @param c [in] The tested Cell.
    ///
    public bool IsVisited(Cell c)
    {
      return IsVisited(c.row, c.col);
    }

    /// @brief Tests if a cell is part of the maze by the criteria of Prim's
    /// algorithm.
    /// @param row [in] The row of the cell.
    /// @param col [in] The column of the cell.
    ///
    public bool IsMaze(int row, int col)
    {
      return IsValidCell(row, col) && cells[row, col] == mazeValue;
    }

    /// @brief Tests if a cell is part of the maze by the criteria of Prim's
    /// algorithm.
    /// @param c [in] The tested Cell.
    ///
    public bool IsMaze(Cell c)
    {
      return IsMaze(c.row, c.col);
    }

    /// @brief Tests if a cell is a wall by the criteria of Prim's algorithm.
    /// @param row [in] The row of the cell.
    /// @param col [in] The column of the cell.
    ///
    public bool IsWall(int row, int col)
    {
      return !IsValidCell(row, col) || cells[row, col] == wallValue;
    }

    /// @brief Tests if a cell is a wall by the criteria of Prim's algorithm.
    /// @param c [in] The tested Cell.
    ///
    public bool IsWall(Cell c)
    {
      return IsWall(c.row, c.col);
    }

    /// @brief Returns the cells in the maze that are divided by the given cell.
    /// @param row [in] The row of the cell.
    /// @param col [in] The column of the cell.
    ///
    public List<Cell> DividedCells(int row, int col)
    {
      List<Cell> dividedCells = new List<Cell>();

      for (int cellRow = row - 1; cellRow <= row + 1; cellRow++)
      {
        for (int cellCol = col - 1; cellCol <= col + 1; cellCol++)
        {
          // Valid cell above, below, to the left, or the right of the current
          // cell that is a maze cell.
          if (IsValidCell(cellRow, cellCol) &&
              (cellRow == row || cellCol == col) &&
              !(cellRow == row && cellCol == col) &&
              cells[cellRow, cellCol] == mazeValue)
          {
            dividedCells.Add(new Cell(cellRow, cellCol));
            dividedCells.Add(new Cell(row - (cellRow - row),
                                      col - (cellCol - col)));
            return dividedCells;
          }
        }
      }

      return dividedCells;
    }

    /// @brief Returns the cells in the maze that are divided by the given cell.
    ///
    public List<Cell> DividedCells(Cell cellPosition)
    {
      return DividedCells(cellPosition.row, cellPosition.col);
    }

    /// @brief Generates a maze grid using a randomized Prim's algorithm.
    /// @param numRows [in] The number of rows in the final grid.
    /// @param numCols [in] The number of columns in the final grid.
    /// @param startingRow [in] The row in the grid that the algorithm
    /// generates from.
    /// @param startingRow [in] The column in the grid that the algorithm
    /// generates from.
    ///
    public static Grid RandomizedPrim(uint numRows, uint numCols,
                                      int startingRow = 0,
                                      int startingCol = 0)
    {
      // Start with a grid full of walls.
      Grid grid = new Grid(numRows, numCols);
      // Pick a cell, mark it as part of the maze. Add the walls of the cell to
      // the wall list.
      Cell startingCell = new Cell(startingRow, startingCol);
      bool successfulMark = grid.MarkCell(startingCell);
      List<Cell> wallList = grid.GetWalls(startingCell);
      System.Random randomNumberGen = new System.Random();

      if (successfulMark)
      {
        // While there are walls in the list:
        while (wallList.Count > 0)
        {
          // Pick a random wall from the list.
          int randomIndex = randomNumberGen.Next(wallList.Count);
          Cell randomWall = wallList[randomIndex];
          // If only one of the two cells that the wall divides is visited, then:
          List<Cell> dividedCells = grid.DividedCells(randomWall);

          if (!(grid.IsVisited(dividedCells[0]) &&
                grid.IsVisited(dividedCells[1])))
          {
            // Make the wall a passage and mark the unvisited cell as part of the
            // maze.
            grid.MarkCell(randomWall);

            // Add the neighboring walls of the cell to the wall list.
            if (grid.IsVisited(dividedCells[0]))
            {
              grid.MarkCell(dividedCells[1]);
              wallList.AddRange(grid.GetWalls(dividedCells[1]));
            }
            else
            {
              grid.MarkCell(dividedCells[0]);
              wallList.AddRange(grid.GetWalls(dividedCells[0]));
            }
          }

          // Remove the wall from the list.
          wallList.Remove(randomWall);
        }
      }

      return grid;
    }

    public uint GetNumWalls()
    {
      uint num = 0;

      for (uint row = 0; row < numRows; row++)
      {
        for (uint col = 0; col < numCols; col++)
        {
          if (cells[row, col] == mazeValue)
          {
            num++;
          }
        }
      }

      return num;
    }

    public uint GetNumCells()
    {
      return numRows * numCols;
    }

    public uint GetNumMazeCells()
    {
      return GetNumCells() - GetNumWalls();
    }

    public List<Cell> GetAllWalls()
    {
      List<Cell> wallList = new List<Cell>();

      for (uint row = 0; row < numRows; row++)
      {
        for (uint col = 0; col < numCols; col++)
        {
          if (cells[row, col] == wallValue)
          {
            wallList.Add(new Cell((int)row, (int)col));
          }
        }
      }

      return wallList;
    }

    public List<Cell> GetAllMazeCells()
    {
      List<Cell> cellList = new List<Cell>();

      for (uint row = 0; row < numRows; row++)
      {
        for (uint col = 0; col < numCols; col++)
        {
          if (cells[row, col] == mazeValue)
          {
            cellList.Add(new Cell((int)row, (int)col));
          }
        }
      }

      return cellList;
    }

    /// @brief Reduces the number of walls in the maze by a factor of the
    /// decimation ratio. Decimation ratios outside of [0, 1] are ignored.
    /// @param desiredDecimationRatio [in] The ratio of the number of wall cells
    /// after decimation to the number of wall cells before decimation.
    /// @param actualRatioAllowedHigh [in] Whether the actual decimation ratio
    /// is allowed to be higher than the given decimation
    /// @returns The actual decimation ratio.
    ///
    public double Decimate(double desiredDecimationRatio,
                           bool actualRatioAllowedHigh)
    {
      if (0.0 <= desiredDecimationRatio && desiredDecimationRatio <= 1.0)
      {
        uint numWalls = GetNumWalls();
        double numWallsAfterDecimation = numWalls * desiredDecimationRatio;
        Debug.Log(numWallsAfterDecimation);
        Debug.Log(numWalls);
        uint numWallsToRemove;

        if (actualRatioAllowedHigh)
        {
          numWallsToRemove = numWalls - (uint)Math.Ceiling(
            numWallsAfterDecimation);
        }
        else
        {
          numWallsToRemove = numWalls - (uint)Math.Floor(
            numWallsAfterDecimation);
        }


        Debug.Log(numWallsToRemove);

        if (numWallsToRemove > 0)
        {
          List<Cell> walls = GetAllWalls();
          Debug.Log(walls.Count);

          System.Random randomNumberGen = new System.Random();

          for (uint i = 0; i < numWallsToRemove; i++)
          {
            int removedWallIndex = randomNumberGen.Next(walls.Count);
            Cell removedWallCell = walls[removedWallIndex];
            walls.RemoveAt(removedWallIndex);
            cells[removedWallCell.row, removedWallCell.col] = mazeValue;
          }
        }

        return (double)numWallsAfterDecimation / (double)numWalls;
      }

      return 1.0;
    }
  }
}
