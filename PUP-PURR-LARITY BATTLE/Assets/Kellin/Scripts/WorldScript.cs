using UnityEngine;
using Random = UnityEngine.Random;

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


    [Header("Machines Settings")]
    public MachineScript[] m_machineScripts;

    public float m_TimeTillNextIssue = 20f;
    public float m_currentTimeTillIssue;

    [Header("Scripts")]
    public CatMovementScript m_catMovementScript;

    private void Start()
    {
        if (m_exclamationPoint.activeInHierarchy)
        {
            m_exclamationPoint.SetActive(false);
        }
    }

    private void Update()
    {
        CreateIssue();
    }

    public void CreateIssue()
    {
        m_currentTimeTillIssue += Time.deltaTime;

        if (m_currentTimeTillIssue > m_TimeTillNextIssue)
        {
            int value = Random.Range(0, m_machineScripts.Length);
            
            if (!m_machineScripts[value].m_isFixed)
            {
                m_machineScripts[value].BreakDown();
                var type = m_machineScripts[value].m_type;
                MonitorManager.Instance.HandleBreakdown(type);
            }

            m_currentTimeTillIssue = 0;
            m_TimeTillNextIssue = Random.Range(20f, 30f);
        }
    }
}
