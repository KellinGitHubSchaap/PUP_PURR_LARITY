using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyMovementScript : MonoBehaviour
{
    [Header("Body Movement")]
    public float m_movementSpeed = 1;   // Speed of the body
    private int m_direction;            // Movement direction of the body

    [Header("Body Visuals")]
    public Vector2 m_timeTillFlip;      // Time till the body flips around
    public float m_pickedValue;         // Time that has been chosen for when a flip will occur 
    public float m_time;                // Current time

    private bool m_isFlipped = false;   // Is the body already flipped?
    private Vector3 m_bodyScale;        // Scale of the body

    [Header("Outside of Body Settings")]
    public Transform[] m_borderPoints;  // Points where the body can move freely 
    private Transform m_target;

    public enum CatState { Wandering, GoingToFixSpot, Fixing, Distracted }
    public CatState m_catState;


    [Header("Debug Variables")]
    public bool m_randomFlipTime = false;   // To make the flip time no longer randomized

    void Start()
    {
        m_catState = CatState.Wandering;

        m_pickedValue = m_randomFlipTime ? Random.Range(m_timeTillFlip.x, m_timeTillFlip.y) : m_pickedValue = m_timeTillFlip.x;
        m_time = m_pickedValue;

        m_bodyScale = transform.localScale;
    }

    void Update()
    {
        if (m_catState == CatState.Wandering)
        {
            if (m_time > m_pickedValue || transform.position.x < m_borderPoints[0].position.x | transform.position.x > m_borderPoints[1].position.x)
            {
                FlipBody();
            }
        }
        else if(m_catState == CatState.GoingToFixSpot)
        {
            if(Vector3.Distance(transform.position, m_target.position) < 1)
            {
                m_catState = CatState.Fixing;
            }
        }

        if (m_catState != CatState.Fixing)
        {
            MoveBody();
            m_time += Time.deltaTime;
        }
        else
        {
            m_time = 0;
        }

    }

    // Move the body
    private void MoveBody()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x += m_movementSpeed * m_direction * Time.deltaTime;

        transform.position = currentPosition;
    }

    // Flip the sprite of the body
    private void FlipBody()
    {
        m_isFlipped = !m_isFlipped;
        m_direction = m_isFlipped ? 1 : -1;
        m_bodyScale.x = m_isFlipped ? -1 : 1;

        transform.localScale = m_bodyScale;
        m_pickedValue = m_randomFlipTime ? Random.Range(m_timeTillFlip.x, m_timeTillFlip.y) : m_pickedValue = m_timeTillFlip.x;
        m_time = 0;
    }

    // Set the target position to be that of the givenMachine that needs fixing + Change cat state
    public void MoveToBrokenMachine(Transform givenMachine)
    {
        m_catState = CatState.GoingToFixSpot;
        m_target = givenMachine;

        if (transform.position.x > m_target.position.x & m_isFlipped || transform.position.x < m_target.position.x & !m_isFlipped)
        {
            FlipBody();
        }

        Debug.Log(givenMachine.position);
    }

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
