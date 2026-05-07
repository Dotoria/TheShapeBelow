using UnityEngine;
using Util;

namespace Character
{
    public class Player : CharacterBase
    {
        protected override void SetDefaultValues()
        {
        }
        
        public override void OnSpawned()
        {
        }
        
        public override void OnDespawned()
        {
        }
        
        private void Update()
        {
            _movement.Move(InputManager.Delta);
        }
    }
}