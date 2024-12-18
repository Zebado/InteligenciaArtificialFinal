using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 WorldPosition;
    public int GridX, GridY;
    public bool IsWalkable;

    public float GCost;
    public float HCost;
    public Node Parent;

    public float FCost => GCost + HCost;
    public Node(Vector3 worldPosition, int gridX, int gridY, bool isWalkable)
    {
        WorldPosition = worldPosition;
        GridX = gridX;
        GridY = gridY;
        IsWalkable = isWalkable;
    }
}
