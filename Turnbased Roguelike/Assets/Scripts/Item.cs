using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item : MonoBehaviour
{
    public int price;

    public virtual bool Interact(Player player)
    {
        return false;
    }
}
