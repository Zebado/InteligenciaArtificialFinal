using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public float nodeSize;
    Node[,] grid;
    int gridSizeX, gridSizeY;

    bool _drawGizoms = true;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _drawGizoms = !_drawGizoms;
        }
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
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int nodeX = Mathf.RoundToInt((node.position.x + gridWorldSize.x / 2) / nodeSize);
        int nodeY = Mathf.RoundToInt((node.position.z + gridWorldSize.y / 2) / nodeSize);

        // Revisamos cada una de las direcciones alrededor del nodo actual (horizontal, vertical y diagonal)
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Evitar el nodo central
                if (x == 0 && y == 0) continue;

                int checkX = nodeX + x;
                int checkY = nodeY + y;

                // Asegurarnos de que los índices estén dentro de los límites de la cuadrícula
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbors.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
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
        if (!_drawGizoms) return;
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

