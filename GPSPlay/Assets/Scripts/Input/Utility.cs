using UnityEngine;

namespace InputSystem
{
    public class Utility : MonoBehaviour
    {
        public static Vector3 ScreenToWorldPoint(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }
        public static bool ScreenTouch(bool touch)
        {
            return touch;
        }
    }
}