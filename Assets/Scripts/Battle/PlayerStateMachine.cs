using Character;
using Environment;
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
            TryMove(Vector2.zero);
            TryFireProjectile();
            
            if (InputManager.Delta.sqrMagnitude >= INPUT_THRESHOLD)
            {
                ChangeState(EBattleUnitState.Move);
                return;
            }

            _target = _unit.FindTarget();

            if (null != _target && _target.Count > 0)
                ChangeState(EBattleUnitState.Attack);
        }

        protected override void Move()
        {
            TryMove(InputManager.Delta);
            TryFireProjectile();
            
            if (InputManager.Delta.sqrMagnitude < INPUT_THRESHOLD)
            {
                ChangeState(EBattleUnitState.Idle);
                return;
            }

            _target = _unit.FindTarget();
            if (null != _target && _target.Count > 0)
            {
                var sqrDistance = (_target[0].Position - _unit.Position).sqrMagnitude;
                if (sqrDistance <= _unit.AttackRange * _unit.AttackRange)
                    ChangeState(EBattleUnitState.Attack);
            }
        }

        protected override void Attack()
        {
            TryMove(InputManager.Delta);

            _target = _unit.FindTarget();
            if (null == _target || _target.Count == 0 || _target[0].IsDead)
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
            _movement.Stop();
        }

        private void TryMove(Vector2 input)
        {
            bool hasInput = input.sqrMagnitude >= INPUT_THRESHOLD;
            bool isMoving = _movement.IsMoving;

            if (!hasInput && !isMoving)
            {
                _movement.Move(Vector2.zero);
                MapController.MoveBackgroundStatic(Vector2.zero, false);
                return;
            }

            Vector2 inputDirection = hasInput
                ? input.normalized
                : Vector2.zero;

            Vector2 allowedDirection = inputDirection;
            bool isBlockedByBoundary = false;

            if (hasInput)
            {
                allowedDirection = MapController.GetAllowedMoveDirection(
                    _unit.Position,
                    inputDirection,
                    out isBlockedByBoundary
                );
            }

            _movement.Move(allowedDirection);
            MapController.MoveBackgroundStatic(
                inputDirection,
                isBlockedByBoundary
            );
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