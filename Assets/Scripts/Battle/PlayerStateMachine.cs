using Character;

namespace Battle
{
    public class PlayerStateMachine : BattleStateMachineBase
    {
        public PlayerStateMachine(IBattleUnit unit, Movement movement) : base(unit, movement)
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
            _movement.Move(_target[0].Position);
            var sqrDistance = (_target[0].Position - _unit.Position).sqrMagnitude;
            if (sqrDistance <= _unit.AttackRange * _unit.AttackRange)
                ChangeState(EBattleUnitState.Attack);
        }

        protected override void Attack()
        {
        }

        protected override void Dead()
        {

        }
    }
}