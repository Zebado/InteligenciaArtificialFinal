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
        if (npc.IsLowHealth())
        {
            fsm.SetState(new FleeState(npc, fsm));
        }
        else if (Vector3.Distance(npc.transform.position, npc.currentTarget.transform.position) > npc.viewDistance * 1.2f)
        {
            fsm.SetState(new FollowState(npc, fsm));
        }
        if (npc.currentTarget == null)
        {
            fsm.SetState(new FollowState(npc, fsm));
            return;
        }

        Vector3 flatDir = npc.currentTarget.transform.position - npc.transform.position;
        flatDir.y = 0f;

        if (flatDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(flatDir);
            npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, targetRot, Time.deltaTime * 10f);
        }

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            npc.Shoot();
        }      
    }
    public void OnExit()
    {
        Debug.Log("NPC sale de estado ATTACK");
    }
}
