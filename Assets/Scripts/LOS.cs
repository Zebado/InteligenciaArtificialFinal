using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LOS
{
    public static bool InLineOfSight(Vector3 origin, Vector3 destination, LayerMask obstacleLayer)
    {
        Vector3 direction = destination - origin;
        return !Physics.Raycast(origin, direction, direction.magnitude, obstacleLayer);
    }
}
