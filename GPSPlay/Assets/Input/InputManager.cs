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
            touchControls.Touch.PrimaryTouch.started += ctx => StartTouchPrimary(ctx);
            touchControls.Touch.PrimaryTouch.canceled += ctx => EndTouchPrimary(ctx);

            touchControls.Touch.SecondaryTouch.started += ctx => StartTouchSecondary(ctx);
            touchControls.Touch.SecondaryTouch.canceled += ctx => EndTouchSecondary(ctx);
        }
        private void StartTouchPrimary(InputAction.CallbackContext context)
        {
            if (OnStartPrimaryTouch != null)
            {
                OnStartPrimaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.PrimaryTouch.ReadValue<Vector2>()));
            }
        }
        private void EndTouchPrimary(InputAction.CallbackContext context)
        {
            if (OnEndPrimaryTouch != null)
            {
                OnEndPrimaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.PrimaryTouch.ReadValue<Vector2>()));
            }
        }
        private void StartTouchSecondary(InputAction.CallbackContext context)
        {
            if (OnStartSecondaryTouch != null)
            {
                OnStartSecondaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.SecondaryTouch.ReadValue<Vector2>()));
            }
        }
        private void EndTouchSecondary(InputAction.CallbackContext context)
        {
            if (OnEndSecondaryTouch != null)
            {
                OnEndSecondaryTouch(Utility.ScreenToWorldPoint(mainCamera, touchControls.Touch.SecondaryTouch.ReadValue<Vector2>()));
            }
        }
    }
}