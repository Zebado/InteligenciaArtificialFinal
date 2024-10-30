using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOS : MonoBehaviour
{
    public LayerMask WallLayerMask;
    Vector3 _target;
    public bool LineOfSight(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        float distance = Vector3.Distance(transform.position, target);
        _target = target;

        if (Physics.Raycast(transform.position, direction.normalized, distance, WallLayerMask))
        {
            return false;
        }
        return true;
    }
    private void OnDrawGizmos()
    {
        if (_target != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _target);
        }
    }
    public static bool InLineOfSight(Vector3 origin, Vector3 destination, LayerMask obstacleLayer)
    {
        Vector3 direction = destination - origin;
        return !Physics.Raycast(origin, direction, direction.magnitude, obstacleLayer);
    }
}
