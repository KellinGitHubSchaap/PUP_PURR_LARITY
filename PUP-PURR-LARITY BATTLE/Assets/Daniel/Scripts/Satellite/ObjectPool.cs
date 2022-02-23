using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private void Awake()
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
        foreach (var obj in pooledObjects.Where(obj => !obj.activeInHierarchy))
        {
            return obj;
        }

        var gameObj = Instantiate(objectToPool, transform);
        gameObj.SetActive(false);
        pooledObjects.Add(gameObj);
        return gameObj;
    }
}