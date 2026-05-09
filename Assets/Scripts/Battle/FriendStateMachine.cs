using Character;

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
            var direction = _target[0].Position - _unit.Position;
            var sqrDistance = direction.sqrMagnitude;
            
            _movement.Move(direction.normalized);
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