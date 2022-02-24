using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LeftRight
{
    Left,
    Right
}

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private LeftRight facing = LeftRight.Left;
    [SerializeField] private float rotationTime;
    [SerializeField] private float moveSpeed;

    private Coroutine _moveToCoroutine;
    private Coroutine _flipCoroutine;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    [ContextMenu("MoveLeft")]
    public void MoveLeft()
    {
        MoveTo(_transform.position + Vector3.left * 5);
    }

    [ContextMenu("MoveRight")]
    public void MoveRight()
    {
        MoveTo(_transform.position + Vector3.right * 5);
    }

    public void MoveTo(Vector3 position, List<Action> callbacks = null)
    {
        if (_moveToCoroutine != null)
        {
            StopCoroutine(_moveToCoroutine);
        }

        var direction = position.x > _transform.position.x ? LeftRight.Right : LeftRight.Left;
        if (facing != direction)
        {
            if (_flipCoroutine != null)
            {
                StopCoroutine(_flipCoroutine);
            }

            _flipCoroutine = StartCoroutine(FlipCoroutine(direction));
        }

        _moveToCoroutine = StartCoroutine(MoveToCoroutine(position, callbacks));
    }

    private IEnumerator FlipCoroutine(LeftRight direction)
    {
        var startRotation = _transform.rotation;
        var targetRotation = direction == LeftRight.Left
            ? Quaternion.Euler(Vector3.up * 180)
            : Quaternion.Euler(Vector3.zero);

        float t = 0;
        while (t < 1)
        {
            _transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            t += Time.deltaTime / rotationTime;
            yield return null;
        }

        _transform.rotation = targetRotation;
        facing = facing == LeftRight.Left ? LeftRight.Right : LeftRight.Left;
        yield return null;
    }

    private IEnumerator MoveToCoroutine(Vector3 position, List<Action> callbacks = null)
    {
        var defaultY = _transform.position.y;
        position = new Vector3(position.x, defaultY, position.z);

        var direction = (position - _transform.position).normalized;
        var sqrDistance = (_transform.position - position).sqrMagnitude;

        while (sqrDistance > .05f)
        {
            sqrDistance = (_transform.position - position).sqrMagnitude;
            _transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }

        if (callbacks != null)
            foreach (var callback in callbacks)
            {
                callback?.Invoke();
            }

        yield return null;
    }
}