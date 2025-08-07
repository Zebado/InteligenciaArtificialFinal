using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float hp = 100f;
    public Transform leader;    
    public List<NPC> allEnemies;
    public NPC currentTarget;
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    public Transform healingZone;

    public List<Node> currentPath;
    public int currentIndex;
    public float moveSpeed = 3f;

    [HideInInspector] public Flocking boid;
    private LOS los;
    private StateMachine fsm;

    void Awake()
    {
        boid = GetComponent<Flocking>();
        los = GetComponent<LOS>();
    }

    void Start()
    {
        fsm = new StateMachine();
        fsm.SetState(new FollowState(this, fsm));

        if (healingZone == null)
        {
            healingZone = GameObject.Find(CompareTag("Blue") ? "HealingTeamBlue" : "HealingTeamRed").transform;
        }
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
                if (los.HasLineOfSight(enemy.transform.position))
                {
                    return enemy;
                }
            }
        }

        return null;
    }

    public void Shoot()
    {
        GameObject bullet = BulletPool.Instance.GetBullet();
        bullet.transform.position = transform.position + transform.forward * 1f;
        bullet.transform.forward = transform.forward;
        bullet.GetComponent<Bullet>().Shoot(transform.forward);
    }

    public bool IsEnemyInSight()
    {
        currentTarget = FindVisibleEnemy();
        return currentTarget != null;
    }

    public bool HasLineOfSightTo(Vector3 target)
    {
        return los.HasLineOfSight(target);
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            Die();
            return;
        }

        if (IsLowHealth() && fsm.CurrentState is not FleeState)
        {
            fsm.SetState(new FleeState(this, fsm));
        }
    }

    public bool IsLowHealth()
    {
        return hp <= 20;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}