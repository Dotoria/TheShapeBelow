using Character;
using Onboarding;
using UI;
using UnityEngine;
using Util;

namespace Core
{
    public class GameInitializer : MonoBinder
    {
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private UIController _uiController;
        [SerializeField] private GameEventListener[] _gameEventListeners;
        [SerializeField] private Arrow _arrowPrefab;
        
        private void Start()
        {
            GameEventInvoker.OnEventInvoked += HandleEvent;
            OnboardingManager.Initialize(_arrowPrefab);
            
            _uiController.Initialize();
        }

        private void OnDisable()
        {
            GameEventInvoker.OnEventInvoked -= HandleEvent;
        }

        private void HandleEvent(EGameEvent gameEvent)
        {
            foreach (var listener in _gameEventListeners)
            {
                switch (gameEvent)
                {
                    case EGameEvent.Start: listener.InvokeEvent(gameEvent); break;
                    case EGameEvent.Over: listener.InvokeEvent(gameEvent); break;
                    case EGameEvent.PhaseStart: listener.InvokeEvent(gameEvent); break;
                    case EGameEvent.PhaseEnd: listener.InvokeEvent(gameEvent); break;
                }
            }
        }

#if UNITY_EDITOR
        public override void Bind()
        {
            _gameEventListeners = GetComponentsInChildren<GameEventListener>();
        }
#endif
    }
}
