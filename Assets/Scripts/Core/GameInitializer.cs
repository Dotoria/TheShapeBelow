using UnityEngine;
using Character;
using UI;
using Util;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private Player _player;

        private void Start()
        {
        }
    }
}
