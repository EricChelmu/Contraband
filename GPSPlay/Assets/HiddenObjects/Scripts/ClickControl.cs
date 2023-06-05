using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControl : MonoBehaviour
{
    public static string obj_name;
    public GameObject obj_txt;

    void OnMouseDown()
    {
        //Link the object and the text
        obj_name = gameObject.name;
        //Debug.Log (obj_name);
        //Destroy the object and the text
        Destroy(gameObject);
        Destroy(obj_txt);
    }
}
