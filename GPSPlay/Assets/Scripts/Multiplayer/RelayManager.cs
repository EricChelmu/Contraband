using JetBrains.Annotations;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Multiplayer
{
    public class RelayManager : MonoBehaviour
    {
        public static RelayManager Instance { get; private set; }

        //authorizing player
        private async void Start()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private void Awake()
        {
            Instance = this;
        }

        public async void CreateRelay()
        {
            try
            {
                //allocating relay to best location, example eu-west4
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

                //code that players use to join the relay
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                //starting the relay and server
                NetworkManager.Singleton.StartHost();

                NetworkManagerUI.Instance.codeTextHost.text = joinCode;

                //changing ui
                NetworkManagerUI.Instance.hostButton.gameObject.SetActive(false);
                NetworkManagerUI.Instance.joinButton.gameObject.SetActive(false);
                NetworkManagerUI.Instance.yourCodeIsButton.gameObject.SetActive(true);
                NetworkManagerUI.Instance.codeTextHost.gameObject.SetActive(true);
                NetworkManagerUI.Instance.startGameButton.gameObject.SetActive(true);
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
        }

        //joining server with code
        public async void JoinRelay(string joinCode)
        {
            try
            {
                Debug.Log("Joining relay with " + joinCode);

                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}
