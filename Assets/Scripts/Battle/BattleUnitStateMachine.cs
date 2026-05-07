namespace Battle
{
    public class BattleUnitStateMachine
    {
        public enum EBattleUnitState
        {
            Idle,
            Move,
            Attack,
            Dead
        }
        
        private readonly IBattleUnit _unit;
        private EBattleUnitState _currentState;

        public EBattleUnitState CurrentState => _currentState;

        public BattleUnitStateMachine(IBattleUnit unit)
        {
            _unit = unit;
        }

        public void ChangeState(EBattleUnitState nextState)
        {
            if (_currentState == nextState)
                return;

            _currentState = nextState;
        }

        public void Update()
        {
            switch (_currentState)
            {
                case EBattleUnitState.Idle:
                    IBattleUnit target = _unit.FindTarget(_unit);
                    break;

                case EBattleUnitState.Move:
                    // 타겟 방향 이동
                    break;

                case EBattleUnitState.Attack:
                    // 공격 처리
                    break;

                case EBattleUnitState.Dead:
                    // 사망 처리
                    break;
            }
        }
    }
}