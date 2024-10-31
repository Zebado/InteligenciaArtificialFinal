using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<Node> nodes;
    public static NodeManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    public Node NodeClosetIsPosition(Vector3 position, LayerMask obstacleLayer)
    {
        Node nodeClosest = null;

        foreach (Node node in nodes)
        {
            if (LOS.InLineOfSight(position, node.transform.position, obstacleLayer))
            {
                if (nodeClosest == null || (node.transform.position - position).magnitude < (nodeClosest.transform.position - position).magnitude)
                {
                    nodeClosest = node;
                }
            }
        }

        return nodeClosest;
    }
}

