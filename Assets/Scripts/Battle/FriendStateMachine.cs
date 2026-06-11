using Character;
using UnityEngine;

namespace Battle
{
    public class FriendStateMachine : BattleStateMachineBase
    {
        public FriendStateMachine(IBattleUnit unit, Movement movement) : base(unit, movement)
        {
        }

        protected override void Idle()
        {
            _target = _unit.FindTarget();
            if (null != _target)
                ChangeState(EBattleUnitState.Move);
        }

        protected override void Move()
        {
            if (null == _target || _target.Count == 0 || _target[0].IsDead)
            {
                ChangeState(EBattleUnitState.Idle);
                return;
            }
            
            var direction = _target[0].Position - _unit.Position;
            var sqrDistance = direction.sqrMagnitude;
            
            _movement.Move(direction.normalized);
            
            if (sqrDistance <= _unit.AttackRange * _unit.AttackRange)
                ChangeState(EBattleUnitState.Attack);
        }

        protected override void Attack()
        {
            if (null == _target || _target.Count == 0 || _target[0].IsDead)
            {
                ChangeState(EBattleUnitState.Idle);
                return;
            }

            IBattleUnit target = _target[0];

            float sqrDistance = (target.Position - _unit.Position).sqrMagnitude;
            if (sqrDistance > _unit.AttackRange * _unit.AttackRange)
            {
                ChangeState(EBattleUnitState.Move);
                return;
            }

            if (Time.time < _lastAttackTime + _unit.AttackCooldown)
                return;

            _lastAttackTime = Time.time;

            _unit.Attack();
            target.TakeDamage(_unit.AttackPower);
        }

        protected override void Dead()
        {
        }
    }
}