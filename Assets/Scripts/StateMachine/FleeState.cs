using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    private NPC npc;
    private float fleeDuration = 5f;
    private float startTime;
    StateMachine fsm;

    public FleeState(NPC npc, StateMachine fsm)
    {
        this.npc = npc;
        this.fsm = fsm;
    }

    public void OnEnter()
    {
        Debug.Log("NPC entra en estado FLEE");
        startTime = Time.time;
    }

    public void OnUpdate()
    {
        if (npc.healingZone != null)
        {
            Vector3 dir = npc.healingZone.position - npc.transform.position;
            Vector3 moveDir = dir.normalized;

            npc.transform.position += moveDir * Time.deltaTime * 2.5f;

            if (moveDir != Vector3.zero)
            {
                Quaternion lookRot = Quaternion.LookRotation(moveDir);
                npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, lookRot, Time.deltaTime * 10f);
            }
        }

        if (Time.time - startTime > fleeDuration)
        {
            fsm.SetState(new FollowState(npc, fsm));
        }
    }

    public void OnExit()
    {
        Debug.Log("NPC sale de estado FLEE");
    }
}
