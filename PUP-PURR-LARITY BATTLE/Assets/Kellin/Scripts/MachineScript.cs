using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    public CatMovementScript m_movementScript;
    public bool m_isFixed = false;
    public bool m_isGettingFixed = false;

    public GameObject m_exclamationPoint;

    private void OnMouseDown()
    {
        if (!m_isFixed && m_movementScript.m_isFocused)
        {
            Debug.Log("Cat goes fixing");
            m_movementScript.MoveToBrokenMachine(this);
        }
    }
    
    public void StartFixing()
    {
        StartCoroutine(FixMachine());
    }

    public void BreakDown()
    {
        m_isFixed = false;
        m_exclamationPoint.SetActive(true);
        tag = "Needs Fixing";
        GetComponent<MeshRenderer>().material = WorldScript.instance.m_machineBrokeMaterial;
    }

    private IEnumerator FixMachine()
    {
        yield return new WaitForSeconds(3);
        tag = "Fixed";
        GetComponent<MeshRenderer>().material = WorldScript.instance.m_machineFixedMaterial;

        m_isFixed = true;
        m_isGettingFixed = false;

        m_movementScript.m_catState = CatMovementScript.CatState.Wandering;

        m_exclamationPoint.SetActive(false);

        yield return new WaitForSeconds(2);
        m_isFixed = false;

    }
}
