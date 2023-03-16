using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Vector3 ScreenToWorldPoint(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }
}
