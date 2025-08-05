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
    public void UpdatePlayerPath(Player player)
    {
        Node start = player.GetCurrentNode();
        if (_goalNode == null || start == null) return;

        List<Node> path = pf.ThetaStar(start, _goalNode);
        player.SetPath(path);
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
    public void UpdatePlayerPath()
    {
        Node start = playerBlue.GetCurrentNode();
        if (_goalNode == null || start == null) return;

        List<Node> path = pf.ThetaStar(start, _goalNode);
        playerBlue.SetPath(path);
    }
}
