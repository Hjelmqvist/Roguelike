using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{
    [SerializeField] float _actionsPerSecond = 4f;
    float _lastMoveTime = float.MinValue;
    bool _isMyTurn = true;

    [SerializeField] Weapon _heldWeapon;
    [SerializeField] Attack _basicAttack;

    public Attack CurrentWeapon => _heldWeapon ? _heldWeapon.WeaponAttack : _basicAttack;

    public UnityEvent<Player> OnPlayerTurnEnded;

    private void Update()
    {
        if (!_isMyTurn || Time.time < _lastMoveTime + (1f / _actionsPerSecond))
            return;

        foreach (InputDirection moveDir in InputDirection.InputDirections)
        {
            if (Input.GetKeyDown(moveDir.Key) && TryMovePosition(moveDir.Direction))
            {
                _lastMoveTime = Time.time;
                EndPlayerTurn();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Attack(CurrentWeapon);
    }

    public void StartPlayerTurn()
    {
        _isMyTurn = true;
    }

    private void EndPlayerTurn()
    {
        _isMyTurn = false;
        OnPlayerTurnEnded.Invoke(this);
    }
}