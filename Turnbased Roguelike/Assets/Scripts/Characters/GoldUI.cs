using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GoldUI : MonoBehaviour
{
    TextMeshProUGUI _text = null;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetGold(int gold)
    {
        _text.text = $"{gold}";
    }
}