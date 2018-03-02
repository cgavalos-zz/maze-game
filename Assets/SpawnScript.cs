using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

  void VaryingDimensions() {
    for (int i = 0; i < 10; i++) {
      GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
      cube.transform.localPosition = new Vector3(1.1f * i, 2.0f, 0.0f);
      cube.transform.localScale = new Vector3(1.0f, 0.1f * (i + 1), 1.0f);
      cube.AddComponent<Rigidbody>();
    }
  }

  void CreateHallway() {
    GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
    leftWall.transform.localPosition = new Vector3(-2.0f, 5.0f, 0.0f);
    leftWall.transform.localScale = new Vector3(1.0f, 10.0f, 10.0f);

    GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
    rightWall.transform.localPosition = new Vector3(2.0f, 5.0f, 0.0f);
    rightWall.transform.localScale = new Vector3(1.0f, 10.0f, 10.0f);
  }

  void CreateHouse() {
    float wallHeight = 8.0f;
    float insideY = 5.0f;
    float wallThickness = 1.0f;
    float doorWidth = 2.0f;
    float insideX = 10.0f;
    float insideZ = 10.0f;
    GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
    leftWall.transform.localScale = new Vector3(wallThickness, wallHeight,
      insideZ + 2.0f * wallThickness);
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
      wallHeight / 2.0f, -(insideZ / 2 + wallThickness / 2));

    GameObject backWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
    backWall.transform.localScale = new Vector3(insideX,
      wallHeight, wallThickness);
    backWall.transform.localPosition = new Vector3(0.0f,
      wallHeight / 2.0f, insideZ / 2 + wallThickness / 2);

    GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
    ceiling.transform.localScale = new Vector3(insideX, wallThickness, insideZ);
    ceiling.transform.localPosition = new Vector3(0.0f,
      insideY + wallThickness / 2.0f, 0.0f);
  }

  // Use this for initialization
  void Start () {
    CreateHouse();
  }

  // Update is called once per frame
  void Update () {
    //
  }
}
