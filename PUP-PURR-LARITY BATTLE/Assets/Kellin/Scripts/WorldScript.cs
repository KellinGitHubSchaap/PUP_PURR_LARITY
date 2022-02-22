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

    public Material m_machineFixedMaterial;     // Temp material to show that a machine is fixed
    public Material m_machineBrokeMaterial;     // Temp material to show that a machine is broken


}
