using UnityEngine;
using Util;

namespace Battle.AttackObject
{
    public abstract class AttackObjectBase : MonoBehaviour, IPoolable
    {
        public float Damage { get; protected set; }
        public float LifeTime { get; protected set; }

        public abstract void OnSpawned();
        public abstract void OnDespawned();
    }
}