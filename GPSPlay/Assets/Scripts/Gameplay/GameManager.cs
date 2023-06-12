using InputSystem;
using Mapbox.Unity.Location;
using Multiplayer;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace GamePlay
{
    [DefaultExecutionOrder(-1)]
    public class GameManager : Singleton<GameManager>
    {
        public List<GameObject> playerObjects = new List<GameObject>();
        public List<GameObject> englishTeam = new List<GameObject>();
        public List<GameObject> italianTeam = new List<GameObject>();
        public List<GameObject> russianTeam = new List<GameObject>();

        public GameObject CheckLocalPlayer()
        {
            foreach (GameObject i in playerObjects)
            {
                if (i.GetComponent<NetworkObject>().IsLocalPlayer)
                {
                    return i;
                }
            }
            return null;
        }
        public GameObject CheckMolePlayer(int teamNum)
        {
            if(teamNum == 0)
            {
                foreach (GameObject i in englishTeam)
                {
                    if (i.GetComponent<PlayerNetwork>().isMole)
                    {
                        return i;
                    }
                }                
            }
            if (teamNum == 1)
            {
                foreach (GameObject i in italianTeam)
                {
                    if (i.GetComponent<PlayerNetwork>().isMole)
                    {
                        return i;
                    }
                }
            }
            if (teamNum == 2)
            {
                foreach (GameObject i in russianTeam)
                {
                    if (i.GetComponent<PlayerNetwork>().isMole)
                    {
                        return i;
                    }
                }
            }
            return null;
        }

        private void Update()
        {
            //Debug.Log(LocationProviderFactory.Instance.DefaultLocationProvider.CurrentLocation.LatitudeLongitude.ToString());
        }

    }
}

