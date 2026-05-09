using System.Collections.Generic;
using Battle;
using UnityEngine;

namespace Character
{
    public class Friend : CharacterBase, IBattleUnit, IPoolable
    {
        public enum EFriendType
        {
            Default,
            Healer,
            Dealer,
            Tanker
        }

        public EFriendType FriendType { get; private set; }
        public Vector3 Position => transform.position;
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }
        public bool IsDead => CurrentHp <= 0f;
        
        private BattleStateMachineBase _stateMachineBase;

        public void Initialize(CharacterData<Friend, EFriendType> config)
        {
            FriendType = config.characterType;
            MaxHp = config.maxHp;
            MoveSpeed = config.moveSpeed;
            AttackPower = config.attackPower;
            AttackRange = config.attackRange;
            AttackCooldown = config.attackCooldown;

            Initialize();
        }

        protected override void SetDefaultValues()
        {
        }
        
        protected override void OnEntered(Collider other)
        {
        }
        
        protected override void OnExited(Collider other)
        {
        }

        public void OnSpawned()
        {
            CurrentHp = MaxHp;
            gameObject.SetActive(true);
            
            if (null == _stateMachineBase)
                _stateMachineBase = new FriendStateMachine(this, _movement);
            _stateMachineBase.ChangeState(BattleStateMachineBase.EBattleUnitState.Idle);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
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
            if (CurrentHp <= 0f)
            {
                CurrentHp = 0f;
                OnDespawned();
            }
        }
        
        private void Update()
        {
            if (null == _stateMachineBase)
                return;
            
            _stateMachineBase.Update();
        }
    }
}