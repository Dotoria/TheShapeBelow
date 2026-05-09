using Character;
using UnityEngine;

namespace Battle
{
    public class EnemyStateMachine : BattleStateMachineBase
    {
        public EnemyStateMachine(IBattleUnit unit, Movement movement) : base(unit, movement)
        {
        }
        
        protected override void Idle()
        {
            _target = _unit.FindTarget();

            if (_target == null || _target.Count == 0)
                return;

            ChangeState(EBattleUnitState.Move);
        }

        protected override void Move()
        {
            if (_target == null || _target.Count == 0 || _target[0].IsDead)
            {
                ChangeState(EBattleUnitState.Idle);
                return;
            }

            IBattleUnit target = _target[0];

            Vector3 direction = target.Position - _unit.Position;
            float sqrDistance = direction.sqrMagnitude;

            _movement.Move(direction.normalized);

            if (sqrDistance <= _unit.AttackRange * _unit.AttackRange)
                ChangeState(EBattleUnitState.Attack);
        }
        
        protected override void Attack()
        {
            if (_target == null || _target.Count == 0 || _target[0].IsDead)
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