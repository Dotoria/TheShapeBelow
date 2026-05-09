using System.Collections.Generic;
using Battle;

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
        
        private BattleStateMachineBase _stateMachine;

        public void ApplyConfig(CharacterData<Enemy, EEnemyType> config)
        {
            EnemyType = config.characterType;
            MaxHp = config.maxHp;
            MoveSpeed = config.moveSpeed;
            AttackPower = config.attackPower;
            AttackRange = config.attackRange;
            AttackCooldown = config.attackCooldown;
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
                _stateMachine = new EnemyStateMachine(this, _movement);
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
        
        public override IReadOnlyList<IBattleUnit> FindTarget()
        {
            IReadOnlyList<IBattleUnit> targets = base.FindTarget();

            if (targets.Count > 0)
                return targets;

            if (null == BattleController.Player || BattleController.Player.IsDead)
                return null;

            return new[] { BattleController.Player };
        }
        
        private void Update()
        {
            if (null == _stateMachine)
                return;
            
            _stateMachine.Update();
        }
    }
}