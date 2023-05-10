using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POI_Interact : MonoBehaviour
{
    public Text message;
    public string n_message;

    private void Start()
    {
        message = GameObject.FindGameObjectWithTag("ChatLog").GetComponent<Text>();
    }
    private void OnMouseDown()
    {
        Debug.Log("Clicked");
        message.text = "Hello World!";
    }
}
