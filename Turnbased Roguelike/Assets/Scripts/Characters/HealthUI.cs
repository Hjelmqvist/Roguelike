using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HealthUI : MonoBehaviour
{
    TextMeshProUGUI _text = null;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetHealth(Health health)
    {
        _text.text = $"{health.CurrentHealth}";
    }
}