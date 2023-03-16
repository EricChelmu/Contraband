using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    [SerializeField] private float minDistance;
    [SerializeField] private float maxTime;

    private InputManager inputManager;
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    private void Awake()
    {
        inputManager = InputManager.Instance;
    }
    private void OnEnable()
    {
        inputManager.OnStartPrimaryTouch += DragStart;
        inputManager.OnEndPrimaryTouch += DragEnd;
    }
    private void OnDisable()
    {
        inputManager.OnStartPrimaryTouch -= DragStart;
        inputManager.OnEndPrimaryTouch -= DragEnd;
    }
    private void DragStart(Vector2 position)
    {
        startPosition = position;
    }
    private void DragEnd(Vector2 position)
    {
        endPosition = position;
        DetectDrag(); 
    }
    private void DetectDrag()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minDistance) 
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            //Camera.main.transform.position += direction;
            DragDirection(direction2D);
        }
    }
    private void DragDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > 0.9f) 
        {
            Debug.Log("Drag Up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > 0.9f)
        {
            Debug.Log("Drag Down");
        }
        else if (Vector2.Dot(Vector2.left, direction) > 0.9f)
        {
            Debug.Log("Drag Left");
        }
        else if (Vector2.Dot(Vector2.right, direction) > 0.9f)
        {
            Debug.Log("Drag Right");
        }
    }
}
