using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    public List<Node> AStar(Node start, Node goal)
    {
        if (start == null || goal == null) return null;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Put(start, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(start, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(start, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Get();

            if (current == goal)
            {
                List<Node> path = new List<Node>();
                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }
                return path;
            }
            foreach (var next in current.GetNeighbors())
            {
                if (next.isBlocked) continue;

                float distance = HeuristicDistance(next.transform.position, goal.transform.position);
                float newCost = costSoFar[current] + next.cost;
                float priority = distance + newCost;

                if (!cameFrom.ContainsKey(next))
                {
                    frontier.Put(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else
                {
                    if (newCost < costSoFar[next])
                    {
                        frontier.Put(next, priority);
                        cameFrom[next] = current;
                        costSoFar[next] = newCost;
                    }
                }
            }
        }
        return null;
    }
    public List<Node> ThetaStar(Node start, Node goal)
    {
        if (start == null || goal == null) return null;

        List<Node> path = AStar(start, goal);

        if (path == null) return null;

        int current = 0;
        int nextNext = current + 2;
        while (nextNext < path.Count)
        {
            if (InSight(path[current].transform.position, path[nextNext].transform.position))
            {
                path.RemoveAt(current + 1);
            }
            else
            {
                current++;
                nextNext++;
            }
        }
        return path;
    }
    public bool InSight(Vector3 a, Vector3 b)
    {
        return !Physics.Raycast(a, b - a, Vector3.Distance(a, b), GameManager.Instance.wallMask);
    }
    public float HeuristicDistance(Vector3 a, Vector3 b)
    {
        Vector3 distance = b - a;
        return distance.magnitude;
    }

}
