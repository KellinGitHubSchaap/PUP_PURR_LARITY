using System;
using System.Collections;
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
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    [ContextMenu("MoveTest")]
    public void MoveTest()
    {
        MoveTo(_transform.position + Vector3.left * 10);
    }

    public void MoveTo(Vector3 position)
    {
        if (_moveToCoroutine != null)
        {
            StopCoroutine(_moveToCoroutine);
        }

        var direction = position.x > _transform.position.x ? LeftRight.Right : LeftRight.Left;
        if (facing != direction)
        {
            StartCoroutine(FlipCoroutine());
        }

        _moveToCoroutine = StartCoroutine(MoveToCoroutine(position));
    }

    private IEnumerator FlipCoroutine()
    {
        var startRotation = _transform.rotation;
        var targetRotation = _transform.rotation * Quaternion.Euler(0, 180, 0);
        
        float t = 0;
        while (t < 1)
        {
            _transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            t += Time.deltaTime / rotationTime;
            yield return null;
        }

        _transform.rotation = targetRotation;
        yield return null;
    }

    private IEnumerator MoveToCoroutine(Vector3 position)
    {
        var direction = (position - _transform.position).normalized;
        var sqrDistance = (_transform.position - position).sqrMagnitude;

        while (sqrDistance > .5f)
        {
            sqrDistance = (_transform.position - position).sqrMagnitude;
            _transform.position += direction * moveSpeed * Time.deltaTime;
            yield return null;
        }

        yield return null;
    }
}