using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public Grid grid;

    public List<Node> FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPosition);
        Node targetNode = grid.GetNodeFromWorldPosition(targetPosition);

        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                    currentNode = openSet[i];
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
                return RetracePath(startNode, targetNode);

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                Node parent = currentNode.parent != null && HasLineOfSight(currentNode.parent, neighbor) ? currentNode.parent : currentNode;
                if (parent == null)
                {
                    parent = currentNode;
                }
                float newGCost = parent.GCost + GetDistance(parent, neighbor);

                if (newGCost < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newGCost;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = parent;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return null;
    }
    private bool HasLineOfSight(Node from, Node to)
    {
        Vector3 direction = to.position - from.position;
        float distance = Vector3.Distance(from.position, to.position);
        return !Physics.Raycast(from.position, direction.normalized, distance, grid.wallMask);
    }
    private List<Node> RetracePath(Node startNode, Node lastNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = lastNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
    private float GetDistance(Node a, Node b)
    {
        int dstX = Mathf.Abs(Mathf.RoundToInt(a.position.x) - Mathf.RoundToInt(b.position.x));
        int dstY = Mathf.Abs(Mathf.RoundToInt(a.position.z) - Mathf.RoundToInt(b.position.z));

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
