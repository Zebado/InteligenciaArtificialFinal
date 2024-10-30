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
    PathFinding pathFinding;
    LayerMask obstacleLayer;
    Stack<Node2> path;

    private void Start()
    {
        _targetPosition = new Vector3();
        pathFinding = new PathFinding();

        // Asignamos el LOS del agente seleccionado (si existe)
        if (_agentSelected != null)
            _los = _agentSelected.GetComponent<LOS>();
    }

    void Update()
    {
        // Selecciona el agente al hacer clic izquierdo
        if (Input.GetMouseButtonDown(0))
            AgentSelected();

        // Establece el destino al hacer clic derecho y empieza a moverse si no está en movimiento
        if (Input.GetMouseButtonDown(1) && !_isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _targetPosition = hit.point;

                // Convertimos el _targetPosition a un Node2 objetivo
                Node2 goalNode = FindClosestNode(_targetPosition);

                // Verificamos si hay línea de visión directa hacia el objetivo
                if (_los != null && _los.LineOfSight(_targetPosition))
                {
                    Debug.Log("Moveremos al agente directamente hacia el objetivo.");
                    ActiveMoveAgent(_agentSelected, null);
                }
                else
                {
                    Debug.Log("Usaremos ThetaStar para calcular el camino.");
                    // Calculamos el camino usando ThetaStar si no hay línea de visión
                    path = pathFinding.ThetaStar(_agentSelected.GetComponent<Node2>(), goalNode, obstacleLayer);
                    ActiveMoveAgent(_agentSelected, path);
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
            // Cambiamos el agente seleccionado al hacer clic en otro agente
            if ((_agentSelected != hit.transform.gameObject) && (hit.transform.gameObject == _agentBlue || hit.transform.gameObject == _agentRed))
            {
                _agentSelected = hit.transform.gameObject;
                _los = _agentSelected.GetComponent<LOS>();
                Debug.Log("Agente seleccionado: " + _agentSelected.name);
            }
            else if (_agentSelected == hit.transform.gameObject)
            {
                Debug.Log("Este agente ya está seleccionado.");
            }
        }
    }

    // Método para activar el movimiento del agente
    public void ActiveMoveAgent(GameObject agent, Stack<Node2> path)
    {
        if (agent == null) return;
        Debug.Log("Iniciando el movimiento del agente.");
        StartCoroutine(MoveAgent(agent, path));
    }

    IEnumerator MoveAgent(GameObject agent, Stack<Node2> path)
    {
        _isMoving = true;
        float ejeY = agent.transform.position.y;

        // Si hay un camino calculado, sigue cada nodo del camino
        while (Vector3.Distance(agent.transform.position, _targetPosition) > 0.1f)
        {
            Vector3 nextPosition;
            if (path != null && path.Count > 0)
            {
                Node2 nextNode = path.Pop();
                nextPosition = nextNode.transform.position;
            }
            else
            {
                // Si no hay un camino, mueve directamente al objetivo
                nextPosition = _targetPosition;
            }

            // Si hay una obstrucción, recalculamos el camino desde la posición actual
            if (!_los.LineOfSight(nextPosition))
            {
                Debug.Log("Obstrucción detectada, recalculando el camino.");
                Node2 current = agent.GetComponent<Node2>();
                Node2 goalNode = FindClosestNode(_targetPosition);
                path = pathFinding.ThetaStar(current, goalNode, obstacleLayer);
                continue;
            }

            Vector3 direction = (nextPosition - agent.transform.position).normalized;
            agent.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

            Vector3 newPosition = new Vector3(agent.transform.position.x, _targetPosition.y = 0, agent.transform.position.z);
            agent.transform.position = Vector3.MoveTowards(newPosition, nextPosition, _speed * Time.deltaTime);
            yield return null;
        }

        Debug.Log("El agente ha llegado al objetivo.");
        _isMoving = false;
    }

    // Método para encontrar el nodo más cercano al destino
    private Node2 FindClosestNode(Vector3 targetPosition)
    {
        Node2[] allNodes = FindObjectsOfType<Node2>();
        Node2 closestNode = null;
        float minDistance = Mathf.Infinity;

        foreach (Node2 node in allNodes)
        {
            float distance = Vector3.Distance(node.transform.position, targetPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }
}