using Battle;
using UnityEngine;
using Character;
using UI;
using UnityEngine.Serialization;
using Util;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private BattleController _battleController;

        private void Start()
        {
            _uiController.Initialize();
            _battleController.Initialize();
        }
    }
}
