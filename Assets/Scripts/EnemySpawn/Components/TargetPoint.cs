using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class TargetPoint : MonoBehaviour
{
    private const float MinDistance = 2f;

    [SerializeField] private List<Vector3> _targetPositions;

    private Coroutine _moveCoroutine;
    private int _currentTargetIndex;
    private float _speed = 2f;

    private void Start()
    {
        selectTargetPoint();
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(MoveLogic());
    }

    private IEnumerator MoveLogic()
    {
        while (_currentTargetIndex != null)
        {
            MoveToTargetPosition();
            yield return null;
        }
    }

    private void MoveToTargetPosition()
    {

        if (_targetPositions == null || _targetPositions[_currentTargetIndex] == null)
        {
            selectTargetPoint();
            return;
        }

        Vector3 targetPosition = _targetPositions[_currentTargetIndex];
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= MinDistance)
        {
            selectTargetPoint();
            return;
        }

        Vector3 newPosition = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            _speed * Time.deltaTime
        );

        transform.position = newPosition;
    }

    private void selectTargetPoint()
    {

        if (_targetPositions == null)
        {
            return;
        }

        if (_currentTargetIndex == null || _currentTargetIndex >= _targetPositions.Count - 1)
        {
            _currentTargetIndex = 0;
        }
        else
        {
            _currentTargetIndex++;
        }

    }
}