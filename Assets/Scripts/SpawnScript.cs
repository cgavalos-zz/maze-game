using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// My `using`s
using GridGeneration;
using CreationGeneral;
using WallManagement;

public class SpawnScript : MonoBehaviour
{
  public WallManager manager;
  public float wallHeight = 6.0f;
  public float wallHorizontalDimension = 3.0f;
  public uint mazeRows = 39;
  public uint mazeCols = 39;
  protected bool hasDone;
  protected float start;

  // Use this for initialization
  void Start ()
  {
    GridGeneration.Grid grid = GridGeneration.Grid.RandomizedPrim(mazeRows,
                                                                  mazeCols);
    grid.Decimate(0.8, true);
    manager =
      new WallManager(grid,
                      wallHeight,
                      wallHorizontalDimension);
    InterpData data = new InterpData();
    data.startTime = Time.time;
    data.endTime = data.startTime + 10.0f;
    data.startHeightQuality = 0.0f;
    data.endHeightQuality = 1.0f;
    data.wallRow = 0;
    data.wallCol = 0;

    manager.moveData.Add(data);

    manager.CreateClearedWalledClearingNow(5, 20, 5, 20, 1.0f);
    hasDone = false;
    start = Time.time;
  }

  // Update is called once per frame
  void Update ()
  {
    manager.Update();

    if (Time.time - start > 2.0f && !hasDone)
    {
      hasDone = true;
      manager.FixWidePathways(0, mazeRows, 0, mazeCols, Time.time, 2.0f);
    }
  }
}
