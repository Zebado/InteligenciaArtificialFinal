using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMove : MonoBehaviour
{
    Vector3 _targetPosition = new Vector3();
    [SerializeField, Range(0, 10)] float _speed;

    [SerializeField] GameObject _agentBlue;
    [SerializeField] GameObject _agentRed;
    [SerializeField] GameObject _agentSelected;
    public bool _isMoving = false;

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

        if (Input.GetMouseButtonDown(1) && !_isMoving)
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
        StartCoroutine(MoveAgent(agent));
    }

    IEnumerator MoveAgent(GameObject agent)
    {
        _isMoving = true;
        float ejeY = agent.transform.position.y;
        while (Vector3.Distance(agent.transform.position, _targetPosition) > 0.1f)
        {
            Vector3 direction = (_targetPosition - agent.transform.position).normalized;

            agent.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            Vector3 newPosition = new Vector3(agent.transform.position.x, _targetPosition.y = 0, agent.transform.position.z);
            agent.transform.position = Vector3.MoveTowards(newPosition, _targetPosition, _speed * Time.deltaTime);
            yield return null;
        }
        print("ha llegado a destino");
        _isMoving = false;
    }
}
