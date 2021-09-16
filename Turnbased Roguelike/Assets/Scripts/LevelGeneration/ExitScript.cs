using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : Item
{
    // Start is called before the first frame update
    private GameObject _manager;
    public override bool Interact(Player player)
    {
        _manager = GameObject.FindWithTag("GameController");
        _manager.GetComponent<GenScript>().NextLevel();
        return true;
    }
}
