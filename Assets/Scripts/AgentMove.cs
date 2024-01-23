using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMove : MonoBehaviour
{
    [SerializeField, Range(0, 10)] float _speed;
    Vector3 _targetPosition;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _targetPosition = hit.point;

                Vector3 direction = _targetPosition - transform.position;
                transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
    }
}
