using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class Node : MonoBehaviour
{

    [SerializeField] List<Node> _neighbors;

    public int _cost;

    private void Start()
    {
        _cost = 1;
    }
    public List<Node> GetNeighbors()
    {
        return _neighbors;
    }
}

