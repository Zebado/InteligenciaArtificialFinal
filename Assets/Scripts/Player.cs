using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    List<Node> _path;
    public Grid grid;

    public void SetPath(List<Node> path)
    {
        _path = path;
        _path.Reverse();
    }

    public void SetPos(Vector3 pos)
    {
        pos.y = 1;
        transform.position = pos;
    }
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
    }
    public Node GetCurrentNode()
    {
        return grid.GetClosestNodeTo(transform.position);
    }
    private void Update()
    {
        if (_path != null && _path.Count > 0)
        {
            Vector3 dir = _path[0].transform.position - transform.position;
            dir.y = 0;

            if (dir.magnitude <= 0.1f)
            {
                _path.RemoveAt(0);
            }

            transform.position += dir.normalized * speed * Time.deltaTime;

            if (dir != Vector3.zero)
                transform.forward = dir.normalized;

            Vector3 fixedPos = transform.position;
            fixedPos.y = 1f;
            transform.position = fixedPos;
        }
    }
}
