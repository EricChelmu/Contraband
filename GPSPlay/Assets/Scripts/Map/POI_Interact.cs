using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class POI_Interact : MonoBehaviour
    {
        private QuestManager _questManager;
        public Text message;
        private string my_message;

        private void Start()
        {
            message = GameObject.FindGameObjectWithTag("ChatLog").GetComponent<Text>();
            _questManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<QuestManager>();
            my_message = _questManager.n_message[0];
            _questManager.n_message.Remove(my_message);
        }
        private void OnMouseDown()
        {
            message.text = my_message;
        }
    }
}
