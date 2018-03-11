using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InputAssist;

public class PlayerLook : MonoBehaviour
{
  public float mouseSensitivity = 1.0f;
  public Vector2 mouseLook;
  public GameObject character;
  public Stopper stopper;

  // Use this for initialization
  void Start ()
  {
    stopper = new Stopper();
  }

  // Update is called once per frame
  void Update ()
  {
    if(stopper.Update(Input.GetKey("escape")))
    {
      if (Cursor.lockState == CursorLockMode.Locked)
      {
        Cursor.lockState = CursorLockMode.None;
      }
      else if (Cursor.lockState == CursorLockMode.None)
      {
        Cursor.lockState = CursorLockMode.Locked;
      }
    }

    Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"),
                                     Input.GetAxisRaw("Mouse Y"));

    mouseLook += mouseDelta * mouseSensitivity;
    character.transform.localRotation = Quaternion.AngleAxis(
      mouseLook.x,
      character.
      transform.up);
    transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
  }
}
