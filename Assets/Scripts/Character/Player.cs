using System.Collections.Generic;
using Battle;
using UnityEngine;
using Util;

namespace Character
{
    public class Player : CharacterBase, IBattleUnit
    {
        public Vector3 Position => transform.position;
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }
        public bool IsDead => CurrentHp <= 0f;
        
        protected override void SetDefaultValues()
        {
            
        }
        
        protected override void OnEntered(Collider other)
        {
            if (GetTarget(other, out var target))
            {
                Debug.Log("Player");
            }
        }
        
        protected override void OnExited(Collider other)
        {
        }
        
        public void OnSpawned()
        {
        }
        
        public void OnDespawned()
        {
        }
        
        public IReadOnlyList<IBattleUnit> FindTarget()
        {
            return null;
        }
        
        public void Attack()
        {
        }
        
        public void TakeDamage(float damage)
        {
            CurrentHp -= damage;
            if (IsDead)
            {
                Debug.Log("Player is Dead");
            }
        }
        
        private void Update()
        {
            _movement.Move(InputManager.Delta);
        }
    }
}