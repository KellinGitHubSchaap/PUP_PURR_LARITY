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
        Ray ray = m_cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hoverHit))
        {
            if (hoverHit.collider.CompareTag("Cat") && hoverHit.collider.gameObject.GetComponent<BodyMovementScript>().m_catState == BodyMovementScript.CatState.IsPlaying)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hoverHit.collider.gameObject.GetComponent<BodyMovementScript>().m_isFocused = true;
                    SoundManager.instance.PlayRefocusSoundEffect(0);
                }
            }

            if (hoverHit.collider.CompareTag("Dog"))
            {
                if (!Input.GetMouseButtonDown(0)) return;
                
                var dog = hoverHit.collider.GetComponent<Dog>();
                if (dog.state != DogState.Playing) return;
                
                dog.EnterState(DogState.Idle);
                SoundManager.instance.PlayRefocusSoundEffect(1);
            }
        }
    }
}
