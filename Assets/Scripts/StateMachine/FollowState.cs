using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{
    private NPC npc;
    StateMachine fsm;

    public FollowState(NPC npc, StateMachine fsm)
    {
        this.npc = npc;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        Debug.Log("NPC entra en estado FOLLOW");

        if (npc.boid != null)
        {
            npc.boid.enabled = true;
        }
        npc.currentPath = null;
        npc.currentIndex = 0;
    }

    public void OnUpdate()
    {
        if (npc.IsLowHealth() && fsm.CurrentState is not FleeState)
        {
            fsm.SetState(new FleeState(npc, fsm));
        }
        else if (npc.IsEnemyInSight())
        {
            fsm.SetState(new AttackState(npc, fsm));
        }
        Vector3 leaderPos = npc.leader.position;

        if (npc.HasLineOfSightTo(leaderPos))
        {
            if (npc.boid != null && !npc.boid.enabled)
                npc.boid.enabled = true;

            npc.currentPath = null;
            npc.currentIndex = 0;
        }
        else
        {
            if (npc.boid != null && npc.boid.enabled)
                npc.boid.enabled = false;

            if (npc.currentPath == null)
            {
                Node start = GameManager.Instance.grid.GetClosestNodeTo(npc.transform.position);
                Node goal = GameManager.Instance.grid.GetClosestNodeTo(leaderPos);

                npc.currentPath = GameManager.Instance.pf.ThetaStar(start, goal);
                npc.currentIndex = 0;
            }

            FollowPath();
        }
    }
    void FollowPath()
    {
        if (npc.currentPath == null || npc.currentIndex >= npc.currentPath.Count) return;

        Vector3 target = npc.currentPath[npc.currentIndex].transform.position;
        Vector3 dir = (target - npc.transform.position).normalized;

        npc.transform.position += dir * Time.deltaTime * npc.moveSpeed;

        if (Vector3.Distance(npc.transform.position, target) < 0.2f)
        {
            npc.currentIndex++;
        }
    }

    public void OnExit()
    {
        Debug.Log("NPC sale de estado FOLLOW");

        if (npc.boid != null)
        {
            npc.boid.enabled = false; 
        }
        npc.currentPath = null;
        npc.currentIndex = 0;
    }
}
