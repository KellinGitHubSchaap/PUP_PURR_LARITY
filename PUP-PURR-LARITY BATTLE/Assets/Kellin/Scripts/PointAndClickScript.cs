using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClickScript : MonoBehaviour
{
    private Camera m_cam;       // The camera
    public CatMovementScript m_bodyMovementScript;

    [Header("Camera Movement Settings")]
    public float m_movementSpeed = 1;
    public float m_timer;

    public Transform[] m_roomPositions;
    private bool m_roomTransitionIsDone = true;

    private void Start()
    {
        m_cam = GetComponent<Camera>();

        transform.position = m_roomPositions[0].position;

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
            if (hoverHit.collider.CompareTag("Cat") && hoverHit.collider.gameObject.GetComponent<CatMovementScript>().m_catState == CatMovementScript.CatState.IsPlaying)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    hoverHit.collider.gameObject.GetComponent<CatMovementScript>().m_isFocused = true;
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

            if(hoverHit.collider.CompareTag("Cockpit Entry"))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(MoveCamera(4));
                }
            }
        }

        if (m_roomTransitionIsDone)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(MoveCamera(0));
            }
            else if (Input.GetKey(KeyCode.A))
            {
                StartCoroutine(MoveCamera(1));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(MoveCamera(2));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(MoveCamera(3));
            }
        }


    }

    private IEnumerator MoveCamera(int roomID)
    {
        m_roomTransitionIsDone = false;

        while (m_timer < 0.99)
        {
            transform.position = Vector3.Lerp(transform.position, m_roomPositions[roomID].position, m_timer);
            m_timer += Time.deltaTime * m_movementSpeed;
            yield return new WaitForEndOfFrame();
        }

        transform.position = m_roomPositions[roomID].position;
        m_roomTransitionIsDone = true;
        m_timer = 0;
    }


}
