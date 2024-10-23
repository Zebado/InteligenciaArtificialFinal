using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeSize;
    Node[,] grid;
    int gridSizeX, gridSizeY;

    public LayerMask wallMask;
    void Awake()
    {
        CreateGrid();
    }
    void Start()
    {
        CreateGrid();
    }
    private void OnValidate()
    {
        CreateGrid();
    }
    void CreateGrid()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeSize);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeSize);
        grid = new Node[gridSizeX, gridSizeY];

        Vector3 worldDownmLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldDownmLeft + Vector3.right * (x * nodeSize + nodeSize / 2) + Vector3.forward * (y * nodeSize + nodeSize / 2);

                bool walkable = !Physics.CheckSphere(worldPoint, nodeSize / 2, wallMask);

                grid[x, y] = new Node(worldPoint, walkable);
            }
        }
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node i in grid)
            {
                if (!i.walkable)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(i.position, Vector3.one * (nodeSize - 0.1f));
                }
                else
                {
                    Gizmos.color = Color.yellow;

                    Vector3 nodePosition = i.position;
                    Vector3 topLeft = nodePosition + new Vector3(-nodeSize / 2, 0, nodeSize / 2);
                    Vector3 topRight = nodePosition + new Vector3(nodeSize / 2, 0, nodeSize / 2);
                    Vector3 downLeft = nodePosition + new Vector3(-nodeSize / 2, 0, -nodeSize / 2);
                    Vector3 downRight = nodePosition + new Vector3(nodeSize / 2, 0, -nodeSize / 2);

                    Gizmos.DrawLine(topLeft, topRight);
                    Gizmos.DrawLine(topRight, downRight);
                    Gizmos.DrawLine(downRight, downLeft); 
                    Gizmos.DrawLine(downLeft, topLeft); 
                }
            }
        }
    }
}

