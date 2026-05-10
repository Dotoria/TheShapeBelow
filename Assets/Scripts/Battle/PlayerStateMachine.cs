using Character;
using UnityEngine;
using Util;

namespace Battle
{
    public class PlayerStateMachine : BattleStateMachineBase
    {
        private float _lastProjectileTime;
        private const float PROJECTILE_COOLDOWN = 1.0f;
        private const float INPUT_THRESHOLD = 0.001f;
        
        public PlayerStateMachine(IBattleUnit unit, Movement movement) : base(unit, movement)
        {
        }

        protected override void Idle()
        {
            if (_movement.IsMoving)
                _movement.Move(Vector2.zero);
            TryFireProjectile();
            
            if (InputManager.Delta.sqrMagnitude >= INPUT_THRESHOLD)
            {
                ChangeState(EBattleUnitState.Move);
                return;
            }

            _target = _unit.FindTarget();

            if (_target != null && _target.Count > 0)
                ChangeState(EBattleUnitState.Attack);
        }

        protected override void Move()
        {
            _movement.Move(InputManager.Delta);
            TryFireProjectile();
            
            if (InputManager.Delta.sqrMagnitude < INPUT_THRESHOLD)
            {
                ChangeState(EBattleUnitState.Idle);
                return;
            }

            _target = _unit.FindTarget();
            if (_target != null && _target.Count > 0)
            {
                var sqrDistance = (_target[0].Position - _unit.Position).sqrMagnitude;
                if (sqrDistance <= _unit.AttackRange * _unit.AttackRange)
                    ChangeState(EBattleUnitState.Attack);
            }
        }

        protected override void Attack()
        {
            _movement.Move(InputManager.Delta);

            _target = _unit.FindTarget();
            if (_target == null || _target.Count == 0 || _target[0].IsDead)
            {
                ChangeState(EBattleUnitState.Idle);
                return;
            }

            IBattleUnit target = _target[0];

            float sqrDistance = (target.Position - _unit.Position).sqrMagnitude;
            if (sqrDistance > _unit.AttackRange * _unit.AttackRange)
            {
                ChangeState(EBattleUnitState.Idle);
                return;
            }

            TryAttack(target);
            TryFireProjectile();
        }

        protected override void Dead()
        {
        }
        
        private void TryAttack(IBattleUnit target)
        {
            if (Time.time < _lastAttackTime + _unit.AttackCooldown)
                return;

            _lastAttackTime = Time.time;

            _unit.Attack();
            target.TakeDamage(_unit.AttackPower);
        }
        
        private void TryFireProjectile()
        {
            if (Time.time < _lastProjectileTime + PROJECTILE_COOLDOWN)
                return;

            if (_unit is not Player player)
                return;

            _lastProjectileTime = Time.time;

            player.FireProjectile();
        }
    }
}