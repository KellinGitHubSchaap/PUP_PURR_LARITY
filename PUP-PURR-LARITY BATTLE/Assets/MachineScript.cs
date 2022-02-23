using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    public BodyMovementScript m_movementScript;
    private bool m_isFixed = false;

    private void OnMouseDown()
    {
        if (!m_isFixed)
        {
            Debug.Log("Cat goes fixing");
            m_movementScript.MoveToBrokenMachine(this.gameObject.transform);
            StartCoroutine(FixMachine());
        }
    }

    public IEnumerator FixMachine()
    {
        yield return new WaitForSeconds(3);
        tag = "Fixed";
        GetComponent<MeshRenderer>().material = WorldScript.instance.m_machineFixedMaterial;
        m_isFixed = true;

    }
}
