using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Mapbox.Unity.Location;
using Mapbox.Examples;
using Unity.Netcode.Transports.UTP;
using InputSystem;
using GamePlay;
using Unity.VisualScripting;

namespace Multiplayer
{
    public class PlayerNetwork : NetworkBehaviour
    {
        private GameManager gameManager;
        public GameObject[] playerSkins;
        public enum Teams
        { 
            English,
            Italian,
            Russian,
        }
        public Teams team = new Teams();
        public bool isMole;


        private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData
        {
            _int = 56,
            _bool = false,
    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        public struct MyCustomData : INetworkSerializable
        {
            public int _int;
            public bool _bool;
            public string _message;
            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref _int);
                serializer.SerializeValue(ref _bool);
            }
        }

        public override void OnNetworkSpawn()
        {
            randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
            {
                Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool);
            };
            //get game managers instance
            gameManager = GameManager.Instance;
            //add yourself to the player list
            gameManager.playerObjects.Add(this.gameObject);

            if(gameManager.russianTeam.Count < gameManager.italianTeam.Count)
            {
                gameManager.russianTeam.Add(this.gameObject);
                team = Teams.Russian;
                playerSkins[2].SetActive(true);
                if (gameManager.CheckMolePlayer(2) == null)
                {
                    MoleSelection(2);
                }
            }
            else if(gameManager.italianTeam.Count < gameManager.englishTeam.Count)
            {
                gameManager.italianTeam.Add(this.gameObject);
                team = Teams.Italian;
                playerSkins[1].SetActive(true);
                if (gameManager.CheckMolePlayer(1) == null)
                {
                    MoleSelection(1);
                }
            }
            else
            {
                gameManager.englishTeam.Add(this.gameObject);
                team = Teams.English;
                playerSkins[0].SetActive(true);
                if(gameManager.CheckMolePlayer(0) == null)
                {
                    MoleSelection(0);
                }
            }
        }

        public override void OnNetworkDespawn()
        {
            //get game managers instance
            gameManager = GameManager.Instance;
            //remove yourself from the player list
            gameManager.playerObjects.Remove(this.gameObject);
            if(team == Teams.English)
            {
                gameManager.englishTeam.Remove(this.gameObject);
                playerSkins[0].SetActive(false);
            }
            else if (team == Teams.Italian)
            {
                gameManager.italianTeam.Remove(this.gameObject);
                playerSkins[1].SetActive(false);
            }
            else if (team == Teams.Russian)
            {
                gameManager.russianTeam.Remove(this.gameObject); 
                playerSkins[2].SetActive(false);
            }

            if(isMole == true)
            {
                isMole = false;
            }
        }
        private void MoleSelection(int teamNum)
        {
            if (teamNum == 0 && Random.Range(gameManager.englishTeam.Count, 5) == 5)
            {
                isMole = true;
            }
            else if (teamNum == 1 && Random.Range(gameManager.italianTeam.Count, 5) == 5)
            {
                isMole = true;
            }
            else if (teamNum == 2 && Random.Range(gameManager.russianTeam.Count, 5) == 5)
            {
                isMole = true;
            }
            else
            {
                isMole = false;
            }
        }
        

        bool _isInitialized;

        ILocationProvider _locationProvider;
        ILocationProvider LocationProvider
        {
            get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
                }

                return _locationProvider;
            }
        }
        void Start()
        {
            LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
        }

        void LateUpdate()
        {
            if (!IsOwner) return;
            //if (_isInitialized)
            //{
            //    var map = LocationProviderFactory.Instance.mapManager;
            //    transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            //}
            var map = LocationProviderFactory.Instance.mapManager;
            transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
        }
    }
}