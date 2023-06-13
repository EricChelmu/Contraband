using GamePlay;
using Mapbox.Unity.Location;
using Multiplayer;
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

        public Button trapButton;

        public bool isTrapped;

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
                    if(gameManager.CheckLocalPlayer().GetComponent<PlayerNetwork>().isMole && isTrapped == false)
                    {
                        trapButton.gameObject.SetActive(true);
                    }
                }
                else
                {
                    gameObject.GetComponent<Animator>().speed = 1f;
                    trapButton.gameObject.SetActive(false);
                }

                if (trapButton == true)
                {
                    isTrapped = true;
                }
            }            
        }
        private void OnMouseDown()
        {
            float playerDistance = Vector3.Distance(gameManager.CheckLocalPlayer().transform.position, transform.position);
            if (playerDistance <= 20f)
            {
                if (isTrapped != true)
                {
                    riddle.SetActive(true);
                }
                else
                {
                    //play trap minigame
                    LockDown();                    
                }
            }            
        }
        private void LockDown()
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            poiSkin[0].SetActive(false);

            ExecuteAfterTime(60);
        }

        private bool isCoroutineExecuting = false;

        IEnumerator ExecuteAfterTime(float time)
        {
            if (isCoroutineExecuting)
                yield break;

            isCoroutineExecuting = true;

            yield return new WaitForSeconds(time);

            // Code to execute after the delay
            gameObject.GetComponent<BoxCollider>().enabled = true;
            poiSkin[0].SetActive(true);
            isTrapped = false;

            isCoroutineExecuting = false;
        }
    }
}
