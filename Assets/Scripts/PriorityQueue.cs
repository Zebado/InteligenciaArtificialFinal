using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    Dictionary<Node, float> _allNodes = new Dictionary<Node, float>();

    public int Count
    {
        get { return _allNodes.Count; }
    }
    public void Put(Node node, float cost)
    {
        if (_allNodes.ContainsKey(node)) _allNodes[node] = cost;
        else _allNodes.Add(node, cost);
    }
    public Node Get()
    {
        Node node = null;
        float lowestCost = Mathf.Infinity;

        foreach (var item in _allNodes)
        {
            if (item.Value < lowestCost)
            {
                lowestCost = item.Value;
                node = item.Key;
            }
        }
        _allNodes.Remove(node);
        return node;
    }
}
