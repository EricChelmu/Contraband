using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Multiplayer
{
    public class LobbyManager1 : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TMP_InputField codeText;
        [SerializeField]
        private TMPro.TMP_Text codeTextHost;
        private Lobby hostLobby;
        private Lobby joinedLobby;
        private float heartbeatTimer;
        private float lobbyUpdateTimer;
        private string playerName;
        private string code;
        private async void Start()
        {
            await UnityServices.InitializeAsync();

            //authenticate the player with unity ID
            AuthenticationService.Instance.SignedIn += () =>
            {
                Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            playerName = "player" + UnityEngine.Random.Range(10, 99);
            Debug.Log(playerName);
        }

        private void Update()
        {
            //testing connection through hotkeys ingame
            code = codeText.text;
            HandleLobbyHeartbeat();
            HandleLobbyPollForUpdates();
            if (Input.GetKeyDown(KeyCode.C))
            {
                CreateLobby();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                ListLobbies();
            }
            if (Input.GetKeyDown(KeyCode.J))
            {

                JoinLobbyByCode(code);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                QuickJoinLobby();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                PrintPlayers();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                LeaveLobby();
            }
        }

        //creating lobby with custom options and data like map and gamemode as an example
        private async void CreateLobby()
        {
            try
            {
                string lobbyName = "MyLobby";
                int maxPlayers = 4;
                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
                {
                    IsPrivate = false,
                    Player = GetPlayer(),
                    Data = new Dictionary<string, DataObject>
                    {
                        { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag") },
                        { "Map", new DataObject(DataObject.VisibilityOptions.Public, "de_dust2") }
                    }
                };
                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

                hostLobby = lobby;
                joinedLobby = hostLobby;

                Debug.Log("Created lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);

                codeTextHost.text = lobby.LobbyCode;

                PrintPlayers(hostLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        //listing lobbies based on custom search
        private async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
                {
                    Count = 25,
                    Filters = new List<QueryFilter>
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                    },
                    Order = new List<QueryOrder>
                    {
                        new QueryOrder(false, QueryOrder.FieldOptions.Created)
                    }
                };
                QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                Debug.Log("Lobbies found: " + queryResponse.Results.Count);
                foreach (Lobby lobby in queryResponse.Results)
                {
                    Debug.Log(lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Data["GameMode"].Value);
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void JoinLobbyByCode(string lobbyCode)
        {
            try
            {
                JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
                {
                    Player = GetPlayer(),
                };
                Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
                joinedLobby = lobby;

                Debug.Log("Joined lobby with code " + lobbyCode);
                PrintPlayers(lobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }

        }

        //lobby heartbeat so that the lobby doesnt shut down automatically if no player joins within 15 seconds
        //the shutdown is the default setting for the lobby service
        private async void HandleLobbyHeartbeat()
        {
            if (hostLobby != null)
            {
                heartbeatTimer -= Time.deltaTime;
                if (heartbeatTimer < 0f)
                {
                    float heartbeatTimerMax = 15f;
                    heartbeatTimer = heartbeatTimerMax;

                    await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
                }
            }
        }

        //clients request an update from the host lobby every second
        private async void HandleLobbyPollForUpdates()
        {
            if (joinedLobby != null)
            {
                lobbyUpdateTimer -= Time.deltaTime;
                if (lobbyUpdateTimer < 0f)
                {
                    float lobbyUpdateTimerMax = 1.1f;
                    lobbyUpdateTimer = lobbyUpdateTimerMax;

                    Lobby lobby = await LobbyService.Instance.GetLobbyAsync(joinedLobby.Id);
                    joinedLobby = lobby;
                }
            }
        }

        //quick join any open lobby
        private async void QuickJoinLobby()
        {
            try
            {
                await LobbyService.Instance.QuickJoinLobbyAsync();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        //obtaining player name
        private Player GetPlayer()
        {
            return new Player
            {
                Data = new Dictionary<string, PlayerDataObject>
                        {
                            { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                        }
            };
        }

        private void PrintPlayers()
        {
            PrintPlayers(joinedLobby);
        }

        private void PrintPlayers(Lobby lobby)
        {
            Debug.Log("Players in lobby " + lobby.Name + " " + lobby.Data["GameMode"].Value + " " + lobby.Data["Map"].Value);
            foreach (Player player in lobby.Players)
            {
                Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
            }
        }

        private async void UpdateLobbyGameMode(string gameMode)
        {
            try
            {
                hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
                {
                    Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
                });
                joinedLobby = hostLobby;

                PrintPlayers(hostLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void UpdatePlayerName(string newPlayerName)
        {
            try
            {
                playerName = newPlayerName;
                await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
                {
                    Data = new Dictionary<string, PlayerDataObject>
                    {
                        { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
                    }
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void LeaveLobby()
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void KickPlayer()
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void DeleteLobby()
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}