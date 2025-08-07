using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Grid : MonoBehaviour
{
    GameObject[,] _grid;

    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] float _offset;
    [SerializeField] GameObject _nodePrefab;
    [SerializeField] LayerMask wallMask;

    public int Width => _width;
    public int Height => _height;

    void Start()
    {
        _grid = new GameObject[_width, _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 worldPos = new Vector3(x + x * _offset, 0, y + y * _offset);

                GameObject newNode = Instantiate(_nodePrefab, worldPos, Quaternion.identity, transform);

                bool isBlocked = Physics.CheckSphere(worldPos + Vector3.up * 0.5f, 0.4f, wallMask);

                Node node = newNode.GetComponent<Node>();
                node.Initialize(x, y, worldPos, this);
                node.isBlocked = isBlocked;
                _grid[x, y] = newNode;
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height) return null;
        return _grid[x, y].GetComponent<Node>();
    }

    public Node GetClosestNodeTo(Vector3 position)
    {
        Node closestNode = null;
        float minDistance = float.MaxValue;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Node node = _grid[x, y].GetComponent<Node>();
                if (node == null || node.isBlocked) continue;

                float dist = Vector3.Distance(position, node.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestNode = node;
                }
            }
        }
        return closestNode;
    }
}

