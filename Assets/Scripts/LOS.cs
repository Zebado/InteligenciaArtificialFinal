using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOS : MonoBehaviour
{
    public static bool InLineOfSight(Vector3 origin, Vector3 destination, LayerMask obstacleLayer)
    {
        Debug.Log("Estamos en el line of sight");
        Vector3 direction = destination - origin;
        return !Physics.Raycast(origin, direction, direction.magnitude, obstacleLayer);
    }
}
