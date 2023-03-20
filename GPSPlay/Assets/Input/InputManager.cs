using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : Singleton<InputManager>
    {
        #region Events
        public delegate void Touch(Vector2 position);
        public event Touch OnStartPrimaryTouch;
        public event Touch OnEndPrimaryTouch;
        public event Touch OnStartSecondaryTouch;
        public event Touch OnEndSecondaryTouch;

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
            touchControls.Touch.PrimaryFingerContact.started += ctx => StartTouchPrimary(ctx);
            touchControls.Touch.PrimaryFingerContact.canceled += ctx => EndTouchPrimary(ctx);

            touchControls.Touch.SecondaryFingerContact.started += ctx => StartTouchSecondary(ctx);
            touchControls.Touch.SecondaryFingerContact.canceled += ctx => EndTouchSecondary(ctx);
        }
        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            if (OnStartPrimaryTouch != null)
            {
                OnStartPrimaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.PrimaryFingerPosition.ReadValue<Vector2>()));
            }
        }
        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            if (OnEndPrimaryTouch != null)
            {
                OnEndPrimaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.PrimaryFingerPosition.ReadValue<Vector2>()));
            }
        }
        private void StartTouchSecondary(InputAction.CallbackContext context)
        {
            if (OnStartSecondaryTouch != null)
            {
                OnStartSecondaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.SecondaryFingerPosition.ReadValue<Vector2>()));
            }
        }
        private void EndTouchSecondary(InputAction.CallbackContext context)
        {
            if (OnEndSecondaryTouch != null)
            {
                OnEndSecondaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.SecondaryFingerPosition.ReadValue<Vector2>()));
            }
        }
        public Vector2 PrimaryPosition()
        {
            return Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.PrimaryFingerPosition.ReadValue<Vector2>());
        }
        public Vector2 SecondaryPosition()
        {
            return Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());
        }
        public Vector2 PrimaryDelta()
        {
            return Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.PrimaryFingerDelta.ReadValue<Vector2>());
        }
        public Vector2 SecondaryDelta()
        {
            return Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.SecondaryFingerDelta.ReadValue<Vector2>());
        }
    }
}