using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerZoomAndPan : MonoBehaviour
{
    public Camera cam;
    public ViewChange view;
    bool canChange;

    // Update is called once per frame
    void Update()
    {
        canChange = view.interactable;

        if (cam.orthographicSize >= 0.1f && cam.orthographicSize <= 100)
            cam.orthographicSize -= Input.mouseScrollDelta.y * 0.5f;
        else if (cam.orthographicSize <= 0.1f)
            cam.orthographicSize = 0.1f;
        else if (cam.orthographicSize >= 100)
            cam.orthographicSize = 100;

        if (Input.GetMouseButton(2))
        {
            Quaternion camRot = cam.transform.rotation;
            Cursor.lockState = CursorLockMode.Locked;
            cam.transform.Translate( new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y")), Space.Self);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        float angle = 0.1f;

        if(Input.GetKey(KeyCode.LeftArrow) && canChange)
        {
            cam.transform.Rotate(Vector3.up, angle);
        }
        if (Input.GetKey(KeyCode.RightArrow) && canChange)
        {
            cam.transform.Rotate(Vector3.up, -angle);
        }
        if (Input.GetKey(KeyCode.UpArrow) && canChange)
        {
            cam.transform.Rotate(Vector3.right, angle);
        }
        if (Input.GetKey(KeyCode.DownArrow) && canChange)
        {
            cam.transform.Rotate(Vector3.right, -angle);
        }
        //-2.392152 - (-2.3836)
    }
}
