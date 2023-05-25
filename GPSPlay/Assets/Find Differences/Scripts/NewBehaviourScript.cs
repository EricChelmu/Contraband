using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public static string obj_name;
    public GameObject obj_txt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        obj_name = gameObject.name;
        Destroy(gameObject);
        Destroy(obj_txt);
    }
}
