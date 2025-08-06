using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { Red, Blue }
public class Flocking : MonoBehaviour
{
    Vector3 _velocity;
    [SerializeField] float _maxSpeed;
    [Range(0f, 0.1f), SerializeField] float _maxForce;

    [SerializeField] private Faction _faction;
    public Faction FactionType => _faction;

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
        if (_los.LineOfSight(_leader.position))
        {
        if (Vector3.Distance(transform.position, _leader.transform.position) > _followDistance)
            ApplyFlocking();
        else
            _velocity = Vector3.zero;
        }
        Vector3 move = _velocity * Time.deltaTime;
        move.y = 0;
        transform.position += move;
        if (_velocity != Vector3.zero)
        {
            transform.forward = _velocity;
        }
    }

    private void ApplyFlocking()
    {
        AddForce(Separation() * BoidManager.Instance.SeparationWeight +
                 Alignment() * BoidManager.Instance.AlignmentWeight +
                 Cohesion() * BoidManager.Instance.CohesionWeight);
    }

    Vector3 Separation()
    {

        Vector3 desired = Vector3.zero;

        foreach (Flocking boid in BoidManager.Instance.AllBoids)
        {
            if (boid == this) continue;

            Vector3 dirToBoid = boid.transform.position - transform.position;

            if (dirToBoid.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                desired -= dirToBoid / dirToBoid.sqrMagnitude;
            }
        }

        if (desired == Vector3.zero) return desired;

        return CalculateSteering(desired);
    }

    Vector3 Alignment()
    {
        Vector3 desired = Vector3.zero;

        int count = 0;

        foreach (Flocking boid in BoidManager.Instance.AllBoids)
        {
            if (boid == this) continue;

            Vector3 dirToBoid = boid.transform.position - transform.position;

            if (dirToBoid.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                desired += boid._velocity;

                count++;
            }
        }

        if (count == 0) return desired;

        desired /= count;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;

        int count = 0;

        if (_leader != null)
        {
            desired += _leader.position;
            count++;
        }
        foreach (Flocking boid in BoidManager.Instance.AllBoids)
        {
            if (boid == this) continue;

            Vector3 dirToBoid = boid.transform.position - transform.position;

            if (dirToBoid.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                desired += boid.transform.position;

                count++;
            }
        }

        if (count == 0) return desired;

        desired /= count;

        desired -= transform.position;

        return CalculateSteering(desired);
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude((desired.normalized * _maxSpeed) - _velocity, _maxForce);
    }

    void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    private void OnDrawGizmos()
    {
        if (!BoidManager.Instance) return;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(BoidManager.Instance.ViewRadius));
        if (_leader != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _leader.position);
        }
    }
}
