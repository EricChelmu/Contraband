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
    public class TestRelay : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_InputField codeText;
        [SerializeField]
        private TMPro.TMP_Text codeTextHost;
        private string code;
        private async void Start()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private void Update()
        {
            code = codeText.text;

            if (Input.GetKeyDown(KeyCode.R))
            {
                CreateRelay();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {

                JoinRelay(code);
            }
        }

        private async void CreateRelay()
        {
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

                Debug.Log(joinCode);

                RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

                NetworkManager.Singleton.StartHost();

                codeTextHost.text = joinCode;
            }
            catch (RelayServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void JoinRelay(string joinCode)
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
