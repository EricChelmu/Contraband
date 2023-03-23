using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace InputSystem
{
    public class CameraControls : MonoBehaviour
    {
        private InputManager inputManager;
        private Camera mainCamera;
        private Vector3 spawnLocation;

        private Vector2 startLocation;
        private float startTime;

        private Vector2 currentLocation;
        private float currentTime;

        private Vector3 dir;
        private float speed;
        
        private void Awake()
        {
            inputManager = InputManager.Instance;
            mainCamera = GetComponent<Camera>();
            spawnLocation = transform.position;
        }
        private void OnEnable()
        {
            inputManager.OnPreformPrimaryFingerStart += MoveStart;
            inputManager.OnPerformPrimaryFingerPosition += MoveCamera;
        }
        
        private void OnDisable()
        {
            inputManager.OnPreformPrimaryFingerStart -= MoveStart;
            inputManager.OnPerformPrimaryFingerPosition -= MoveCamera;
        }
        private void MoveStart(Vector2 positionStart)
        {
            startLocation = positionStart;
        }
        private void MoveCamera(Vector2 screenPosition)
        {
            currentLocation = screenPosition;
            dir = new Vector3(currentLocation.x - startLocation.x, 0f,currentLocation.y - startLocation.y).normalized;
            speed = (currentLocation - startLocation).magnitude * Time.deltaTime;
            
            transform.Translate(-dir * speed, Space.World);
            MoveStart(currentLocation);
            
        }
    }
}

