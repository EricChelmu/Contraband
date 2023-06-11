using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class NetworkManagerUI : MonoBehaviour
    {
        //gathering references
        public static NetworkManagerUI Instance;
        public GameObject Manager;
        public Button hostButton;
        public Button joinButton;
        public TMP_InputField codeInputField;
        public TMP_Text yourCodeIsButton;
        public TMP_Text codeTextHost;
        public Button startGameButton;
        public Button joinGameButton;
        public GameObject background;
        public GameObject title;
        private string code;
        private void Awake()
        {
            Instance = this;

            //turning off the UI upon starting the game by pressing "host"
            hostButton.onClick.AddListener(() =>
            {
                //relay is created
                //RelayManager.Instance.CreateRelay();
                codeInputField.gameObject.SetActive(false);
                joinGameButton.gameObject.SetActive(false);
                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                startGameButton.gameObject.SetActive(false);
                yourCodeIsButton.gameObject.SetActive(false);
                codeTextHost.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
                title.gameObject.SetActive(false);
                NetworkManager.Singleton.StartHost();
            });

            //joining as client
            joinButton.onClick.AddListener(() =>
            {
                OpenInputField();
                NetworkManager.Singleton.StartClient();
            });

            //joining through relay
            joinGameButton.onClick.AddListener(() =>
            {
                //RelayManager.Instance.JoinRelay(code);
            });

            //unused ui features, that have been temporarily removed because of mapbox bug
            startGameButton.onClick.AddListener(() =>
            {
                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                startGameButton.gameObject.SetActive(false);
                yourCodeIsButton.gameObject.SetActive(false);
                codeTextHost.gameObject.SetActive(false);
                background.gameObject.SetActive(false);
                title.gameObject.SetActive(false);
            });

            //SCRAPPED SEGMENT, HAS BEEN UPDATED TO BE MORE EFFICIENT WITH RELAY AND LOBBY

            //hostButton.onClick.AddListener(() =>
            //{
            //    //Manager.GetComponent<UnityTransport>().ConnectionData.Address = ObtainIP.instance.myAddressLocal;
            //    NetworkManager.Singleton.StartHost();
            //});
            //clientButton.onClick.AddListener(() =>
            //{
            //    //requestIPButton.gameObject.SetActive(true);
            //    //requestIPConfirmButton.gameObject.SetActive(true);
            //    NetworkManager.Singleton.StartClient();
            //});
            //requestIPConfirmButton.onClick.AddListener(() =>
            //{
            //    NetworkManager.Singleton.StartClient();
            //});
        }

        //take input from players
        private void Update()
        {
            code = codeInputField.text;
        }

        private void OpenInputField()
        {
            codeInputField.gameObject.SetActive(false);
            joinGameButton.gameObject.SetActive(false);
            hostButton.gameObject.SetActive(false);
            joinButton.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(false);
            yourCodeIsButton.gameObject.SetActive(false);
            codeTextHost.gameObject.SetActive(false);
            background.gameObject.SetActive(false);
            title.gameObject.SetActive(false);
        }

        //scrapped automatic IP updating, better version being used with relay and lobby

        //private void Update()
        //{
        //    _statusID.text = Manager.GetComponent<UnityTransport>().ConnectionData.Address;
        //}

        //public void UpdateIP()
        //{
        //    IPOfHost = (requestIPButton.text);
        //    Debug.Log(IPOfHost);
        //    Manager.GetComponent<UnityTransport>().ConnectionData.Address = IPOfHost;
        //}
    }
}
