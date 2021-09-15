using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int price;
    public string Type;

    public void Interact(int CurrentGold)
    {
        if (price != 0)
        {
            if (price < CurrentGold)
            {
                
            }
        }
        else
        {
            
        }
    }
}
