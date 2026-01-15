using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Enemy : MonoBehaviour
{
    private const string SpeedKeyName = "Speed";

    [SerializeField] private GameObject _target;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _targetPosition;
    private Vector3 _targetDirection;
    private Coroutine _moveCoroutine;
    private float _maxSlowdownDistance = 3f;
    private float _speed = 2f;

    private void OnEnable()
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MoveLogic());
        _animator.SetFloat(SpeedKeyName, _speed);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public void SetTargetDirection(Vector3 targetDirection)
    {
        _targetDirection = targetDirection;
    }

    private IEnumerator MoveLogic()
    {
        while (_targetDirection != null)
        {
            MoveToDirection();
            yield return null;
        }
    }

    private void MoveToDirection()
    {
        if(_targetDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_targetDirection);

        Vector3 movement = _targetDirection * _speed * Time.fixedDeltaTime;
        
        if(_rigidbody != null)
            _rigidbody.MovePosition(_rigidbody.position + movement);

    }

    private void MoveToTargetPosition()
    {
        Vector3 direction = (_targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        float distanceToTarget = Vector3.Distance(transform.position, _targetPosition);
        float slowdownFactor = Mathf.Clamp01(distanceToTarget / _maxSlowdownDistance);

        float adjustedSpeed = _speed * slowdownFactor;

        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            _targetPosition,
            adjustedSpeed * Time.deltaTime
        );

        transform.position = newPosition;
    }
}