using Character;

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
            if (null != _target)
                ChangeState(EBattleUnitState.Move);
        }

        protected override void Move()
        {
        }
        
        protected override void Attack()
        {
        }
        
        protected override void Dead()
        {
        }
    }
}