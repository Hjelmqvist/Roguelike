using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    int _health = 100;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _health;

    public UnityEvent<Health> OnHealthChanged;

    private void Start()
    {
        _health = _maxHealth;

        // Invoke the event once on start so that other scripts can get the values without a reference.
        OnHealthChanged.Invoke(this);
    }

    public void ModifyHealth(int value)
    {
        _health = Mathf.Clamp(_health + value, 0, _maxHealth);
        OnHealthChanged.Invoke(this);
    }
}