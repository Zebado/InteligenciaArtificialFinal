using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    static float Heuristic(Node currentNode, Node goalNode)
    {
        return Mathf.Abs(currentNode.transform.position.x - goalNode.transform.position.x) +
            Mathf.Abs(currentNode.transform.position.z - goalNode.transform.position.z);
    }
    public static List<Node> ThetaStar(Node startingNode, Node goalNode, LayerMask blockLayer)
    {
        if (startingNode == null || goalNode == null) return new List<Node>();

        var path = new List<Node>(AStar(startingNode, goalNode));

        int current = 0;

        while (current + 2 < path.Count)
        {
            if (LOS.InLineOfSight(path[current].transform.position, path[current + 2].transform.position, blockLayer))
            {
                path.RemoveAt(current + 1);
            }
            else
            {
                current++;
            }
        }

        return path;
    }

    public static Stack<Node> AStar(Node startingNode, Node goalNode)
    {
        Stack<Node> path = new Stack<Node>();

        if (startingNode == null || goalNode == null) return path;

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                while (current != null)
                {
                    path.Push(current);

                    current = cameFrom[current];
                }
                return path;
            }
            foreach (var next in current.GetNeighbors())
            {
                int newCost = costSoFar[current] + next._cost;

                float priority = newCost + Heuristic(next, goalNode);

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }
        return path;
    }

    public IEnumerator PaintAStar(Node startingNode, Node goalNode)
    {

        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        WaitForSeconds time = new WaitForSeconds(0.01f);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            yield return time;

            if (current == goalNode)
            {
                while (current != null)
                {

                    yield return time;
                    current = cameFrom[current];
                }
                break;
            }
            foreach (var next in current.GetNeighbors())
            {

                int newCost = costSoFar[current] + next._cost;

                float priority = newCost + Heuristic(next, goalNode);

                if (!costSoFar.ContainsKey(next))
                {
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                    costSoFar.Add(next, newCost);
                }
                else if (newCost < costSoFar[next])
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }
    }
}
