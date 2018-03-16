using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  public float walkForce = 100.0f;

  public Rigidbody rb;

  // Use this for initialization
  void Start () {}

  // Update is called once per frame
  void Update ()
  {
    Vector3 direction = Vector3.Normalize(
      transform.right * Input.GetAxis("Horizontal")
      + transform.forward * Input.GetAxis("Vertical"));

    rb.AddForce(direction * walkForce,
                ForceMode.Force);
  }
}
