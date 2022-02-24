using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DogState
{
    Idle,
    Playing,
    Shooting,
    Walking
}

public class Dog : MonoBehaviour
{
    public DogState state;
    [SerializeField] private float playingChance;

    [SerializeField] private Transform leftWall;
    [SerializeField] private Transform rightWall;

    [SerializeField] private Transform gunnerWalkSpot;
    [SerializeField] private Transform idleWalksSpot;

    private CharacterMovement _movement;
    private float _idleTimer;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _movement = GetComponent<CharacterMovement>();
    }

    private void Start()
    {
        EnterState(DogState.Idle);
    }

    private void Update()
    {
        switch (state)
        {
            case DogState.Idle:
                Idling();
                break;
            default:
                return;
        }
    }

    public void EnterState(DogState newState)
    {
        state = newState;

        switch (newState)
        {
            case DogState.Idle:
                //TODO: Trigger idle animation
                _idleTimer = Random.Range(2f, 4f);
                break;
            case DogState.Playing:
                //TODO: Trigger playing animation
                break;
            case DogState.Shooting:
                break;
            case DogState.Walking:
                //TODO: Trigger walking animation
                break;
        }
    }

    public void HandleInteract(GameObject obj, List<Action> callbacks)
    {
        if (state == DogState.Playing) return;
        if (!obj.CompareTag("GunnerPanel")) return;

        EnterGunnerMode(callbacks);
    }

    private void EnterGunnerMode(List<Action> callbacks)
    {
        void EnterShootState() => EnterState(DogState.Shooting);
        callbacks.Add(EnterShootState);
        EnterState(DogState.Walking);
        void GunnerWalk() => _movement.MoveTo(gunnerWalkSpot.position, callbacks);
        _movement.MoveTo(idleWalksSpot.position, new List<Action>() {GunnerWalk});
    }

    [ContextMenu("ExitGunnerMode")]
    public void ExitGunnerMode()
    {
        void EnterIdleState() => EnterState(DogState.Idle);
        _movement.MoveTo(idleWalksSpot.position, new List<Action>() {EnterIdleState});
    }

    private void Idling()
    {
        _idleTimer -= Time.deltaTime;
        if (_idleTimer > 0) return;

        var rand = Random.Range(0f, 1f);

        if (rand < playingChance)
        {
            EnterState(DogState.Playing);
        }
        else
        {
            RandomWander();
        }
    }

    private void RandomWander()
    {
        EnterState(DogState.Walking);

        var randomX = Random.Range(leftWall.position.x, rightWall.position.x);
        var randomWanderSpot = new Vector3(randomX, _transform.position.y, transform.position.z);

        void State() => EnterState(DogState.Idle);
        var callbacks = new List<Action>() {State};
        _movement.MoveTo(randomWanderSpot, callbacks);
    }
}