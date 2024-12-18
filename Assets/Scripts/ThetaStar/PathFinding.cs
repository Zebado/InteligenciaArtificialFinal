using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Grid Grid;

    public List<Vector3> ThetaStar(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = Grid.NodeFromWorldPoint(startPos);
        Node targetNode = Grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in Grid.GetNeighbours(currentNode))
            {
                if (!neighbour.IsWalkable || closedSet.Contains(neighbour)) continue;

                float newCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);

                if (!openSet.Contains(neighbour) || newCostToNeighbour < neighbour.GCost)
                {
                    neighbour.GCost = newCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, targetNode);

                    if (HasLineOfSight(currentNode.Parent, neighbour))
                    {
                        neighbour.Parent = currentNode.Parent;
                    }
                    else
                    {
                        neighbour.Parent = currentNode;
                    }

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        return new List<Vector3>();
    }

    bool HasLineOfSight(Node from, Node to)
    {
        Vector3 direction = to.WorldPosition - from.WorldPosition;
        float distance = Vector3.Distance(from.WorldPosition, to.WorldPosition);
        return !Physics.Raycast(from.WorldPosition, direction.normalized, distance, Grid.UnwalkableMask);
    }

    List<Vector3> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.WorldPosition);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    float GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int dstY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
