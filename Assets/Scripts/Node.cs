using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node : MonoBehaviour
{
    int _x;
    int _y;
    Grid _grid;
    List<Node> _neighbors = new List<Node>();
    public float cost = 1;
    public bool isBlocked = false;
    public List<Node> GetNeighbors()
    {

        if (_neighbors.Count > 0) return _neighbors;

        Node neighbor;

        neighbor = _grid.GetNode(_x + 1, _y);
        if (neighbor != null) _neighbors.Add(neighbor);

        neighbor = _grid.GetNode(_x - 1, _y);
        if (neighbor != null) _neighbors.Add(neighbor);

        neighbor = _grid.GetNode(_x, _y + 1);
        if (neighbor != null) _neighbors.Add(neighbor);

        neighbor = _grid.GetNode(_x, _y - 1);
        if (neighbor != null) _neighbors.Add(neighbor);

        return _neighbors;
    }
    public void Initialize(int x, int y, Vector3 pos, Grid grid)
    {
        _x = x;
        _y = y;
        transform.position = pos;
        _grid = grid;

        gameObject.name = "Node" + _x + "," + _y;
        SetCost(cost);
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.SetGoalNode(this, true);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            GameManager.Instance.SetGoalNode(this, false);
        }
        if (Input.GetMouseButtonDown(2))
        {
            isBlocked = !isBlocked;
            gameObject.layer = isBlocked ? 7 : 0;
        }
    }
    public void SetCost(float newCost)
    {
        cost = Mathf.Clamp(newCost, 1, 99);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;

        foreach (Node node in GetNeighbors())
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
