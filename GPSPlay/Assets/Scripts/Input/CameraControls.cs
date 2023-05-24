using Mapbox.Map;
using Mapbox.Unity.Location;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace InputSystem
{
    public class CameraControls : MonoBehaviour
    {
        //basics
        private InputManager inputManager;
        public Transform player;

        //follow player
        private float smoothTime = 0.25f;
        private Vector3 velocity = Vector3.zero;

        //finger movement
        private Vector2 startPosition;
        private Vector2 currentPosition;

        //camera movement
        private Vector3 direction;
        private float minimumDistance;
        private float speed;
        private float speedbuffer;

        private void Awake()
        {
            inputManager = InputManager.Instance;
        }
        private void OnEnable()
        {
            inputManager.OnPerformPrimaryFingerStart += MoveStart;
            inputManager.OnPerformPrimaryFingerPosition += MoveCamera;
        }
        
        private void OnDisable()
        {
            inputManager.OnPerformPrimaryFingerStart -= MoveStart;
            inputManager.OnPerformPrimaryFingerPosition -= MoveCamera;
        }
        private void MoveStart(Vector2 positionStart)
        {            
            startPosition = positionStart;
        }
        private void MoveCamera(Vector2 screenPosition)
        {
            currentPosition = screenPosition;
            direction = new Vector3(currentPosition.x - startPosition.x, 0f,currentPosition.y - startPosition.y).normalized;
            speed = (currentPosition - startPosition).magnitude * Time.deltaTime;
            
            transform.Translate(-direction * 2 * speed, Space.World);
            MoveStart(currentPosition);
        }
        private void FollowPlayer()
        {
            if (player != null && Input.touchCount == 0) 
            {
                Vector3 playerPosition = new Vector3(player.position.x, transform.position.y, player.position.z - 67.3f);
                transform.position = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothTime);
            }
        }

        private void Update()
        {
            FollowPlayer();
        }
    }
}

