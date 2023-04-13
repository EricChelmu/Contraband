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
        public GameObject Manager;
        [SerializeField] private Button serverButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private TMP_InputField requestIPButton;
        [SerializeField] private Button requestIPConfirmButton;
        [SerializeField] private Text _statusID;
        [SerializeField] private TMP_Text IPText;
        private string IPOfHost;

        private void Awake()
        {
            serverButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartServer();
            });
            hostButton.onClick.AddListener(() =>
            {
                Manager.GetComponent<UnityTransport>().ConnectionData.Address = ObtainIP.instance.myAddressLocal;
                NetworkManager.Singleton.StartHost();
            });
            clientButton.onClick.AddListener(() =>
            {
                Instantiate(requestIPButton, serverButton.transform.position, serverButton.transform.rotation);
                Instantiate(requestIPConfirmButton, hostButton.transform.position, hostButton.transform.rotation);
            });
            requestIPConfirmButton.onClick.AddListener(() =>
            {
                Manager.GetComponent<UnityTransport>().ConnectionData.Address = IPOfHost;
                NetworkManager.Singleton.StartClient();
            });
        }

        private void Update()
        {
            _statusID.text = Manager.GetComponent<UnityTransport>().ConnectionData.Address;
        }

        public void UpdateIP()
        {
            IPOfHost = IPText.text;
            Debug.Log(IPOfHost);
        }
    }
}
