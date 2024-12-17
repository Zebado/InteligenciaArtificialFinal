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
                {
                    Debug.Log("Iremos por LOS");
                    ActiveMoveAgent(_agentSelected, _targetPosition);
                }
                else
                {
                    Debug.Log("Iremos por pathfinding");
                    _currentPath = _pathFinding.FindPath(this.transform.position, _targetPosition);
                    if (_currentPath != null && _currentPath.Count > 0)
                    {
                        _currentPathIndex = 0;
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

                // Recalcular path si se pierde la línea de visión
                if (!_los.LineOfSight(targetPos))
                {
                    Debug.Log("Recalculando path...");
                    _currentPath = _pathFinding.FindPath(_agentSelected.transform.position, _targetPosition);
                    _currentPathIndex = 0;
                    break;
                }
                yield return null;
            }
            _currentPathIndex++;
        }
        _isMoving = false;
        Debug.Log("El agente ha llegado a su destino.");
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
                Debug.Log("El agente seleccionado es " + _agentSelected.name);
            }
            else if (_agentSelected == hit.transform.gameObject)
            {
                Debug.Log("Este agente ya está seleccionado.");
            }
        }
    }

    public void ActiveMoveAgent(GameObject agent, Vector3 target)
    {
        if (agent == null) return;
        Debug.Log("Agente moviéndose por LOS");
        StartCoroutine(MoveAgent(agent, target));
    }

    IEnumerator MoveAgent(GameObject agent, Vector3 target)
    {
        _isMoving = true;
        while (Vector3.Distance(agent.transform.position, target) > 0.1f)
        {
            Vector3 direction = (target - agent.transform.position).normalized;
            agent.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, target, _speed * Time.deltaTime);
            yield return null;
        }
        Debug.Log("El agente ha llegado a su destino por LOS.");
        _isMoving = false;
    }
}