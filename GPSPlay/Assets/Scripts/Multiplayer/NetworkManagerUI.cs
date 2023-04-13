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
        public GameObject Manager;
        [SerializeField] private Button serverButton;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        [SerializeField] private Button requestIPButton;

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
            });
            requestIPButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
            });
        }
    }
}
