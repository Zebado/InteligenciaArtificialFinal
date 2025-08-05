using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Node _goalNode;
    public static GameManager Instance;
    public Player playerBlue;
    public Player playerRed;
    public PathFinding pf;
    public Grid grid;

    public LayerMask wallMask;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        StartCoroutine(InitAfterGridReady());
    }

    public void SetGoalNode(Node node, bool isLeftClick)
    {
        _goalNode = node;
        if (isLeftClick)
            UpdatePlayerPath(playerBlue);
        else
            UpdatePlayerPath(playerRed);
    }
    private IEnumerator InitAfterGridReady()
    {
        yield return new WaitForEndOfFrame();

        if (grid == null)
            grid = FindObjectOfType<Grid>();

        if (playerBlue == null)
            playerBlue = FindObjectOfType<Player>();

        _goalNode = null;
    }
    public void UpdatePlayerPath(Player player)
    {
        Node start = player.GetCurrentNode();
        if (_goalNode == null || start == null) return;

        bool hasLOS = pf.InSight(start.transform.position, _goalNode.transform.position);

#if UNITY_EDITOR
        Debug.DrawLine(start.transform.position, _goalNode.transform.position, hasLOS ? Color.green : Color.red, 2f);
#endif

        if (hasLOS)
        {
            player.SetPath(new List<Node> { _goalNode });
        }
        else
        {
            List<Node> path = pf.ThetaStar(start, _goalNode);
            player.SetPath(path);
        }
    }
}
