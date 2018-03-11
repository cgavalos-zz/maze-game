using System.Collections.Generic;

namespace GridGeneration
{
  public class Cell
  {
    public int row;
    public int col;

    public Cell(int row, int col)
    {
      this.row = row;
      this.col = col;
    }
  }

  public class Grid
  {
    public const int wallValue = 0;
    public const int mazeValue = 1;

    public int[,] cells;
    public uint numRows;
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

    public bool IsValidCell(int row, int col)
    {
      return row >= 0 && col >= 0 && row < numRows && col < numCols;
    }

    public bool IsValidCell(Cell c)
    {
      return IsValidCell(c.row, c.col);
    }

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

    public bool MarkCell(Cell c)
    {
      return MarkCell(c.row, c.col);
    }

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

    public List<Cell> GetWalls(Cell c)
    {
      return GetWalls(c.row, c.col);
    }

    public bool IsVisited(int row, int col)
    {
      return !IsValidCell(row, col) || cells[row, col] == mazeValue;
    }

    public bool IsVisited(Cell c)
    {
      return IsVisited(c.row, c.col);
    }

    public bool IsMaze(int row, int col)
    {
      return IsValidCell(row, col) && cells[row, col] == mazeValue;
    }

    public bool IsMaze(Cell c)
    {
      return IsMaze(c.row, c.col);
    }

    public bool IsWall(int row, int col)
    {
      return !IsValidCell(row, col) || cells[row, col] == wallValue;
    }

    public bool IsWall(Cell c)
    {
      return IsWall(c.row, c.col);
    }

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

    public List<Cell> DividedCells(Cell cellPosition)
    {
      return DividedCells(cellPosition.row, cellPosition.col);
    }

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
  }
}
