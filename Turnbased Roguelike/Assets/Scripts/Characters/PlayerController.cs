using UnityEngine;
using UnityEngine.Events;

public partial class PlayerController : MonoBehaviour
{
    [SerializeField] GenScript _levelGenerator;
    [SerializeField] Player _player;
    [SerializeField] float _actionsPerSecond = 4f;
    [SerializeField]
    KeyCode _upKey = KeyCode.W,
                             _leftKey = KeyCode.A,
                             _downKey = KeyCode.S,
                             _rightKey = KeyCode.D,
                             _attackKey = KeyCode.Space,
                             _interactKey = KeyCode.E;

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

        if (TryMove() || TryAttack() || TryInteract())
        {
            _lastMoveTime = Time.time;
            EndPlayerTurn();
            return;
        }
    }

    /// <summary>
    /// Returns true if player moved or attacked
    /// </summary>
    private bool TryMove()
    {
        foreach (InputDirection inputDirection in _inputDirections)
        {
            if (Input.GetKeyDown(inputDirection.Key) &&
                (_player.TryMovePosition(_levelGenerator.Tiles, inputDirection.Direction) || // Try to move in the direction
                 _player.Attack(_levelGenerator.Tiles)))                                     // Else try to attack in the direction)
            {
                return true;
            }
        }
        return false;
    }

    private bool TryAttack()
    {
        if (Input.GetKeyDown(_attackKey))
        {
            _player.Attack(_levelGenerator.Tiles);
            return true;
        }
        return false;
    }

    private bool TryInteract()
    {
        if (Input.GetKeyDown(_interactKey) && _player.Interact())
            return true;
        return false;
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