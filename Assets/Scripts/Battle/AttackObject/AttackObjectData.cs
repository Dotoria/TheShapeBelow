using System;
using UnityEngine;

namespace Battle.AttackObject
{
    [Serializable]
    public class AttackObjectData<TAttackObject, TType>
        where TAttackObject : AttackObjectBase
        where TType : Enum
    {
        [Header("Identity")]
        public TType attackObjectType;
        public TAttackObject prefab;

        [Header("Common Stats")]
        public float damage = 1f;
        public float lifeTime = 3f;
        public int maxHitCount = 1;

        [Header("Pool")]
        public int initialPoolSize = 20;
    }
}