using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenScript : MonoBehaviour
{
   [SerializeField] private GameObject prefab;
   // Start is called before the first frame update
    void Start()
    {
        GameObject newPart = Instantiate(prefab);
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Connector"))
            {
                newPart.transform.position = new Vector2(child.transform.position.x *2, child.transform.position.y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
