using Battle;
using UnityEngine;

namespace Character
{
    public class Enemy : CharacterBase, IBattleUnit, IPoolable
    {
        public enum EEnemyType
        {
            Default,
        }

        public EEnemyType EnemyType { get; private set; }
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }

        public bool IsDead => CurrentHp <= 0f;
        
        private BattleUnitStateMachine _stateMachine;

        public void Initialize(CharacterData<Enemy, EEnemyType> config)
        {
            EnemyType = config.characterType;
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
            if (GetTarget(other, out var target))
            {
                Debug.Log("Enemy");
            }
        }

        protected override void OnExited(Collider other)
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
            
            _stateMachine?.Update();
        }
    }
}