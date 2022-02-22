using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
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

    void Start()
    {
        m_pickedValue = Random.Range(m_timeTillFlip.x, m_timeTillFlip.y);
        m_time = m_pickedValue;
        m_bodyScale = transform.localScale;
    }

    void Update()
    {
        m_time += Time.deltaTime;

        if (m_time > m_pickedValue)
        {
            FlipBody();
        }

        MoveBody();
    }

    // Move the body
    private void MoveBody()
    {
        Vector3 currentPosition = transform.position;

        currentPosition.x += m_movementSpeed * m_direction * Time.deltaTime;

        transform.position = currentPosition;
    }

    private void FlipBody()
    {
        m_isFlipped = !m_isFlipped;
        m_direction = m_isFlipped ? 1 : -1;
        m_bodyScale.x = m_isFlipped ? -1 : 1;

        transform.localScale = m_bodyScale;
        m_pickedValue = Random.Range(m_timeTillFlip.x, m_timeTillFlip.y);
        m_time = 0;
    }
}
