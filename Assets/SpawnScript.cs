using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.transform.position = new Vector3(0, 2, 0);
  }

  // Update is called once per frame
  void Update () {
    //
  }
}
