using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private NPC npc;
    private float attackCooldown = 1.5f;
    private float lastAttackTime;
    StateMachine fsm;

    public AttackState(NPC npc, StateMachine fsm)
    {
        this.npc = npc;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        Debug.Log("NPC entra en estado ATTACK");
        lastAttackTime = Time.time - attackCooldown;
    }

    public void OnUpdate()
    {
        if (npc.currentTarget == null)
        {
            fsm.SetState(new FollowState(npc, fsm));
            return;
        }

        Vector3 dir = npc.currentTarget.transform.position - npc.transform.position;
        float distance = dir.magnitude;

        if (distance > 1.5f)
        {
            npc.transform.position += dir.normalized * Time.deltaTime * 2f;
        }
        else
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Debug.Log("NPC ataca al enemigo");
                lastAttackTime = Time.time;

            }
        }

        if (npc.IsLowHealth())
        {
            fsm.SetState(new FleeState(npc, fsm));
        }
        else if (!npc.IsEnemyInSight())
        {
            fsm.SetState(new FollowState(npc, fsm));
        }
    }

    public void OnExit()
    {
        Debug.Log("NPC sale de estado ATTACK");
    }
}
