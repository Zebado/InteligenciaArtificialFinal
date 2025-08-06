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
    }

    public void OnExit()
    {
        Debug.Log("NPC sale de estado FOLLOW");

        if (npc.boid != null)
        {
            npc.boid.enabled = false; 
        }
    }
}
