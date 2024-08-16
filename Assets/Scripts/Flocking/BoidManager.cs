using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance { get; private set; }
    public List<Boid> AllBoids { get; private set; }
    public float ViewRadius
    {
        get
        {
            return _viewRadius * _viewRadius;
        }
    }

    [SerializeField] float _viewRadius;
    [field: SerializeField, Range(0f, 2.5f)] public float SeparationWeight { get; private set; }
    [field: SerializeField, Range(0f, 2.5f)] public float AlignmentWeight { get; private set; }
    [field: SerializeField, Range(0f, 2.5f)] public float CohesionWeight { get; private set; }

    public Transform LeaderBlue;
    public Transform LeaderRed;

    void Awake()
    {
        Instance = this;
        AllBoids = new List<Boid>();
    }

    public void RegisterNewBoid(Boid newBoid)
    {
        if (!AllBoids.Contains(newBoid))
            AllBoids.Add(newBoid);
    }
}
