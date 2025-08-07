using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowState : State
{
    private NPC npc;
    StateMachine fsm;

    private bool hasLostLOS = false;
    private bool isUsingPath = false;
    public FollowState(NPC npc, StateMachine fsm)
    {
        this.npc = npc;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        Debug.Log("NPC entra en estado FOLLOW");

        npc.currentPath = null;
        npc.currentIndex = 0;

        if (npc.boid != null)
            npc.boid.enabled = false;

        hasLostLOS = false;
        isUsingPath = false;
    }

    public void OnUpdate()
    {
        if (npc.IsLowHealth() && fsm.CurrentState is not FleeState)
        {
            fsm.SetState(new FleeState(npc, fsm));
            return;
        }
        else if (npc.IsEnemyInSight())
        {
            fsm.SetState(new AttackState(npc, fsm));
            return;
        }

        if (!hasLostLOS)
        {
            if (npc.HasLineOfSightTo(npc.leader.position))
            {
                if (npc.boid != null && !npc.boid.enabled)
                    npc.boid.enabled = true;

                return;
            }
            else
            {
                hasLostLOS = true;
                isUsingPath = true;

                if (npc.boid != null)
                    npc.boid.enabled = false;

                Node start = GameManager.Instance.grid.GetClosestNodeTo(npc.transform.position);
                Node goal = GameManager.Instance.grid.GetClosestNodeTo(npc.leader.position);
                npc.currentPath = GameManager.Instance.pf.ThetaStar(start, goal);
                npc.currentPath.Reverse();
                npc.currentIndex = 0;
            }
        }

        if (isUsingPath)
        {
            FollowPath();
        }
    }
    void FollowPath()
    {
        if (npc.currentPath == null || npc.currentIndex >= npc.currentPath.Count) return;

        Vector3 target = npc.currentPath[npc.currentIndex].transform.position;
        Vector3 targetPos = new Vector3(target.x, npc.transform.position.y, target.z);
        Vector3 dir = (targetPos - npc.transform.position).normalized;

        npc.transform.position += dir * Time.deltaTime * npc.moveSpeed;

        if (dir != Vector3.zero)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, lookRot, Time.deltaTime * 10f);
        }

        if (Vector3.Distance(npc.transform.position, targetPos) < 0.2f)
        {
            npc.currentIndex++;
        }
    }

    public void OnExit()
    {
        Debug.Log("NPC sale de estado FOLLOW");
        if (npc.boid != null)
            npc.boid.enabled = false;

        npc.currentPath = null;
        npc.currentIndex = 0;
        isUsingPath = false;
    }
}
