using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour {

  public float mouseSensitivity = 1.0f;
  public Vector2 mouseLook;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"),
      Input.GetAxisRaw("Mouse Y"));

    mouseLook += mouseDelta * mouseSensitivity;
    transform.localRotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
    transform.localRotation = transform.localRotation *
      Quaternion.AngleAxis(-mouseLook.y, transform.right);
	}
}
