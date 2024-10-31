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
    PathFinding pathFinding;
    LayerMask _obstacleLayer;

    private void Start()
    {
        _targetPosition = new Vector3();
        pathFinding = new PathFinding();
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
                Debug.Log("vamos a verificar la linea de vision");
                // Verificamos si hay línea de visión directa hacia el objetivo
                if (LOS.InLineOfSight(this.transform.position, _targetPosition, _obstacleLayer))
                {
                    Debug.Log("Moveremos al agente directamente hacia el objetivo.");
                    ActiveMoveAgent(_agentSelected, null);
                }
                //else
                //{
                //    Debug.Log("Usaremos ThetaStar para calcular el camino.");
                //    path = pathFinding.ThetaStar(_agentSelected.GetComponent<Node>(), goalNode, _obstacleLayer);
                //    ActiveMoveAgent(_agentSelected, path);
                //}
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
                Debug.Log("Agente seleccionado: " + _agentSelected.name);
            }
            else if (_agentSelected == hit.transform.gameObject)
            {
                Debug.Log("Este agente ya está seleccionado.");
            }
        }
    }

    // Método para activar el movimiento del agente
    public void ActiveMoveAgent(GameObject agent, Stack<Node> path)
    {
        if (agent == null) return;
        Debug.Log("Iniciando el movimiento del agente.");
        StartCoroutine(MoveAgent(agent, path));
    }

    IEnumerator MoveAgent(GameObject agent, Stack<Node> path)
    {
        _isMoving = true;
        float ejeY = agent.transform.position.y;

        // Si hay un camino calculado, sigue cada nodo del camino
        while (Vector3.Distance(agent.transform.position, _targetPosition) > 0.1f)
        {
            Vector3 nextPosition;
            if (path != null && path.Count > 0)
            {
                Node nextNode = path.Pop();
                nextPosition = nextNode.transform.position;
            }
            else
            {
                // Si no hay un camino, mueve directamente al objetivo
                nextPosition = _targetPosition;
            }

            // Si hay una obstrucción, recalculamos el camino desde la posición actual
            if (LOS.InLineOfSight(this.transform.position, nextPosition, _obstacleLayer))
            {
                Debug.Log("Obstrucción detectada, recalculando el camino.");
                Node current = agent.GetComponent<Node>();
                Node goalNode = FindClosestNode(_targetPosition);
                path = pathFinding.ThetaStar(current, goalNode, _obstacleLayer);
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
    private Node FindClosestNode(Vector3 targetPosition)
    {
        Node[] allNodes = FindObjectsOfType<Node>();
        Node closestNode = null;
        float minDistance = Mathf.Infinity;

        foreach (Node node in allNodes)
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