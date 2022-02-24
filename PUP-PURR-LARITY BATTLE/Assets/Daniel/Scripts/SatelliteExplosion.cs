using System;
using UnityEngine;

public class SatelliteExplosion : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 1f);
    }
}
