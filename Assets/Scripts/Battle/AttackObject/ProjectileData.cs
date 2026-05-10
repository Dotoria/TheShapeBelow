using System;
using UnityEngine;

namespace Battle.AttackObject
{
    [Serializable]
    public class ProjectileData 
        : AttackObjectData<Projectile, EProjectileType>
    {
        [Header("Projectile")]
        public float speed = 10f;
    }
}