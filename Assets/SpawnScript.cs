using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

  // Use this for initialization
  void Start () {

    for (int i = 0; i < 10; i++) {
      GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
      cube.transform.localPosition = new Vector3(1.1f * i, 2.0f, 0.0f);
      cube.transform.localScale = new Vector3(1.0f, 0.1f * (i + 1), 1.0f);
      cube.AddComponent<Rigidbody>();
    }
  }

  // Update is called once per frame
  void Update () {
    //
  }
}
