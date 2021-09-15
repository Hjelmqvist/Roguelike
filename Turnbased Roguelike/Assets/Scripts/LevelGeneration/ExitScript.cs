using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : Item
{
    // Start is called before the first frame update
    private GameObject _saveObject;
    private GameObject _boardHolder;
    private GameObject _manager;
    private GameObject _enemyController;
    public override bool Interact(Player player)
    {
        _manager = GameObject.FindWithTag("GameController");
        _saveObject = GameObject.FindWithTag("Save");
        _saveObject.GetComponent<InfoToSave>().AddLevel();
        _boardHolder = GameObject.Find("Board");
        _enemyController = GameObject.Find("EnemyController");
        Destroy(_boardHolder.gameObject);
        _enemyController.GetComponent<EnemyController>().ClearEnemyList();
        _manager.GetComponent<GenScript>().MapGeneration();
        return true;
    }
}
