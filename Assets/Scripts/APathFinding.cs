using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class APathFinding : MonoBehaviour
{
    public static APathFinding instance;
    public static int DIALOG = 14;
    public static int STRAIGHT = 10;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public List<Vector2> GetPaths(int2 start, int2 end, List<int> blocked, int x, int y)
    {
        List<Vector2> paths = new();
        NativeList<int2> list = new(Allocator.Temp);
        PathFinding(start, end, blocked, x, y, list);
        for (int i = 0; i < list.Length; i++)
            paths.Add(new(list[i].x, list[i].y));
        list.Dispose();
        paths.Reverse();
        return paths;
    }
    public void PathFinding(int2 start, int2 end, List<int> blocked, int x, int y, NativeList<int2> path)
    {
        NativeArray<NodePath> nodes = new NativeArray<NodePath>(x * y, Allocator.Temp);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                int index = i * y + j;
                NodePath newNode = new()
                {
                    x = i,
                    y = j,
                    index = index,
                    gCost = int.MaxValue,
                    cameFromNode = -1,
                    isWalkable = !blocked.Contains(index)
                };
                nodes[index] = newNode;
            }
        }
        int startIndex = CalculateIndex(start, y);
        int endIndex = CalculateIndex(end, y);

        NodePath startNode = nodes[startIndex];
        startNode.hCost = GetDistance(startNode.GetPos(), end);
        startNode.CalculateFCost();

        nodes[startIndex] = startNode;

        NativeList<int> processed = new(Allocator.Temp);
        NativeList<int> closed = new(Allocator.Temp);

        processed.Add(startIndex);
        while (processed.Length > 0)
        {
            NodePath currentNode = GetLowestNode(processed, nodes);

            if (currentNode.index == endIndex)
            {
                break;
            }

            for (int i = 0; i < processed.Length; i++)
            {
                if (processed[i] == currentNode.index)
                {
                    processed.RemoveAtSwapBack(i);
                    break;
                }
            }
            closed.Add(currentNode.index);

            NativeList<int2> leighbours = new(Allocator.Temp)
            {
                new(+1,+1),
                new(-1,-1),
                new(+1,-1),
                new(-1,+1),
                new(+1,0),
                new(-1,0),
                new(0,-1),
                new(0,+1),
            };

            for (int i = 0; i < leighbours.Length; i++)
            {
                int2 neighbourPos = currentNode.GetPos() + leighbours[i];
                if (IsValuePosition(neighbourPos, x, y))
                {
                    int neighbourIndex = CalculateIndex(neighbourPos, y);

                    if (closed.Contains(neighbourIndex))
                    {
                        continue;
                    }
                    NodePath neighbour = nodes[neighbourIndex];

                    if (!neighbour.isWalkable)
                    {
                        closed.Add(neighbourIndex);
                        continue;
                    }

                    if (processed.Contains(neighbourIndex))
                    {
                        continue;
                    }

                    int tentactiveGCost = currentNode.gCost + GetDistance(currentNode.GetPos(), neighbourPos);
                    if (tentactiveGCost < neighbour.gCost)
                    {
                        neighbour.gCost = tentactiveGCost;
                        neighbour.hCost = GetDistance(neighbour.GetPos(), end);
                        neighbour.cameFromNode = currentNode.index;
                        neighbour.CalculateFCost();
                        nodes[neighbourIndex] = neighbour;
                        processed.Add(neighbourIndex);
                    }
                }
            }

            leighbours.Dispose();
        }

        NodePath endNode = nodes[endIndex];
        if (endNode.cameFromNode == -1)
        {
            Debug.Log("No path finding");
        }
        else
        {
            NativeList<int2> endPaths = new(Allocator.Temp);

            NodePath currentNode = endNode;
            endPaths.Add(currentNode.GetPos());
            while (currentNode.cameFromNode != -1)
            {
                NodePath rootNode = nodes[currentNode.cameFromNode];
                endPaths.Add(rootNode.GetPos());
                currentNode = rootNode;
            }

            for (int i = 0; i < endPaths.Length; i++)
            {
                path.Add(endPaths[i]);
            }

            endPaths.Dispose();
        }

        processed.Dispose();
        closed.Dispose();
        nodes.Dispose();

    }
    public int GetDistance(int2 start, int2 end)
    {
        int x = Mathf.Abs(start.x - end.x);
        int y = Mathf.Abs(start.y - end.y);
        int remain = Mathf.Abs(x - y);
        return DIALOG * Mathf.Min(x, y) + STRAIGHT * remain;
    }
    public bool IsValuePosition(int2 pos, int x, int y)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < x && pos.y < y;
    }
    public int CalculateIndex(int2 pos, int height)
    {
        return pos.x * height + pos.y;
    }
    private NodePath GetLowestNode(NativeList<int> processed, NativeArray<NodePath> list)
    {
        NodePath lowest = list[processed[0]];
        for (int i = 1; i < processed.Length; i++)
        {
            NodePath current = list[processed[i]];
            if (current.fCost < lowest.fCost)
            {
                lowest = current;
            }
        }
        return lowest;
    }
    public struct NodePath
    {
        public int x;
        public int y;
        public int index;
        public int gCost;
        public int hCost;
        public int fCost;
        public int cameFromNode;
        public bool isWalkable;
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        public int2 GetPos()
        {
            return new(x, y);
        }
    }
}
