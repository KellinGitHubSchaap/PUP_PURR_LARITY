using System;
using System.Collections.Generic;
using UnityEngine;

public enum DogState
{
    Idle,
    IsPlaying,
    Shooting,
    Walking
}

public class Dog : MonoBehaviour
{
    public DogState state;
    private CharacterMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
    }

    public void HandleInteract(GameObject obj, List<Action> callbacks)
    {
        if (state == DogState.IsPlaying) return;
        if (!obj.CompareTag("GunnerPanel")) return;
        
        void State() => EnterState(DogState.Shooting);
        callbacks.Add(State);
        _movement.MoveTo(obj.transform.position, callbacks);
    }

    private void EnterState(DogState newState)
    {
        state = newState;

        switch (newState)
        {
            case DogState.Idle:
                break;
            case DogState.IsPlaying:
                break;
            case DogState.Shooting:
                break;
            case DogState.Walking:
                break;
        }
    }

    private void Shoot()
    {
        throw new NotImplementedException();
    }

    private void Distract()
    {
        throw new NotImplementedException();
    }

    private void Idling()
    {
        throw new NotImplementedException();
    }
}