using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMove : MonoBehaviour
{
    Vector3 _targetPosition;
    [SerializeField, Range(0, 10)] float _speed;
    [SerializeField] GameObject _agentBlue;
    [SerializeField] GameObject _agentRed;
    [SerializeField] GameObject _agentSelected;

    public bool _isMoving = false;
    LOS _los;
    PathFinding _pathFinding;
    List<Node> _currentPath;
    int _currentPathIndex;

    private void Start()
    {
        _pathFinding = GetComponent<PathFinding>();
        _targetPosition = new Vector3();
        if (_agentSelected != null)
            _los = _agentSelected.GetComponent<LOS>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            AgentSelected();
        if (Input.GetMouseButtonDown(1) && !_isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _targetPosition = hit.point;
                if (_los != null && _los.LineOfSight(_targetPosition))
                    ActiveMoveAgent(_agentSelected);
                else
                {
                    _currentPath = _pathFinding.FindPath(_agentSelected.transform.position, _targetPosition);
                    _currentPathIndex = 0;
                    if (_currentPath != null && _currentPath.Count > 0)
                    {
                        StartCoroutine(FollowPath());
                    }
                    else
                    {
                        Debug.Log("No se encontró un camino disponible al destino.");
                    }
                }
            }
        }
    }
    private IEnumerator FollowPath()
    {
        _isMoving = true;
        while (_currentPathIndex < _currentPath.Count)
        {
            Vector3 targetPos = _currentPath[_currentPathIndex].position;
            while (Vector3.Distance(_agentSelected.transform.position, targetPos) > 0.1f)
            {
                Vector3 direction = (targetPos - _agentSelected.transform.position).normalized;
                _agentSelected.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                _agentSelected.transform.position = Vector3.MoveTowards(_agentSelected.transform.position, targetPos, _speed * Time.deltaTime);

                // Si se bloquea el camino, recalcular
                if (!_los.LineOfSight(targetPos))
                {
                    _currentPath = _pathFinding.FindPath(_agentSelected.transform.position, _targetPosition);
                    _currentPathIndex = 0;
                    break;
                }
                yield return null;
            }
            _currentPathIndex++;
        }
        _isMoving = false;
        print("El agente no puede moverse.");
    }
    private void AgentSelected()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if ((_agentSelected != hit.transform.gameObject) && (hit.transform.gameObject == _agentBlue || hit.transform.gameObject == _agentRed))
            {
                _agentSelected = hit.transform.gameObject;
                _los = _agentSelected.GetComponent<LOS>();
                print("el agente seleccionado es " + _agentSelected.name);
            }
            else if (_agentSelected == hit.transform.gameObject)
            {
                print("Este agente ya esta seleccionado");
            }
        }
    }
    public void ActiveMoveAgent(GameObject agent)
    {
        if (agent == null) return;
        Debug.Log("agente movete");
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
        print("el agente ah llegado a destino");
        _isMoving = false;
    }
}
