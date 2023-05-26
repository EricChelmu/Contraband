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
        public delegate void CurrentPosition(Vector2 position);
        public delegate void Contact(Touch touch);

        public event CurrentPosition OnPerformPrimaryFingerStart;
        public event CurrentPosition OnPerformPrimaryFingerPosition;
        public event CurrentPosition OnEndPrimaryFingerPosition;

        public event Contact OnPerformPrimaryFingerTouch;
        #endregion

        private TouchControls touchControls;

        private void Awake()
        {
            touchControls = new TouchControls();
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
            touchControls.Touch.PrimaryFingerStart.performed += ctx => PerformPrimaryFingerStart(ctx);
            touchControls.Touch.PrimaryFingerPosition.performed += ctx => PerformPrimaryFingerPosition(ctx);
            touchControls.Touch.PrimaryFingerPosition.canceled += ctx => EndPrimaryFingerPosition(ctx);
        }
        private void PerformPrimaryFingerStart(InputAction.CallbackContext context)
        {
            if(OnPerformPrimaryFingerStart != null)
            {
                OnPerformPrimaryFingerStart(touchControls.Touch.PrimaryFingerStart.ReadValue<Vector2>());
            }
        }
        private void PerformPrimaryFingerPosition(InputAction.CallbackContext context)
        {
            if(OnPerformPrimaryFingerPosition != null)
            {
                OnPerformPrimaryFingerPosition(touchControls.Touch.PrimaryFingerPosition.ReadValue<Vector2>());  
            }
        }
        private void EndPrimaryFingerPosition(InputAction.CallbackContext context)
        {
            if(OnEndPrimaryFingerPosition != null)
            {
                OnEndPrimaryFingerPosition(touchControls.Touch.PrimaryFingerPosition.ReadValue<Vector2>());
            }
        }
    }
}