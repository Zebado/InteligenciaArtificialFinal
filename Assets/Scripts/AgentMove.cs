using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMove : MonoBehaviour
{
    Vector3 _targetPosition = new Vector3();
    [SerializeField, Range(0, 10)] float _speed;
    SelectAgent _agentMove;

    [SerializeField] GameObject _agentBlue;
    [SerializeField] GameObject _agentRed;
    public GameObject _agentSelected;
    private void Start()
    {
        _targetPosition = new Vector3();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if ((_agentSelected != hit.transform.gameObject) && (hit.transform.gameObject == _agentBlue || hit.transform.gameObject == _agentRed))
                {
                    _agentSelected = hit.transform.gameObject;
                    print("el agente seleccionado es " + _agentSelected);
                }
                else if (_agentSelected == hit.transform.gameObject)
                {
                    print("Este agente ya esta seleccionado");
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _targetPosition = hit.point;
                ActiveMoveAgent(_agentSelected);
            }
        }
    }
    public void ActiveMoveAgent(GameObject agent)
    {

        Vector3 direction = _targetPosition - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        agent.transform.position = Vector3.MoveTowards(this.transform.position, _targetPosition, _speed * Time.deltaTime);

        if (transform.position == _targetPosition)
        {
            _targetPosition = Vector3.zero;
        }
    }
}
