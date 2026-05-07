using Battle;
using UnityEngine;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _smoothTime = 0.5f;
        [SerializeField] private float _turnSpeed = 720f;
        
        protected Movement _movement;

        public void Initialize()
        {
            _movement = new Movement(transform, _moveSpeed, _smoothTime, _turnSpeed);
            SetDefaultValues();
        }

        protected abstract void SetDefaultValues();
        public abstract void OnSpawned();
        public abstract void OnDespawned();
    }
}