using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Battle
{
    public abstract class BattleStateMachineBase
    {
        public enum EBattleUnitState
        {
            Idle,
            Move,
            Attack,
            Dead
        }
        
        protected readonly IBattleUnit _unit;
        protected readonly Movement _movement;
        protected IReadOnlyList<IBattleUnit> _target;
        protected float _lastAttackTime;
        private EBattleUnitState _currentState;

        protected BattleStateMachineBase(IBattleUnit unit, Movement movement)
        {
            _unit = unit;
            _movement = movement;
        }

        public void ChangeState(EBattleUnitState nextState)
        {
            _currentState = nextState;
        }

        public void Update()
        {
            switch (_currentState)
            {
                case EBattleUnitState.Idle:
                    Idle();
                    break;

                case EBattleUnitState.Move:
                    Move();
                    break;

                case EBattleUnitState.Attack:
                    Attack();
                    break;

                case EBattleUnitState.Dead:
                    Dead();
                    break;
            }
        }

        protected abstract void Idle();
        protected abstract void Move();
        protected abstract void Attack();
        protected abstract void Dead();
    }
}