using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

namespace InputSystem
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : Singleton<InputManager>
    {
        #region Events
        public delegate void StartPosition(Vector2 position);
        public delegate void CurrentPosition(Vector2 position);
        public delegate void Touch(bool touch);

        public event StartPosition OnPreformPrimaryFingerStart;
        public event CurrentPosition OnStartPrimaryFingerPosition;
        public event CurrentPosition OnPerformPrimaryFingerPosition;
        #endregion

        private TouchControls touchControls;
        private Camera mainCamera;

        private void Awake()
        {
            touchControls = new TouchControls();
            mainCamera = Camera.main;
        }
        private void OnEnable()
        {
            touchControls.Enable();
        }
        private void OnDisable()
        {
            touchControls.Disable();
        }
        private void Start()
        {
            touchControls.Touch.PrimaryFingerStart.performed += ctx => PreformPrimaryFingerStart(ctx);
            touchControls.Touch.PrimaryFingerPosition.started += ctx => StartPrimaryFingerPosition(ctx);
            touchControls.Touch.PrimaryFingerPosition.performed += ctx => PerformPrimaryFingerPosition(ctx);
        }
        private void PreformPrimaryFingerStart(InputAction.CallbackContext context)
        {
            if(OnPreformPrimaryFingerStart != null)
            {
                OnPreformPrimaryFingerStart(touchControls.Touch.PrimaryFingerStart.ReadValue<Vector2>());
            }
        }        
        private void StartPrimaryFingerPosition(InputAction.CallbackContext context)
        {
            if(OnStartPrimaryFingerPosition != null) 
            {
                OnStartPrimaryFingerPosition(touchControls.Touch.PrimaryFingerPosition.ReadValue<Vector2>());
            }
        }
        private void PerformPrimaryFingerPosition(InputAction.CallbackContext context)
        {
            if (OnPerformPrimaryFingerPosition != null)
            {
                OnPerformPrimaryFingerPosition(touchControls.Touch.PrimaryFingerPosition.ReadValue<Vector2>());                
            }
        }  
    }
}