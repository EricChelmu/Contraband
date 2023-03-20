using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField]private float cameraSpeed;

    private InputManager inputManager;
    private Coroutine zoomCoroutine;
    private new Camera camera;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        camera = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        inputManager.OnStartSecondaryTouch += ZoomStart;
        inputManager.OnEndSecondaryTouch += ZoomEnd;
    }
    private void OnDisable()
    {
        inputManager.OnStartSecondaryTouch -= ZoomStart;
        inputManager.OnEndSecondaryTouch -= ZoomEnd;
    }
    private void ZoomStart(Vector2 position)
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    private void ZoomEnd(Vector2 position)
    {
        StopCoroutine(zoomCoroutine);
    }
    IEnumerator ZoomDetection()
    {
        float prevDistance = 0f, distance = 0f;
        while (true)
        {
            distance = Vector2.Distance(inputManager.PrimaryPosition(), inputManager.SecondaryPosition());
            if (Vector2.Dot(inputManager.PrimaryDelta(), inputManager.SecondaryDelta()) <= -0.9f)
            {
                if (distance > prevDistance)
                {
                    Vector2 targetPosition = camera.transform.position;
                    //camera.transform.position = Vector3.Slerp(camera.transform.position);
                }
                else if (distance < prevDistance)
                {

                }
            }            
            prevDistance = distance;
        }
    }
}
