using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClickScript : MonoBehaviour
{
    private Camera m_cam;       // The camera

    public BodyMovementScript m_bodyMovementScript;

    private void Start()
    {
        m_cam = GetComponent<Camera>();

        if (!m_bodyMovementScript)
        {
            Debug.Log("No m_bodyMovementScript found on: " + this.name);
        }

    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.CompareTag("Needs Fixing"))
                {
                    Debug.Log("Cat goes fixing");

                    m_bodyMovementScript.MoveToBrokenMachine(hit.collider.gameObject.transform);

                }
            }
        }
    }
}


// TODO: Send a signal to the BodyMovementScript that a broken machine is found
