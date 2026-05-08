using Util;

namespace Battle
{
    public interface IBattleUnit
    {
        float MaxHp { get; }
        float CurrentHp { get; }
        float MoveSpeed { get; }
        float AttackPower { get; }
        float AttackRange { get; }
        float AttackCooldown { get; }
        bool IsDead { get; }
        
        TriggerReceiver Trigger { get; }
        
        IBattleUnit FindTarget(IBattleUnit self);
    }
}