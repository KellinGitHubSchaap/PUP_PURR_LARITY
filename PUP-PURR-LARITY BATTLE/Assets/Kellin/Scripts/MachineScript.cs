using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    public BodyMovementScript m_movementScript;
    public bool m_isFixed = false;
    public bool m_isGettingFixed = false;

    private void OnMouseDown()
    {
        if (!m_isFixed && m_movementScript.m_isFocused)
        {
            Debug.Log("Cat goes fixing");
            m_movementScript.MoveToBrokenMachine(this.gameObject.transform);
        }
    }

    private void Update()
    {
        if (m_movementScript.m_catState == BodyMovementScript.CatState.Fixing && !m_isGettingFixed)
        {
            m_isGettingFixed = true;
            StartCoroutine(FixMachine());
        }
    }

    public IEnumerator FixMachine()
    {
        yield return new WaitForSeconds(3);
        tag = "Fixed";
        GetComponent<MeshRenderer>().material = WorldScript.instance.m_machineFixedMaterial;
        m_isFixed = true;

        m_movementScript.m_catState = BodyMovementScript.CatState.Wandering;

    }
}
