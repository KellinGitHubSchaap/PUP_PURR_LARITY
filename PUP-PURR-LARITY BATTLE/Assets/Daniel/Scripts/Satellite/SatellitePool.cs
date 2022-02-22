using System.Collections.Generic;
using UnityEngine;

public class SatellitePool : MonoBehaviour
{
    public static SatellitePool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        SharedInstance = this;
        
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            var tmp = Instantiate(objectToPool, transform);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetSatellite()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        var newSatellite = Instantiate(objectToPool, transform);
        newSatellite.SetActive(false);
        pooledObjects.Add(newSatellite);
        return newSatellite;
    }
}