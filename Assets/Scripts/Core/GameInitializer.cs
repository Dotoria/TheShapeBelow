using Battle;
using Environment;
using UnityEngine;
using Onboarding;
using UI;
using Util;

namespace Core
{
    public class GameInitializer : MonoBehaviour
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private BattleController _battleController;
        [SerializeField] private MapController _mapController;
        [SerializeField] private Arrow _arrowPrefab;

        private void Start()
        {
            _uiController.Initialize();
            _battleController.Initialize();
            _mapController.Initialize();
            
            OnboardingManager.Initialize(_arrowPrefab);
        }
    }
}
