using UnityEngine;

public class InputDirection
{
    private KeyCode _key;
    private Vector2Int _direction;

    public KeyCode Key => _key;
    public Vector2Int Direction => _direction;

    public static InputDirection[] InputDirections { get; } = new InputDirection[]
    {
        new InputDirection(KeyCode.W, Vector2Int.up),
        new InputDirection(KeyCode.S, Vector2Int.down),
        new InputDirection(KeyCode.A, Vector2Int.left),
        new InputDirection(KeyCode.D, Vector2Int.right)
    };

    public InputDirection(KeyCode key, Vector2Int direction)
    {
        _key = key;
        _direction = direction;
    }
}