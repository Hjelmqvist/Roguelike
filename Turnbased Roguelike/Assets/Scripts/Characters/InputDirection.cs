using UnityEngine;

public class InputDirection
{
    KeyCode _key;
    Vector2Int _direction;

    public KeyCode Key => _key;
    public Vector2Int Direction => _direction;

    public InputDirection(KeyCode key, Vector2Int direction)
    {
        _key = key;
        _direction = direction;
    }
}