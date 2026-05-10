using Battle;
using Battle.AttackObject;
using Core;
using UnityEngine;
using Util;

namespace Character
{
    public class Player : CharacterBase, IBattleUnit
    {
        [SerializeField] private EProjectileType _attackObjectType;
        
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }
        public bool IsDead => CurrentHp <= 0f;
        
        private BattleStateMachineBase _stateMachine;
        
        public void ApplyConfig(PlayerConfig config)
        {
            MaxHp = config.maxHp;
            MoveSpeed = config.moveSpeed;
            AttackPower = config.attackPower;
            AttackRange = config.attackRange;
            AttackCooldown = config.attackCooldown;
        }
        
        protected override void SetDefaultValues()
        {
            CurrentHp = MaxHp;
            
            if (null == _stateMachine)
                _stateMachine = new PlayerStateMachine(this, _movement);

            _stateMachine.ChangeState(BattleStateMachineBase.EBattleUnitState.Idle);
        }
        
        public void FireProjectile()
        {
            var projectile = BattleController.SpawnProjectile(_attackObjectType, Position);
            projectile.Fire(transform.up, GameConfig.EnemyLayer);
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