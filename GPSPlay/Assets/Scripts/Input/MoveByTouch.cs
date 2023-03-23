using UnityEngine;

namespace InputSystem
{
    public class MoveByTouch : MonoBehaviour
    {
        public int fingerID;

        // Update is called once per frame
        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(fingerID);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = 0f;
                transform.position = touchPosition;
            }
        }
    }
}