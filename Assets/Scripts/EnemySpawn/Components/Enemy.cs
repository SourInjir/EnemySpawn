using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Enemy : MonoBehaviour
{
    private const string SpeedKeyName = "Speed";
    private const float MinDistance = 2f;

    [SerializeField] private GameObject _target;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;


    private List<TargetPoint> _targetPoints;
    private int _currentTargetIndex;

    private Vector3 _targetPosition;
    private Vector3 _targetDirection;
    private Coroutine _moveCoroutine;
    private float _maxSlowdownDistance = 3f;
    private float _speed = 4f;

    private void OnEnable()
    {
        selectTargetPoint();
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

    public void SetTargetPoints(List<TargetPoint> targetPoints)
    {
        _targetPoints = targetPoints;
    }


    private IEnumerator MoveLogic()
    {
        while (_targetDirection != null)
        {
            MoveToTarget();
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

    private void MoveToTarget()
    {
        if (_targetPoints == null || _targetPoints[_currentTargetIndex] == null) {
            selectTargetPoint();
            return;
        }

        Vector3 targetPosition = _targetPoints[_currentTargetIndex].transform.position;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        
        if (distanceToTarget <= MinDistance)
        {
            selectTargetPoint();
            return;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        float slowdownFactor = Mathf.Clamp01(distanceToTarget / _maxSlowdownDistance);
        float adjustedSpeed = _speed * slowdownFactor;

        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            _speed * Time.deltaTime
        );

        transform.position = newPosition;
    }

    private void selectTargetPoint()
    {

        if (_targetPoints == null)
        {
            return;
        }

        if (_currentTargetIndex == null || _currentTargetIndex >= _targetPoints.Count - 1)
        {
            _currentTargetIndex = 0;
        }
        else
        {
            _currentTargetIndex++;
        }

    }

}