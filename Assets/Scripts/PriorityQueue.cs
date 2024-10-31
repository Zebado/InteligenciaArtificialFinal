using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    Dictionary<T, float> _allElements;

    public int Count { get { return _allElements.Count; } }
    public PriorityQueue()
    {
        _allElements = new Dictionary<T, float>();
    }

    public void Enqueue(T elem, float cost)
    {
        if (!_allElements.ContainsKey(elem)) _allElements.Add(elem, cost);

        else _allElements[elem] = cost;
    }

    public T Dequeue()
    {
        T elem = default;

        var currentValue = float.MaxValue;

        foreach (var item in _allElements)
        {

            if (currentValue > item.Value)
            {

                elem = item.Key;

                currentValue = item.Value;
            }
        }

        _allElements.Remove(elem);

        return elem;
    }
}
