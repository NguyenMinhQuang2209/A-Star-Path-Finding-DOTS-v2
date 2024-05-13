using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public Vector2Int pos;
    public int index;
    bool isWalkable;
    public void BoxInit(bool isWalkable, int index, Vector2Int pos)
    {
        this.isWalkable = isWalkable;
        this.pos = pos;
        this.index = index;
        RenderColor();
    }
    public void Switch()
    {
        isWalkable = !isWalkable;
        RenderColor();
        MapRendering.instance.Switch(index, isWalkable);
    }
    private void RenderColor()
    {
        GetComponent<SpriteRenderer>().color = isWalkable ? Color.white : Color.black;
    }
}
