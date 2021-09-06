using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HealthUI : MonoBehaviour
{
    [SerializeField] Entity _entity = null;
    TextMeshProUGUI _text = null;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (_entity)
        {
            _entity.OnHealthChanged.AddListener(SetHealth);
            SetHealth(_entity);
        }
    }

    public void SetHealth(Entity entity)
    {
        _text.text = $"{entity.CurrentHealth}/{entity.MaxHealth}";
    }
}