using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { Red, Blue }

public class Flocking : MonoBehaviour
{
    Vector3 _velocity;
    [SerializeField] float _maxSpeed = 5f;
    [Range(0f, 0.1f), SerializeField] float _maxForce = 0.05f;

    [SerializeField] private Faction _faction;
    public Faction FactionType => _faction;
    public float FollowDistance => _followDistance;

    Transform _leader;
    [SerializeField] float _followDistance = 5f;

    LOS _los;

    void Start()
    {
        BoidManager.Instance.RegisterNewBoid(this);
        _leader = _faction == Faction.Blue ? BoidManager.Instance.LeaderBlue : BoidManager.Instance.LeaderRed;
        _los = GetComponent<LOS>();
    }

    void Update()
    {
        if (_leader == null) return;

        bool enemyInSight = false;

        NPC npc = GetComponent<NPC>();
        if (npc != null)
            enemyInSight = npc.IsEnemyInSight();

        float distToLeader = Vector3.Distance(transform.position, _leader.position);

        if (enemyInSight)
        {
            Vector3 dir = (npc.currentTarget.transform.position - transform.position).normalized;
            dir.y = 0;
            if (dir != Vector3.zero)
                transform.forward = Vector3.Lerp(transform.forward, dir, Time.deltaTime * 10f);

            _velocity = Vector3.zero;
        }

        else if (_los.HasLineOfSight(_leader.position))
        {
            if (distToLeader > _followDistance)
                ApplyFlocking();
            else
                _velocity = Vector3.zero;
        }

        Vector3 move = _velocity * Time.deltaTime;
        move.y = 0;
        transform.position += move;

        if (_velocity != Vector3.zero)
            transform.forward = Vector3.Lerp(transform.forward, _velocity.normalized, Time.deltaTime * 10f);
    }

    private void ApplyFlocking()
    {
        Vector3 force =
            Separation() * BoidManager.Instance.SeparationWeight +
            Alignment() * BoidManager.Instance.AlignmentWeight +
            Cohesion() * BoidManager.Instance.CohesionWeight;

        AddForce(force);
    }

    Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;

        foreach (var boid in BoidManager.Instance.AllBoids)
        {
            if (boid == this) continue;

            Vector3 diff = boid.transform.position - transform.position;
            if (diff.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                desired -= diff / diff.sqrMagnitude;
            }
        }

        return desired == Vector3.zero ? Vector3.zero : CalculateSteering(desired);
    }

    Vector3 Alignment()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (var boid in BoidManager.Instance.AllBoids)
        {
            if (boid == this) continue;

            Vector3 diff = boid.transform.position - transform.position;
            if (diff.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                sum += boid._velocity;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        return CalculateSteering(sum / count);
    }

    Vector3 Cohesion()
    {
        Vector3 center = Vector3.zero;
        int count = 0;

        if (_leader != null)
        {
            center += _leader.position;
            count++;
        }

        foreach (var boid in BoidManager.Instance.AllBoids)
        {
            if (boid == this) continue;

            Vector3 diff = boid.transform.position - transform.position;
            if (diff.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                center += boid.transform.position;
                count++;
            }
        }

        if (count == 0) return Vector3.zero;

        Vector3 desired = (center / count) - transform.position;
        return CalculateSteering(desired);
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired.normalized * _maxSpeed - _velocity, _maxForce);
    }

    void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }
}