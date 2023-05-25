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
        //private Vector3 spawnLocation;

        private Vector2 startPosition;
        private Vector2 endPosition;
        //private float startTime;

        private Vector2 currentPosition;
        //private float currentTime;

        private Vector3 direction;
        private float minimumDistance;
        private float speed;
        private float speedbuffer;
        
        private void Awake()
        {
            inputManager = InputManager.Instance;
            mainCamera = GetComponent<Camera>();
            //spawnLocation = transform.position;
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
            //SlideCamera();
            MoveStart(currentPosition);            
        }
        private void SlideCamera()
        {
            minimumDistance = .2f;
                for(float i = speed; i > 0;)
                {
                    transform.Translate(-direction * i, Space.World);
                    i /= 1/speed;
                }
            
        }
    }
}

