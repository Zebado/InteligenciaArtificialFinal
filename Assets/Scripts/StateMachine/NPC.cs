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
    public Transform healingZone;

    [HideInInspector] public Boid boid;
    private StateMachine fsm;
    private void Awake()
    {
        boid = GetComponent<Boid>();
    }
    void Start()
    {
        fsm = new StateMachine();

        fsm.SetState(new FollowState(this, fsm));
        if (healingZone == null)
        {
            if (CompareTag("Blue"))
                healingZone = GameObject.Find("HealingTeamBlue").transform;
            else if (CompareTag("Red"))
                healingZone = GameObject.Find("HealingTeamRed").transform;
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
                if (!Physics.Raycast(transform.position + Vector3.up, dir, distance, GameManager.Instance.wallMask))
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
