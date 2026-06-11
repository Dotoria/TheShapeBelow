using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class GameEventListener : MonoBehaviour
    {
        [Serializable]
        private class EventEntry
        {
            public EGameEvent gameEvent;
            public UnityEvent onEvent;
        }

        [SerializeField] private EventEntry[] _events;

        public void InvokeEvent(EGameEvent gameEvent)
        {
            foreach (var entry in _events)
            {
                if (entry.gameEvent != gameEvent)
                    continue;

                entry.onEvent?.Invoke();
            }
        }
    }
}