using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public static BoidManager Instance { get; private set; }
    public List<Flocking> AllBoids { get; private set; }

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
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        AllBoids = new List<Flocking>();
    }

    public void RegisterNewBoid(Flocking newBoid)
    {
        if (!AllBoids.Contains(newBoid))
            AllBoids.Add(newBoid);
    }
}
