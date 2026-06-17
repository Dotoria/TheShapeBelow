using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class CharacterData<TCharacter, TType>
        where TCharacter : CharacterBase
        where TType : Enum
    {
        [Header("Identity")]
        public TType characterType;
        public TCharacter prefab;
        public Color color = Color.white;

        [Header("Stats")]
        public float maxHp = 10f;
        public float moveSpeed = 3f;
        public float attackPower = 1f;
        public float attackRange = 1f;
        public float attackCooldown = 1f;

        [Header("Pool")]
        public int initialPoolSize = 10;
    }
}