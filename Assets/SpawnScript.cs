using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

  // Use this for initialization
  void Start () {
    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    cube.transform.localPosition = new Vector3(0.0f, 2.0f, 0.0f);
    cube.transform.localScale = new Vector3(2.0f, 0.5f, 4.0f);
  }

  // Update is called once per frame
  void Update () {
    //
  }
}
