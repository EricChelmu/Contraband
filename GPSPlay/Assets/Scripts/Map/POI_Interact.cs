using GamePlay;
using Mapbox.Unity.Location;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public class POI_Interact : MonoBehaviour
    {
        //private QuestManager _questManager;
        public TMP_Text message;
        //private string my_message;

        private GameManager gameManager;

        private void Start()
        {
            message = GameObject.FindGameObjectWithTag("ChatLog").GetComponent<TMP_Text>();
            //_questManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<QuestManager>();
            //my_message = _questManager.n_message[0];
            //_questManager.n_message.Remove(my_message);

            gameManager = GameManager.Instance;
        }

        private void Update()
        {
            if (gameManager.playerObjects.Count > 0)
            {
                float playerDistance = Vector3.Distance(gameManager.CheckLocalPlayer().transform.position, transform.position);
                if (playerDistance < 20f)
                {
                    gameObject.GetComponent<Animator>().speed = 3.5f;
                }
                else
                {
                    gameObject.GetComponent<Animator>().speed = 1f;
                }
            }            
        }
        private void OnMouseDown()
        {
            float playerDistance = Vector3.Distance(gameManager.CheckLocalPlayer().transform.position, transform.position);
            if (playerDistance <= 20f)
            {
                Debug.Log(playerDistance);
            }
            message.text = playerDistance.ToString();
        }
    }
}
