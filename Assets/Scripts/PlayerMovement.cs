using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

  public float walkSpeed = 1.0f;

  // Use this for initialization
  void Start () {

  }

  // Update is called once per frame
  void Update () {
    float forwardSpeed = Input.GetAxis("Vertical") * walkSpeed;
    float rightSpeed = Input.GetAxis("Horizontal") * walkSpeed;

    transform.Translate(transform.right * rightSpeed * Time.deltaTime);
    transform.Translate(transform.forward * forwardSpeed * Time.deltaTime);

    if (Input.GetKeyDown("escape")) {
      
    }
  }
}
