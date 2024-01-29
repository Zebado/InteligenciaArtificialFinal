using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMove : MonoBehaviour
{
    Vector3 _targetPosition = new Vector3();
    [SerializeField, Range(0, 10)] float _speed;
    SelectAgent _agentMove;

    private void Start()
    {
        _targetPosition = new Vector3();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _targetPosition = hit.point;

            }
        }
        if (_targetPosition == null) return;
        else
        {
            Vector3 direction = _targetPosition - transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
            if (transform.position == _targetPosition)
            {
                _targetPosition = Vector3.zero;
            }
        }

    }
    public void ActiveMoveAgent()
    {

    }
}
