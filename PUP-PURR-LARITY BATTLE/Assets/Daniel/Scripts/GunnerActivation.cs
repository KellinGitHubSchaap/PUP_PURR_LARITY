using System;
using System.Collections.Generic;
using UnityEngine;

public class GunnerActivation : MonoBehaviour
{
    [SerializeField] private Dog dog;
    [SerializeField] private GameObject gunnerView;
    
    public void OnMouseDown()
    {
        void Shooting() => EnterShooting();
        var callbacks = new List<Action> {Shooting};
        dog.HandleInteract(gameObject, callbacks);
    }

    public void EnterShooting()
    {
        gunnerView.SetActive(true);
    }
}
