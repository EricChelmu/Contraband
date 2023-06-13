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
        private GameManager gameManager;
        [SerializeField] private GameObject riddle;

        public GameObject[] poiSkin;

        private void Start()
        {
            gameManager = GameManager.Instance;
            if(gameManager.riddles != null )
            {
                int riddleNum = Random.Range(0, gameManager.riddles.Count);
                riddle = gameManager.riddles[riddleNum];
                gameManager.riddles.Remove(riddle);
                if (riddleNum == 9)
                {
                    poiSkin[0].SetActive(false);
                    poiSkin[1].SetActive(true);
                }
            }            
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
                riddle.SetActive(true);
            }
        }
    }
}
