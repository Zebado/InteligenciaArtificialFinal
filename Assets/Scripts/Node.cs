using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public bool walkable;
    public Node parent;

    public float GCost;
    public float HCost;

    public float FCost => GCost + HCost;

    public Node(Vector3 position, bool walkable)
    {
        this.position = position;
        this.walkable = walkable;
        GCost = 0;
        HCost = 0;
        parent = null;
    }
}
