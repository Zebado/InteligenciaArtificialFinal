using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Node _startingNode;
    public Node _goalNode;
    public static GameManager Instance;
    public Player player;
    public PathFinding pf;

    public LayerMask wallMask;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.SetPath(pf.ThetaStar(_startingNode, _goalNode));
        }
    }
    public void SetStartingNode(Node node)
    {
        if (_startingNode != null) PaintGameObject(_startingNode.gameObject, Color.white);
        _startingNode = node;
        PaintGameObject(_startingNode.gameObject, Color.green);
        Vector3 nodePos = _startingNode.transform.position;
        player.SetPos(new Vector3(nodePos.x, nodePos.y, nodePos.z));
    }
    public void SetGoalNode(Node node)
    {
        if (_goalNode != null) PaintGameObject(_goalNode.gameObject, Color.white);
        _goalNode = node;
        PaintGameObject(_goalNode.gameObject, Color.red);
    }

    public void PaintGameObject(GameObject obj, Color color)
    {
        obj.GetComponent<Renderer>().material.color = color;
    }
}
