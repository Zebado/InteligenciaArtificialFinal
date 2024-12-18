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

    [SerializeField] PathFinding _pathFinding;
    [SerializeField] Grid _grid;
    List<Vector3> _currentPath = new List<Vector3>();
    int _currentPathIndex = 0;

    public bool _isMoving = false;
    LOS _los;

    private void Start()
    {
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
                    Debug.Log("Línea de visión directa. Moviendo directamente.");
                    _currentPath.Clear();
                    ActiveMoveAgent(_agentSelected);
                }
                else
                {
                    Debug.Log("No hay línea de visión. Calculando path...");
                    _currentPath = _pathFinding.ThetaStar(_agentSelected.transform.position, _targetPosition);
                    if (_currentPath.Count > 0)
                    {
                        _currentPathIndex = 0;
                        StartCoroutine(FollowPath(_agentSelected));
                    }
                    else
                    {
                        Debug.Log("No se encontró un camino al destino.");
                    }
                }
            }
        }
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
    IEnumerator FollowPath(GameObject agent)
    {
        _isMoving = true;
        while (_currentPathIndex < _currentPath.Count)
        {
            Vector3 nextPoint = _currentPath[_currentPathIndex];
            while (Vector3.Distance(agent.transform.position, nextPoint) > 0.1f)
            {
                if (!_los.LineOfSight(nextPoint))
                {
                    Debug.Log("Camino obstruido. Recalculando...");
                    _currentPath = _pathFinding.ThetaStar(agent.transform.position, _targetPosition);
                    if (_currentPath != null && _currentPath.Count > 0)
                    {
                        _currentPathIndex = 0;
                        StartCoroutine(FollowPath(_agentSelected));
                    }
                    else
                    {
                        Debug.Log("No se encontró un camino tras recalcular.");
                        _isMoving = false;
                        yield break;
                    }
                }

                Vector3 direction = (nextPoint - agent.transform.position).normalized;
                agent.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, nextPoint, _speed * Time.deltaTime);
                yield return null;
            }

            _currentPathIndex++;
        }

        Debug.Log("El agente ha llegado al destino.");
        _isMoving = false;
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
    void OnDrawGizmos()
    {
        if (_currentPath != null)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < _currentPath.Count - 1; i++)
            {
                Gizmos.DrawLine(_currentPath[i], _currentPath[i + 1]);
            }
        }
    }
}
