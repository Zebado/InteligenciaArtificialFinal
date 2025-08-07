    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class LOS : MonoBehaviour
{
    public LayerMask WallLayerMask;

    public bool HasLineOfSight(Vector3 target)
    {
        Vector3 from = transform.position + Vector3.up * 0.5f;
        Vector3 to = target + Vector3.up * 0.5f;
        Vector3 dir = to - from;
        float dist = dir.magnitude;

        return !Physics.Raycast(from, dir.normalized, dist, WallLayerMask);
    }
}
