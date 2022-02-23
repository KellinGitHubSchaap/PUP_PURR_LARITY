using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyMovementScript : MonoBehaviour
{
    [Header("Body Movement")]
    public float m_movementSpeed = 1;                           // Speed of the body
    private int m_direction = 1;                                // Movement direction of the body
    private float m_rotationTimer;                              // Current time of the rotation animation
    [Range(0.1f, 2f)] public float m_setRotationSpeed = 0.9f;   // How fast should the body Rotate
    [Range(0.1f, 2f)] public float m_currentRotationSpeed;      // How fast should the body Rotate

    [Header("Body Visuals")]
    public Vector2 m_timeTillFlip;      // Time till the body flips around
    public float m_pickedTime;          // Time that has been chosen for when a flip will occur 
    public float m_currentFlipTimer;    // Current time

    public bool m_isFlipped = false;    // Is the body already flipped?
    private bool m_canFlip = true;      // Can the get flip around?

    [Header("Outside of Body Settings")]
    public Transform[] m_borderPoints;  // Points where the body can move freely 
    public Transform m_target;          // Target for the Body to walk to

    public enum CatState { Wandering, GoingToFixSpot, Fixing, Distracted, IsPlaying }
    public CatState m_catState;     // State of the cat

    [Header("Playing State Settings")]
    public float m_timeTillDonePlaying = 7;     // How long untill they stop playing
    public float m_currentTimePlaying;          // Current time that the cat spent playing
    public bool m_isFocused = true;             // Check to see if body is still focused

    [Header("Debug Variables")]
    public bool m_randomFlipTime = false;   // To make the flip time no longer randomized
    public Sprite m_tempDogSprite;
    public Sprite m_tempCatSprite;

    void Start()
    {
        m_catState = CatState.Wandering;
        m_pickedTime = m_randomFlipTime ? Random.Range(m_timeTillFlip.x, m_timeTillFlip.y) : m_pickedTime = m_timeTillFlip.x; // Pick either a random time or a set time based on boolean m_randomTimeFlip

        m_currentRotationSpeed = m_setRotationSpeed;
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
                DistractedState();
                break;
            case CatState.IsPlaying:
                PlayingState();
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

        m_isFlipped = !m_isFlipped;

        while (m_rotationTimer < 0.99)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_rotationTimer);
            m_rotationTimer += Time.deltaTime * m_currentRotationSpeed; // * speed gain || / speed lowered

            yield return new WaitForEndOfFrame();
        }

        transform.rotation = targetRotation;

        m_rotationTimer = 0;
        m_canFlip = true;
    }
    #endregion

    // Inside of this region there are State functions 
    #region State Functions
    private void WanderingState()
    {
        MoveBody();
        m_currentFlipTimer += Time.deltaTime;

        if (WorldScript.instance.m_exclamationPoint.activeInHierarchy)
        {
            WorldScript.instance.m_exclamationPoint.SetActive(false);
        }

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

    // Move the body to the machine that needs fixing
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

        m_target = givenMachine;
        m_currentRotationSpeed = 1.2f;

        if (transform.position.x > m_target.position.x & !m_isFlipped || transform.position.x < m_target.position.x & m_isFlipped)
        {
            m_canFlip = false;
            StartCoroutine(FlipBodyAnimation());
        }

        m_currentRotationSpeed = m_setRotationSpeed;
    }

    // If the Cat is fixing a machine 
    private void FixingState()
    {
        m_currentFlipTimer = 0;

        if (m_target != null)
        {
            m_target = null;
        }
    }

    // Move the body to the place where the distraction happens 
    private void DistractedState()
    {
        MoveBody();
        m_currentFlipTimer = 0;

        if (Vector3.Distance(transform.position, m_target.position) < 1)
        {
            m_catState = CatState.IsPlaying;
            m_target = null;
            m_isFocused = false;
        }
    }

    // Receive information about the place of the distraction
    public void FindDistraction(Transform objectPosition)
    {
        m_catState = CatState.Distracted;
        WorldScript.instance.m_exclamationPoint.SetActive(true);
        m_target = objectPosition;

        m_currentRotationSpeed = 1.2f;
        m_currentFlipTimer = 0;

        m_isFocused = false;

        if (transform.position.x > m_target.position.x & !m_isFlipped || transform.position.x < m_target.position.x & m_isFlipped)
        {
            m_canFlip = false;
            StartCoroutine(FlipBodyAnimation());
        }

        m_currentRotationSpeed = m_setRotationSpeed;
    }

    // If the cat or dog are no longer focused they are going to play with their distraction
    private void PlayingState()
    {
        if (this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite != m_tempDogSprite)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = m_tempDogSprite;
        }

        m_currentTimePlaying += Time.deltaTime;

        if (m_currentTimePlaying > m_timeTillDonePlaying || m_isFocused)
        {
            m_currentTimePlaying = 0;

            this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = m_tempCatSprite;
            m_catState = CatState.Wandering;
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
