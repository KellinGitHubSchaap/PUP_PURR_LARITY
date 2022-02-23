using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            var gameObj = Instantiate(objectToPool, transform);
            gameObj.SetActive(false);
            pooledObjects.Add(gameObj);
        }
    }

    public GameObject GetObject()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        var gameObj = Instantiate(objectToPool, transform);
        gameObj.SetActive(false);
        pooledObjects.Add(gameObj);
        return gameObj;
    }
}