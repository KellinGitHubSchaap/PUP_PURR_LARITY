using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
    public static WorldScript instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    [Header("Temp Materials")]
    public Material m_machineFixedMaterial;     // Temp material to show that a machine is fixed
    public Material m_machineBrokeMaterial;     // Temp material to show that a machine is broken

    [Header("Distraction Settings")]
    public GameObject m_exclamationPoint;           // Exclamationpoint seen when a Bird or Fish distracts one of the animals
    public float m_timeTillMakeDistraction = 10f;   // Time till a new distraction is thrown into the world
    public float m_currentMakeDistractionTimer;     // Current time for when a new distraction is made

    public Transform m_positionOfDistraction;       // Where is the distraction coming from

    [Header("Scripts")]
    public BodyMovementScript m_catMovementScript;

    private void Start()
    {
        if (m_exclamationPoint.activeInHierarchy)
        {
            m_exclamationPoint.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_catMovementScript.FindDistraction(m_positionOfDistraction);
        }
    }
}
