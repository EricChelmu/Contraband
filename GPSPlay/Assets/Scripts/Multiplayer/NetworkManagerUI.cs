using GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class NetworkManagerUI : MonoBehaviour
    {
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

            hostButton.onClick.AddListener(() =>
            {
                RelayManager.Instance.CreateRelay();
                codeInputField.gameObject.SetActive(false);
                joinGameButton.gameObject.SetActive(false);
                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                startGameButton.gameObject.SetActive(true);
                yourCodeIsButton.gameObject.SetActive(true);
                codeTextHost.gameObject.SetActive(true);

                //NetworkManager.Singleton.StartHost();
                //NetworkManager.Singleton.StartServer();
            });

            joinButton.onClick.AddListener(() =>
            {
                OpenInputField();                
                //NetworkManager.Singleton.StartClient();                
            });

            joinGameButton.onClick.AddListener(() =>
            {
                RelayManager.Instance.JoinRelay(code);
            });

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

        private void Update()
        {
            code = codeInputField.text;
        }

        private void OpenInputField()
        {
            codeInputField.gameObject.SetActive(true);
            joinGameButton.gameObject.SetActive(true);
            hostButton.gameObject.SetActive(false);
            joinButton.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(false);
            yourCodeIsButton.gameObject.SetActive(false);
            codeTextHost.gameObject.SetActive(false);
        }

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
