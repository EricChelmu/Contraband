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
using UnityEngine.UIElements;
using TMPro;

namespace Multiplayer
{
    public class PlayerNetwork : NetworkBehaviour
    {
        private GameManager gameManager;
        public GameObject[] playerSkins;
        public TMP_Text displayTeam;
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
            displayTeam = GameObject.FindGameObjectWithTag("ChatLog").GetComponent<TMP_Text>();
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

            if (LocationProvider != null)
            {
                LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            }

        }
        private void MoleSelection(int teamNum)
        {
            if (teamNum == 0 && Random.Range(gameManager.englishTeam.Count, 5) == 5)
            {
                isMole = true;
                displayTeam.text = "English Mole";
            }
            else if (teamNum == 1 && Random.Range(gameManager.italianTeam.Count, 5) == 5)
            {
                isMole = true;
                displayTeam.text = "Italian Mole";
            }
            else if (teamNum == 2 && Random.Range(gameManager.russianTeam.Count, 5) == 5)
            {
                isMole = true;
                displayTeam.text = "Russian Mole";
            }
            else
            {
                isMole = false;
                displayTeam.text = team.ToString();
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




        /// <summary>
		/// Location property used for rotation: false=Heading (default), true=Orientation  
		/// </summary>
		[SerializeField]
        [Tooltip("Per default 'UserHeading' (direction the device is moving) is used for rotation. Check to use 'DeviceOrientation' (where the device is facing)")]
        bool _useDeviceOrientation;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        [Tooltip("Only evaluated when 'Use Device Orientation' is checked. Subtracts UserHeading from DeviceOrientation. Useful if map is rotated by UserHeading and DeviceOrientation should be displayed relative to the heading.")]
        bool _subtractUserHeading;

        /// <summary>
        /// The rate at which the transform's rotation tries catch up to the provided heading.  
        /// </summary>
        [SerializeField]
        [Tooltip("The rate at which the transform's rotation tries catch up to the provided heading. ")]
        float _rotationFollowFactor = 1;

        /// <summary>
        /// Set this to true if you'd like to adjust the rotation of a RectTransform (in a UI canvas) with the heading.
        /// </summary>
        [SerializeField]
        bool _rotateZ;

        /// <summary>
        /// <para>Set this to true if you'd like to adjust the sign of the rotation angle.</para>
        /// <para>eg angle passed in 63.5, angle that should be used for rotation: -63.5.</para>
        /// <para>This might be needed when rotating the map and not objects on the map.</para>
        /// </summary>
        [SerializeField]
        [Tooltip("Set this to true if you'd like to adjust the sign of the rotation angle. eg angle passed in 63.5, angle that should be used for rotation: -63.5.")]
        bool _useNegativeAngle;

        /// <summary>
        /// Use a mock <see cref="T:Mapbox.Unity.Location.TransformLocationProvider"/>,
        /// rather than a <see cref="T:Mapbox.Unity.Location.EditorLocationProvider"/>.   
        /// </summary>
        [SerializeField]
        bool _useTransformLocationProvider;

        Quaternion _targetRotation;

        

        Vector3 _targetPosition;




        private void Start()
        {
            LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
        }

        void LocationProvider_OnLocationUpdated(Location location)
        {

            float rotationAngle = _useDeviceOrientation ? location.DeviceOrientation : location.UserHeading;

            if (_useNegativeAngle) { rotationAngle *= -1f; }

            // 'Orientation' changes all the time, pass through immediately
            if (_useDeviceOrientation)
            {
                if (_subtractUserHeading)
                {
                    if (rotationAngle > location.UserHeading)
                    {
                        rotationAngle = 360 - (rotationAngle - location.UserHeading);
                    }
                    else
                    {
                        rotationAngle = location.UserHeading - rotationAngle + 360;
                    }

                    if (rotationAngle < 0) { rotationAngle += 360; }
                    if (rotationAngle >= 360) { rotationAngle -= 360; }
                }
                _targetRotation = Quaternion.Euler(getNewEulerAngles(rotationAngle));
            }
            else
            {
                // if rotating by 'Heading' only do it if heading has a new value
                if (location.IsUserHeadingUpdated)
                {
                    _targetRotation = Quaternion.Euler(getNewEulerAngles(rotationAngle));
                }
            }
        }
        private Vector3 getNewEulerAngles(float newAngle)
        {
            var localRotation = transform.localRotation;
            var currentEuler = localRotation.eulerAngles;
            var euler = Mapbox.Unity.Constants.Math.Vector3Zero;

            if (_rotateZ)
            {
                euler.z = -newAngle;

                euler.x = currentEuler.x;
                euler.y = currentEuler.y;
            }
            else
            {
                euler.y = -newAngle;

                euler.x = currentEuler.x;
                euler.z = currentEuler.z;
            }

            return euler;
        }

        private void LateUpdate()
        {
            if (!IsOwner) return;
            //if (_isInitialized)
            //{
            //    var map = LocationProviderFactory.Instance.mapManager;
            //    transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            //}
            var map = LocationProviderFactory.Instance.mapManager;
            transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, _targetRotation, Time.deltaTime * _rotationFollowFactor);
        }
    }
}