using UnityEngine;

namespace Util
{
    public class CameraController
    {
        [SerializeField] private Camera _mainCamera;
        public Camera MainCamera => _mainCamera;
    }
}