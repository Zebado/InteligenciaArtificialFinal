using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Faction { Red, Blue }
public class Boid : MonoBehaviour
{
    Vector3 _velocity;
    [SerializeField] float _maxSpeed;
    [Range(0f, 0.1f), SerializeField] float _maxForce;

    [SerializeField] private Faction _faction;
    public Faction FactionType => _faction;
    void Start()
    {
        BoidManager.Instance.RegisterNewBoid(this);

        Vector3 random = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

        AddForce(random.normalized * _maxSpeed);
    }

    void Update()
    {
        AddForce(Separation() * BoidManager.Instance.SeparationWeight +
                 Alignment() * BoidManager.Instance.AlignmentWeight +
                 Cohesion() * BoidManager.Instance.CohesionWeight);

        transform.position += _velocity * Time.deltaTime;
        if (_velocity != Vector3.zero)
        {
            transform.forward = _velocity;
        }
    }

    Vector3 Separation()
    {

        //Variable donde vamos a recolectar todas las direcciones entre los flockmates
        Vector3 desired = Vector3.zero;

        //Por cada boid
        foreach (Boid boid in BoidManager.Instance.AllBoids)
        {
            //Si soy este boid a chequear, ignoro y sigo la iteracion
            if (boid == this) continue;

            //Saco la direccion hacia el boid
            Vector3 dirToBoid = boid.transform.position - transform.position;

            if (dirToBoid.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                desired -= dirToBoid / dirToBoid.sqrMagnitude; // Inverse distance factor
            }
        }

        if (desired == Vector3.zero) return desired;

        return CalculateSteering(desired);
    }

    Vector3 Alignment()
    {
        //Variable donde vamos a recolectar todas las direcciones entre los flockmates
        Vector3 desired = Vector3.zero;

        //Contador para acumular cantidad de boids a promediar
        int count = 0;

        //Por cada boid
        foreach (Boid boid in BoidManager.Instance.AllBoids)
        {
            //Si soy este boid a chequear, ignoro y sigo la iteracion
            if (boid == this) continue;

            //Saco la direccion hacia el boid
            Vector3 dirToBoid = boid.transform.position - transform.position;

            //Si esta dentro del rango de vision
            if (dirToBoid.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                //Sumo la direccion hacia donde esta yendo el boid
                desired += boid._velocity;

                //Sumo uno mas a mi contador para promediar
                count++;
            }
        }

        if (count == 0) return desired;

        //Promediamos todas las direcciones
        desired /= count;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion()
    {
        //Variable donde vamos a recolectar todas las direcciones entre los flockmates
        Vector3 desired = Vector3.zero;

        //Contador para acumular cantidad de boids a promediar
        int count = 0;

        foreach (Boid boid in BoidManager.Instance.AllBoids)
        {
            //Si soy este boid a chequear, ignoro y sigo la iteracion
            if (boid == this) continue;

            //Saco la direccion hacia el boid
            Vector3 dirToBoid = boid.transform.position - transform.position;

            //Si esta dentro del rango de vision
            if (dirToBoid.sqrMagnitude <= BoidManager.Instance.ViewRadius)
            {
                //Sumo la posicion de cada boid
                desired += boid.transform.position;

                //Sumo uno mas a mi contador para promediar
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
    }
}
