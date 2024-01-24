using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAgent : MonoBehaviour
{
    [SerializeField] GameObject _agentBlue;
    [SerializeField] GameObject _agentRed;
    public GameObject _agentSelected;
    //tenemos un error si seleccionamos otra cosa q no sea un agente, no se mueve, cambia la seleccion de agente
    private void Update()
    {
        //este if detecta si el mouse colisiona con algun agente y detecta el agente al que colisiona

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (_agentSelected != null && _agentSelected != hit.transform.gameObject)
                {
                    _agentSelected = hit.transform.gameObject;
                    if (_agentSelected == _agentBlue || _agentSelected == _agentRed)
                    {
                        print("Se ha seleccionado un agente" + _agentSelected);
                    }
                }
            }
        }
    }
}
