using Battle;
using UnityEngine;

namespace Character
{
    public class Enemy : CharacterBase, IBattleUnit, IPoolable
    {
        public enum EEnemyType
        {
            Default,
        }

        public EEnemyType EnemyType { get; private set; }
        public float MaxHp { get; private set; }
        public float CurrentHp { get; private set; }
        public float MoveSpeed { get; private set; }
        public float AttackPower { get; private set; }
        public float AttackRange { get; private set; }
        public float AttackCooldown { get; private set; }

        public bool IsDead => CurrentHp <= 0f;

        public void Initialize(CharacterData<Enemy, EEnemyType> config)
        {
            EnemyType = config.characterType;
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