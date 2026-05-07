using Battle;
using UnityEngine;

namespace Character
{
    public class Friend : CharacterBase, IBattleUnit, IPoolable
    {
        public enum EFriendType
        {
            Default,
            Healer,
            Dealer,
            Tanker
        }

        [SerializeField] private EFriendType _friendType;
        public EFriendType FriendType => _friendType;

        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }

        public bool IsDead => CurrentHp <= 0f;

        public void Initialize(CharacterData<Friend, EFriendType> config)
        {
            _friendType = config.characterType;

            MaxHp = config.maxHp;
            MoveSpeed = config.moveSpeed;
            AttackPower = config.attackPower;
            AttackRange = config.attackRange;
            AttackCooldown = config.attackCooldown;

            SetDefaultValues();
        }

        protected override void SetDefaultValues()
        {
        }

        public override void OnSpawned()
        {
            CurrentHp = MaxHp;
            gameObject.SetActive(true);
        }

        public override void OnDespawned()
        {
            gameObject.SetActive(false);
        }
    }
}