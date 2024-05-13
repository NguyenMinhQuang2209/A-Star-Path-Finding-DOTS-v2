using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    List<Vector2> nextPos = new();
    public float moveSpeed = 1f;
    Vector2 targetPos;
    private void Update()
    {
        float distance = Vector2.Distance(transform.position, targetPos);
        if (distance >= 0.01f)
        {
            Vector3 dir = (Vector3)targetPos - transform.position;
            transform.position += dir.normalized * Time.deltaTime * moveSpeed;
            return;
        }
        if (nextPos.Count > 0)
        {
            targetPos = nextPos[0];
            nextPos.RemoveAt(0);
        }
        else
        {
            Invoke(nameof(GetNewPos), 1f);
        }
    }
    public void GetNewPos()
    {
        Vector2Int newPos = MapRendering.instance.NewPos();
        FindPath(newPos);
    }
    public void FindPath(Vector2Int target)
    {
        Vector2Int currentPos = new((int)Mathf.Round(transform.position.x), (int)Mathf.Round(transform.position.y));
        nextPos = MapRendering.instance.FindPath(currentPos, target);
    }
}
