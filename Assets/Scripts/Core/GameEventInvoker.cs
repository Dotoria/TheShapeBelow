using System;
using UnityEngine;

namespace Core
{
    public class GameEventInvoker : MonoBehaviour
    {
        public static event Action<EGameEvent> OnEventInvoked;

        [SerializeField] private EGameEvent _gameEvent;
        
        public void InvokeEvent()
        {
            OnEventInvoked?.Invoke(_gameEvent);
        }
    }
}