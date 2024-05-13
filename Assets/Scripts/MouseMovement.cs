using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public Camera mCamera;
    Vector3 dragPos;
    public Vector2 mouseSize;
    public float mouseSensitivity = 100f;
    public float mouseScrollSensitivity = 100f;
    private void Start()
    {
        mCamera.orthographicSize = mouseSize.x;
    }
    private void Update()
    {
        Movement();
    }
    public void Movement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragPos = mCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 differ = dragPos - mCamera.ScreenToWorldPoint(Input.mousePosition);
            differ.z = 0;
            differ.x *= mouseSensitivity;
            differ.y *= mouseSensitivity;
            mCamera.transform.position += differ;
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if (mouseScroll > 0)
        {
            float mouse_Size = ClampSize(mCamera.orthographicSize - Time.deltaTime * mouseScrollSensitivity, mouseSize.x, mouseSize.y);
            mCamera.orthographicSize = mouse_Size;
        }
        else if (mouseScroll < 0)
        {
            float mouse_Size = ClampSize(mCamera.orthographicSize + Time.deltaTime * mouseScrollSensitivity, mouseSize.x, mouseSize.y);
            mCamera.orthographicSize = mouse_Size;
        }
    }
    public float ClampSize(float size, float min, float max)
    {
        return Mathf.Clamp(size, min, max);
    }
}
