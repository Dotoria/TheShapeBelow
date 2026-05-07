using Battle;
using UnityEngine;
using Util;

namespace Character
{
    public class Player : CharacterBase, IBattleUnit
    {
        public float MaxHp { get; }
        public float CurrentHp { get; }
        public float MoveSpeed { get; }
        public float AttackPower { get; }
        public float AttackRange { get; }
        public float AttackCooldown { get; }
        public bool IsDead { get; }
        
        protected override void SetDefaultValues()
        {
        }
        
        public void OnSpawned()
        {
        }
        
        public void OnDespawned()
        {
        }
        
        public IBattleUnit FindTarget(IBattleUnit self)
        {
            return null;
        }
        
        private void Update()
        {
            _movement.Move(InputManager.Delta);
        }
    }
}