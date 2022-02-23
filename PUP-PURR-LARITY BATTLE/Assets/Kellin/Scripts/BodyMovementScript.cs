using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyMovementScript : MonoBehaviour
{
    [Header("Body Movement")]
    public float m_movementSpeed = 1;                           // Speed of the body
    private int m_direction = 1;                                // Movement direction of the body
    private float m_rotationTimer;                              // Current time of the rotation animation
    [Range(0.1f, 2f)] public float m_rotationSpeed = 1.2f;      // How fast should the body Rotate

    [Header("Body Visuals")]
    public Vector2 m_timeTillFlip;      // Time till the body flips around
    public float m_pickedTime;          // Time that has been chosen for when a flip will occur 
    public float m_currentFlipTimer;    // Current time

    public bool m_isFlipped = false;   // Is the body already flipped?
    private bool m_canFlip = true;

    [Header("Outside of Body Settings")]
    public Transform[] m_borderPoints;  // Points where the body can move freely 
    private Transform m_target;         // Target for the Body to walk to

    public enum CatState { Wandering, GoingToFixSpot, Fixing, Distracted }
    public CatState m_catState;

    [Header("Fixing State Settings")]
    public float m_timeTillDoneFixing = 5;
    public float m_currentFixingProgress;

    [Header("Debug Variables")]
    public bool m_randomFlipTime = false;   // To make the flip time no longer randomized

    void Start()
    {
        m_catState = CatState.Wandering;

        m_pickedTime = m_randomFlipTime ? Random.Range(m_timeTillFlip.x, m_timeTillFlip.y) : m_pickedTime = m_timeTillFlip.x;
    }

    void Update()
    {
        switch (m_catState)
        {
            case CatState.Wandering:
                WanderingState();
                break;
            case CatState.GoingToFixSpot:
                GoingToFixSpotState();
                break;
            case CatState.Fixing:
                FixingState();
                break;
            case CatState.Distracted:
                break;
        }
    }

    // Inside of this region there are Move functions
    #region Move Fucntions
    // Move the body
    private void MoveBody()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x += m_movementSpeed * m_direction * Time.deltaTime;

        transform.position = currentPosition;
    }

    // Flip the sprite of the body
    private IEnumerator FlipBodyAnimation()
    {
        m_direction = m_isFlipped ? 1 : -1;
        
        m_currentFlipTimer = 0;
        m_pickedTime = m_randomFlipTime ? Random.Range(m_timeTillFlip.x, m_timeTillFlip.y) : m_pickedTime = m_timeTillFlip.x;

        float newRotation = m_isFlipped ? 0 : 180;
        Quaternion targetRotation = Quaternion.Euler(0, newRotation, 0);

        while (m_rotationTimer < 0.99)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationTimer);
            m_rotationTimer += Time.deltaTime * m_rotationSpeed; // * speed gain || / speed lowered

            yield return new WaitForEndOfFrame();
        }

        transform.rotation = targetRotation;

        m_rotationTimer = 0;
        m_canFlip = true;
        m_isFlipped = !m_isFlipped;
    }
    #endregion

    // Inside of this region there are State functions 
    #region State Functions
    private void WanderingState()
    {
        MoveBody();
        m_currentFlipTimer += Time.deltaTime;

        if (m_canFlip)
        {
            if (m_currentFlipTimer > m_pickedTime || transform.position.x < m_borderPoints[0].position.x | transform.position.x > m_borderPoints[1].position.x)
            {
                m_canFlip = false;
                StartCoroutine(FlipBodyAnimation());
                Debug.Log("FLIP!");
            }
        }
    }

    private void GoingToFixSpotState()
    {
        MoveBody();
        m_currentFlipTimer = 0;

        if (Vector3.Distance(transform.position, m_target.position) < 1)
        {
            m_catState = CatState.Fixing;
        }
    }

    // Set the target position to be that of the givenMachine that needs fixing + Change cat state
    public void MoveToBrokenMachine(Transform givenMachine)
    {
        m_catState = CatState.GoingToFixSpot;

        m_currentFixingProgress = 0;
        m_target = givenMachine;

        if (transform.position.x > m_target.position.x & !m_isFlipped || transform.position.x < m_target.position.x & m_isFlipped)
        {
            m_canFlip = false;
            StartCoroutine(FlipBodyAnimation());
            Debug.Log("Cat goes fixing");
        }
    }

    private void FixingState()
    {
        m_currentFlipTimer = 0;

        m_currentFixingProgress += Time.deltaTime;

        if (m_currentFixingProgress > m_timeTillDoneFixing)
        {
            m_target.GetComponent<MeshRenderer>().material = WorldScript.instance.m_machineFixedMaterial;
            m_target.tag = "Fixed";

            m_catState = CatState.Wandering;
            m_currentFixingProgress = 0;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (m_borderPoints != null)
        {
            for (int i = 0; i < m_borderPoints.Length; i++)
            {
                Gizmos.DrawWireCube(m_borderPoints[i].position, Vector3.one);
            }
        }
    }
}
