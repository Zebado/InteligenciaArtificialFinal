using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMove : MonoBehaviour
{
    Vector3 _targetPosition;
    [SerializeField, Range(0, 10)] float _speed;
    SelectAgent _agentMove;

    //objetivo : que detecte que personaje estoy clickeando y luego poder mover ese personaje y que el otro no se mueva
      // - deberiamos tener un objeto padre donde almacenar el dato del personaje.
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
                _agentMove._agentSelected.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            }
        }
        _agentMove._agentSelected.transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

    }
}
