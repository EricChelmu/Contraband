using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputSystem
{
    public class CameraFind : MonoBehaviour
    {
        private GameObject _camera;
        private CameraControls _controls;
        private void Awake()
        {
            _camera = GameObject.FindGameObjectWithTag("MainCamera");
            _controls = _camera.GetComponent<CameraControls>();
            _controls.player = this.transform;
        }
    }
}
