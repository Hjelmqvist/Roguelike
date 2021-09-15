using System;
using UnityEngine;
using UnityEngine.Events;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] GenScript _levelGenerator;
    [SerializeField] Player _player;
    [SerializeField] float _actionsPerSecond = 4f;
    [SerializeField] KeyCode _upKey = KeyCode.W, 
                             _leftKey = KeyCode.A, 
                             _downKey = KeyCode.S, 
                             _rightKey = KeyCode.D;

    float _lastMoveTime = float.MinValue;
    bool _isPlayerTurn = true;

    InputDirection[] _inputDirections;

    public UnityEvent<Player> OnPlayerTurnEnded;

    private void Awake()
    {
        _inputDirections = new InputDirection[] {
            new InputDirection(_upKey, Vector2Int.up),
            new InputDirection(_leftKey, Vector2Int.left),
            new InputDirection(_downKey, Vector2Int.down),
            new InputDirection(_rightKey, Vector2Int.right)
        };
    }

    void Update()
    {
        if (!_player || !_isPlayerTurn || Time.time < _lastMoveTime + (1f / _actionsPerSecond))
            return;

        foreach (InputDirection inputDirection in _inputDirections)
        {
            if (Input.GetKeyDown(inputDirection.Key) && _player.TryMovePosition(_levelGenerator.Tiles, inputDirection.Direction))
            {
                _lastMoveTime = Time.time;
                EndPlayerTurn();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Attack(_levelGenerator.Tiles);
            EndPlayerTurn();
            return;
        }    
    }

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void StartPlayerTurn()
    {
        _isPlayerTurn = true;
    }

    private void EndPlayerTurn()
    {
        _isPlayerTurn = false;
        OnPlayerTurnEnded.Invoke(_player);
    }
}
