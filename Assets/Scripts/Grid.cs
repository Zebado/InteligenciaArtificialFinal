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

    void Start()
    {
        _grid = new GameObject[_width, _height]; //creo mi grilla del tamaño que asigne a las variables
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject newNode = Instantiate(_nodePrefab, transform);
                newNode.GetComponent<Node>().Initialize(x, y, new Vector3(x + x * _offset, 0, y + y * _offset), this);
                _grid[x, y] = newNode; //asigno cada nodo en cada espacio de mi grilla 
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height) return null;
        return _grid[x, y].GetComponent<Node>();
    }
}

