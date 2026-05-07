using System;
using UnityEngine;

namespace Util
{
    public class TriggerReceiver : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        
        private bool _isActive = false;
        
        public event Action<Collider> OnTriggerEntered;
        public event Action<Collider> OnTriggerExited;
        
        public void Activate()
        {
            _isActive = true;
        }
        
        public void Deactivate()
        {
            _isActive = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!_isActive)
                return;
            
            OnTriggerEntered?.Invoke(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!_isActive)
                return;
            
            OnTriggerExited?.Invoke(other);
        }
    }
}