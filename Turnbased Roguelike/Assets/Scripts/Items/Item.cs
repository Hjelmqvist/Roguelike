using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string itemName;
    // public
    [SerializeField] int price;

    public string ItemName => itemName;
    public int Price => price;

    public virtual bool Interact(Player player)
    {
        return false;
    }
}