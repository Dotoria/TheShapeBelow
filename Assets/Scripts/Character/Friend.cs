using Battle;
using UnityEngine;
using Util;

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
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }
        public bool IsDead => CurrentHp <= 0f;
        
        private BattleStateMachineBase _stateMachine;

        public void ApplyConfig(CharacterData<Friend, EFriendType> config)
        {
            FriendType = config.characterType;
            MaxHp = config.maxHp;
            MoveSpeed = config.moveSpeed;
            AttackPower = config.attackPower;
            AttackRange = config.attackRange;
            AttackCooldown = config.attackCooldown;
            SetColor(config.color);
        }

        protected override void SetDefaultValues()
        {
        }

        public void OnSpawned()
        {
            Initialize();
            CurrentHp = MaxHp;
            gameObject.SetActive(true);
            
            if (null == _stateMachine)
                _stateMachine = new FriendStateMachine(this, _movement);
            _stateMachine.ChangeState(BattleStateMachineBase.EBattleUnitState.Idle);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
        
        public void Attack()
        {
        }
        
        public void TakeDamage(float damage)
        {
            CurrentHp -= damage;
        }
        
        private void Update()
        {
            if (null == _stateMachine)
                return;
            
            _stateMachine.Update();
        }
    }
}