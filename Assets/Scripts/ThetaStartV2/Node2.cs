using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node2 : MonoBehaviour
{
    [SerializeField] List<Node2> _neighbors;
    public int _cost;
    private void Start()
    {
        _cost = 1;
    }
    public List<Node2> GetNeighbors()
    {
        return _neighbors;
    }
}
