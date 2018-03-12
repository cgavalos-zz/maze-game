using UnityEngine;

namespace CreationGeneral
{
  public class Creation : MonoBehaviour
  {
    /// @brief Creates cubes with varying dimensions based on their number.
    ///
    public static void VaryingDimensions()
    {
      for (int i = 0; i < 10; i++)
      {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localPosition = new Vector3(1.1f * i, 2.0f, 0.0f);
        cube.transform.localScale = new Vector3(1.0f, 0.1f * (i + 1), 1.0f);
        cube.AddComponent<Rigidbody>();
      }
    }

    /// @brief Creates a simple hallway by manipulating the scale of cubes.
    ///
    public static void CreateHallway()
    {
      GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
      leftWall.transform.localPosition = new Vector3(-2.0f, 5.0f, 0.0f);
      leftWall.transform.localScale = new Vector3(1.0f, 10.0f, 10.0f);

      GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
      rightWall.transform.localPosition = new Vector3(2.0f, 5.0f, 0.0f);
      rightWall.transform.localScale = new Vector3(1.0f, 10.0f, 10.0f);
    }

    /// @brief Creates a hut-looking house by scaling cubes.
    ///
    public static void CreateHouse()
    {
      float wallHeight = 8.0f;
      float insideY = 5.0f;
      float wallThickness = 1.0f;
      float doorWidth = 2.0f;
      float insideX = 10.0f;
      float insideZ = 10.0f;
      GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
      leftWall.transform.localScale = new Vector3(
        wallThickness, wallHeight,
        insideZ + 2.0f *
        wallThickness);
      leftWall.transform.localPosition = new Vector3(
        -(insideX / 2.0f + wallThickness / 2.0f), wallHeight / 2.0f, 0.0f);

      GameObject rightWall = (GameObject)Instantiate(leftWall);
      Vector3 newPosition = rightWall.transform.localPosition;
      newPosition.x = -newPosition.x;
      rightWall.transform.localPosition = newPosition;

      GameObject frontWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
      frontWall.transform.localScale = new Vector3(insideX - doorWidth,
                                                   wallHeight, wallThickness);
      frontWall.transform.localPosition = new Vector3(-doorWidth / 2.0f,
                                                      wallHeight / 2.0f,
                                                      -(insideZ / 2.0f +
                                                        wallThickness / 2.0f));

      GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
      backWall.transform.localScale = new Vector3(insideX,
                                                  wallHeight, wallThickness);
      backWall.transform.localPosition = new Vector3(
        0.0f,
        wallHeight / 2.0f,
        insideZ / 2.0f + wallThickness / 2.0f);

      GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
      ceiling.transform.localScale = new Vector3(insideX, wallThickness,
                                                 insideZ);
      ceiling.transform.localPosition = new Vector3(
        0.0f,
        insideY + wallThickness /
        2.0f, 0.0f);
    }

    /// @brief Creates a maze using a randomized Prim's algorithm.
    /// @param mazeHeight [in] The height of each wall in game units.
    /// @param mazeHorizontalDimension [in] The width and depth of walls.
    /// @param mazeXSize [in] The number of columns of walls in the maze.
    /// @param mazeZSize [in] The number of rows of walls in the maze.
    ///
    public static void CreateMaze(float mazeHeight,
                                  float mazeHorizontalDimension, uint mazeXSize,
                                  uint mazeZSize)
    {
      GameObject parent = new GameObject();
      parent.name = "MazeParent";

      GridGeneration.Grid grid = GridGeneration.Grid.RandomizedPrim(mazeZSize,
                                                                    mazeXSize);
      uint cellNum = 0;

      for (int row = 0; row < grid.numRows; row++)
      {
        for (int col = 0; col < grid.numCols; col++)
        {
          if (grid.IsWall(row, col))
          {
            cellNum++;
            GameObject tempCell = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tempCell.transform.parent = parent.transform;
            tempCell.name = "MazePart" + cellNum.ToString();
            tempCell.transform.localScale = new Vector3(mazeHorizontalDimension,
                                                        mazeHeight,
                                                        mazeHorizontalDimension);
            tempCell.transform.localPosition = new Vector3(
              col * mazeHorizontalDimension + mazeHorizontalDimension / 2.0f,
              mazeHeight / 2.0f,
              row * mazeHorizontalDimension + mazeHorizontalDimension / 2.0f);
          }
        }
      }
    }
  }
}
