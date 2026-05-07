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
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }

        public bool IsDead => CurrentHp <= 0f;
        private BattleUnitStateMachine _stateMachine;

        public void Initialize(CharacterData<Friend, EFriendType> config)
        {
            FriendType = config.characterType;
            MaxHp = config.maxHp;
            MoveSpeed = config.moveSpeed;
            AttackPower = config.attackPower;
            AttackRange = config.attackRange;
            AttackCooldown = config.attackCooldown;

            SetDefaultValues();
        }

        protected override void SetDefaultValues()
        {
        }

        public void OnSpawned()
        {
            CurrentHp = MaxHp;
            gameObject.SetActive(true);
            
            if (null == _stateMachine)
                _stateMachine = new BattleUnitStateMachine(this);
            _stateMachine.ChangeState(BattleUnitStateMachine.EBattleUnitState.Idle);
        }

        public void OnDespawned()
        {
            gameObject.SetActive(false);
        }
        
        public IBattleUnit FindTarget(IBattleUnit self)
        {
            return null;
        }
        
        private void Update()
        {
            if (null == _stateMachine)
                return;
            
            _stateMachine.Update();
        }
    }
}