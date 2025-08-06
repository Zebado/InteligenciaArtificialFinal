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

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            npc.Shoot();
        }

        if (npc.IsLowHealth() && fsm.CurrentState is not FleeState)
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
