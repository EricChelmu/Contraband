using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
    public class NetworkManagerUI : MonoBehaviour
    {
        public ObtainIP obtainIP;
        [SerializeField] private Button serverButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;

        private void Awake()
        {
            serverButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartServer();
            });
            hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
                GetComponent<UnityTransport>().ConnectionData.Address = obtainIP.myAddressLocal;
            });
            clientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
            });
        }
    }
}
