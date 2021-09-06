using System;
using UnityEngine;

public class Player : Entity
{
    public delegate void PlayerMoved();
    public static event PlayerMoved OnPlayerMoved;

    [SerializeField] float _actionsPerSecond = 4f;
    float _lastMoveTime = float.MinValue;

    private void Update()
    {
        if (Time.time < _lastMoveTime + (1f / _actionsPerSecond))
            return;

        // check inputs
    }

    private void Move(Vector2Int direction)
    {
        Vector2Int newPosition = _currentPosition + direction;
        if (TrySetPosition(newPosition))
        {
            _lastMoveTime = Time.time;
            OnPlayerMoved.Invoke();
        }
    }
}