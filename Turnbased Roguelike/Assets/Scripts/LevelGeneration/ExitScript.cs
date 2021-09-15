using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform _saveObject;
    void Start()
    {
        //may not be needed
    }

    // Update is called once per frame
    void Update()
    {
        //when player enters
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _saveObject = GameObject.FindWithTag("Save").transform;
            _saveObject.GetComponent<InfoToSave>().AddLevel();
            SceneManager.LoadScene(1);
        }
    }
}
