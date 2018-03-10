using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

  public float mouseSensitivity = 1.0f;
  public Vector2 mouseLook;
  public GameObject character;

	// Use this for initialization
	void Start () {
  }

  // Update is called once per frame
  void Update () {
    if(Input.GetKey("escape")) {
      if (Cursor.lockState == CursorLockMode.Locked) {
        Cursor.lockState = CursorLockMode.None;
      } else if (Cursor.lockState == CursorLockMode.None) {
        Cursor.lockState = CursorLockMode.Locked;
      }
    }

    Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"),
      Input.GetAxisRaw("Mouse Y"));

    mouseLook += mouseDelta * mouseSensitivity;
    character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x,
    character.transform.up);
    transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
	}
}
