using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float hp = 100;
    public Transform leader;
    public List<NPC> allEnemies;
    public NPC currentTarget;
    public float viewAngle = 90f;
    public float viewDistance = 10f;

    [HideInInspector] public Boid boid;
    private StateMachine fsm;
    private void Awake()
    {
        boid= GetComponent<Boid>(); 
    }
    void Start()
    {
        fsm = new StateMachine();

        fsm.SetState(new FollowState(this, fsm));
    }

    void Update()
    {
        fsm.Update();
    }
    public NPC FindVisibleEnemy()
    {
        foreach (var enemy in allEnemies)
        {
            if (enemy == null) continue;

            Vector3 dir = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dir);
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (angle < viewAngle / 2f && distance < viewDistance)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, dir, distance, GameManager.Instance.wallMask))
                {
                    return enemy;
                }
            }
        }

        return null;
    }
    public bool IsEnemyInSight()
    {
        currentTarget = FindVisibleEnemy();
        return currentTarget != null;
    }

    public bool IsLowHealth()
    {
        return hp <= 20;
    }
}
