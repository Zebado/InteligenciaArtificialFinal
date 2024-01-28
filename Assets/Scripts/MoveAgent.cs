using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAgent : MonoBehaviour
{
    [SerializeField]
    float _speed;

    void Update()
    {
        if (IsSelected())
        {

        }
    }

    void Move()
    {

    }
    bool IsSelected()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (transform.position == hit.point)
                {
                    return true;
                }
                
            }
        }
        return false;
    }
}
