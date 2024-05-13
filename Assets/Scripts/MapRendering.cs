using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MapRendering : MonoBehaviour
{
    public Vector2Int mapSize;
    public Box mapObject;
    public Transform map;
    public static MapRendering instance;
    public List<int> notWalkables = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        GenerateMap();
    }
    public void GenerateMap()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                int index = i * mapSize.y + j;
                Box tempBox = Instantiate(mapObject, new(i, j), Quaternion.identity, map);
                tempBox.BoxInit(true, index, new(i, j));
            }
        }
    }
    public Vector2Int NewPos()
    {
        Vector2Int newPos = new Vector2Int(UnityEngine.Random.Range(0, mapSize.x), UnityEngine.Random.Range(0, mapSize.y));
        return newPos;
    }
    public void Switch(int index, bool isWalkable)
    {
        if (isWalkable)
        {
            if (notWalkables.Contains(index))
            {
                notWalkables.Remove(index);
            }
        }
        else
        {
            if (!notWalkables.Contains(index))
            {
                notWalkables.Add(index);
            }
        }
    }

    public List<Vector2> FindPath(Vector2Int start, Vector2Int end)
    {
        if (end.x > mapSize.x || end.y > mapSize.y)
        {
            return new();
        }
        List<Vector2> paths = APathFinding.instance.GetPaths(new(start.x, start.y), new(end.x, end.y), notWalkables, mapSize.x, mapSize.y);
        return paths;
    }
}
