using UnityEngine;
using Util;

namespace Character
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _smoothTime = 0.5f;
        [SerializeField] private float _turnSpeed = 720f;
        
        private Movement _movement;

        public void Initialize()
        {
            _movement = new Movement(transform, _moveSpeed, _smoothTime, _turnSpeed);
        }
        
        private void Update()
        {
            _movement.Move(InputManager.Delta);
        }
    }
}