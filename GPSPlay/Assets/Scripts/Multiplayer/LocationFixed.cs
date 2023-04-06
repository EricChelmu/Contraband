using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class LocationFixed : MonoBehaviour
    {
        [SerializeField]
        private GameObject mapPrefab;
        private LocationProviderFactory locationProviderFactory;
        private AbstractMap map;

        private void Awake()
        {
            Instantiate(mapPrefab);
            map = GameObject.FindGameObjectWithTag("map").GetComponent<AbstractMap>();
            locationProviderFactory = GetComponent<LocationProviderFactory>();
            locationProviderFactory.mapManager = map;
        }
    }
}
