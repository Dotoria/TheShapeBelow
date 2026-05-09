using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Battle
{
    public interface IBattleUnit
    {
        Vector3 Position { get; }
        float MaxHp { get; }
        float CurrentHp { get; }
        float MoveSpeed { get; }
        float AttackPower { get; }
        float AttackRange { get; }
        float AttackCooldown { get; }
        bool IsDead { get; }
        
        TriggerReceiver Trigger { get; }
        
        IReadOnlyList<IBattleUnit> FindTarget();
        void Attack();
        void TakeDamage(float damage);
    }
}