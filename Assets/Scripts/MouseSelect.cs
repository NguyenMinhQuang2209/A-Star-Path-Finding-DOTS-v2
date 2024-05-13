using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelect : MonoBehaviour
{
    public Camera mCamera;
    public LineRenderer lineRender;
    Vector2 startMousePos;
    public BoxCollider2D boxCollider;
    public bool selecting = false;
    List<Box> selectBoxes = new();
    private void Start()
    {
        lineRender.positionCount = 4;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            boxCollider.enabled = true;
            lineRender.enabled = true;
            startMousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            SetLinearPosition(0, new(startMousePos.x, startMousePos.y, 0f));
            SetLinearPosition(1, new(startMousePos.x, startMousePos.y, 0f));
            SetLinearPosition(2, new(startMousePos.x, startMousePos.y, 0f));
            SetLinearPosition(3, new(startMousePos.x, startMousePos.y, 0f));
        }
        if (Input.GetMouseButton(1))
        {
            Vector2 newMousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            SetLinearPosition(1, new(startMousePos.x, newMousePos.y, 0f));
            SetLinearPosition(2, new(newMousePos.x, newMousePos.y, 0f));
            SetLinearPosition(3, new(newMousePos.x, startMousePos.y, 0f));
            transform.position = (startMousePos + newMousePos) / 2;
            boxCollider.size = new Vector2(
                Mathf.Abs(startMousePos.x - newMousePos.x),
                Mathf.Abs(startMousePos.y - newMousePos.y));
        }

        if (Input.GetMouseButtonUp(1))
        {
            for (int i = 0; i < selectBoxes.Count; i++)
            {
                Box box = selectBoxes[i];
                box.Switch();
            }
            boxCollider.enabled = false;
            lineRender.enabled = false;
            selectBoxes?.Clear();
        }
    }
    public void SetLinearPosition(int index, Vector3 pos)
    {
        lineRender.SetPosition(index, pos);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Box>(out var box))
        {
            if (!selectBoxes.Contains(box))
            {
                selectBoxes.Add(box);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Box>(out var box))
        {
            if (selectBoxes.Contains(box))
            {
                selectBoxes.Remove(box);
            }
        }
    }
}
