using System;
using System.Collections.Generic;
using UnityEngine;

public class GunnerActivation : MonoBehaviour
{
    [SerializeField] private Dog dog;
    [SerializeField] private GameObject gunnerView;
    [SerializeField] private CanvasGroup monitorCanvas;
    
    public void OnMouseDown()
    {
        void Shooting() => EnterShooting();
        var callbacks = new List<Action> {Shooting};
        dog.HandleInteract(gameObject, callbacks);
    }

    private void EnterShooting()
    {
        gunnerView.SetActive(true);
        monitorCanvas.alpha = .9f;
    }
}
