using System;
using System.Collections.Generic;
using UnityEngine;

public class GunnerPanel : MonoBehaviour
{
    [SerializeField] private Dog dog;
    
    public void OnMouseDown()
    {
        void Shooting() => EnterShooting();
        var callbacks = new List<Action> {Shooting};
        dog.HandleInteract(gameObject, callbacks);
    }

    public void EnterShooting()
    {
        Debug.Log("Gonna enter shooting mode");
        //TODO: Enter satellite shooting mode
    }
}
