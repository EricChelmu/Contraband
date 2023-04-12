using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Mapbox.Unity.Location;
using Mapbox.Examples;
using Unity.Netcode.Transports.UTP;

namespace Multiplayer
{
    public class PlayerNetwork : NetworkBehaviour
    {
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
            if (_isInitialized)
            {
                //var map = LocationProviderFactory.Instance.mapManager;
                //transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            }
            var map = LocationProviderFactory.Instance.mapManager;
            transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
        }
    }
}