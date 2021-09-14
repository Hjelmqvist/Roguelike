using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GenScript _levelGenerator;
    [SerializeField] Player _player;
    [SerializeField] float _actionsPerSecond = 4f;
    float _lastMoveTime = float.MinValue;
    bool _isPlayerTurn = true;

    public UnityEvent<Player> OnPlayerTurnEnded;

    void Update()
    {
        if (!_player || !_isPlayerTurn || Time.time < _lastMoveTime + (1f / _actionsPerSecond))
            return;

        foreach (InputDirection moveDir in InputDirection.InputDirections)
        {
            if (Input.GetKeyDown(moveDir.Key) && _player.TryMovePosition(_levelGenerator.Tiles, moveDir.Direction))
            {
                _lastMoveTime = Time.time;
                EndPlayerTurn();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _player.Attack();
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
