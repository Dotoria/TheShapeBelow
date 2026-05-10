using System;
using Core;
using UnityEngine;
using Util;

namespace Battle.AttackObject
{
    public enum EProjectileType
    {
        Default,
    }
    
    public class Projectile : AttackObjectBase
    {
        [SerializeField] private TriggerReceiver _trigger;
        
        public EProjectileType ProjectileType { get; private set; }
        private float _speed;
        private Vector3 _direction;
        private Action<Projectile> _onDespawnRequested;
        
        private float _elapsedTime;
        private int _remainingHitCount;
        private int _targetLayer;
        private bool _isDespawnRequested;

        public void ApplyConfig(ProjectileData data, Action<Projectile> onDespawnRequested)
        {
            ProjectileType = data.attackObjectType;
            Damage = data.damage;
            LifeTime = data.lifeTime;
            _remainingHitCount = data.maxHitCount;
            _speed = data.speed;
            _onDespawnRequested = onDespawnRequested;
            _trigger.Activate();
        }

        public void Fire(Vector3 direction, int targetLayer)
        {
            _direction = direction.normalized;
            _targetLayer = targetLayer;
            _elapsedTime = 0f;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        private void Update()
        {
            transform.position += _direction * (_speed * Time.deltaTime);

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= LifeTime)
                RequestDespawn();
        }
        
        private void RequestDespawn()
        {
            if (_isDespawnRequested)
                return;

            _isDespawnRequested = true;

            var callback = _onDespawnRequested;
            _onDespawnRequested = null;
            callback?.Invoke(this);
        }
        
        public override void OnSpawned()
        {
            gameObject.SetActive(true);

            _trigger.OnTriggerEntered -= OnEntered;
            _trigger.OnTriggerExited -= OnExited;

            _trigger.OnTriggerEntered += OnEntered;
            _trigger.OnTriggerExited += OnExited;
        }

        public override void OnDespawned()
        {
            _trigger.OnTriggerEntered -= OnEntered;
            _trigger.OnTriggerExited -= OnExited;

            _direction = Vector3.zero;
            _elapsedTime = 0f;
            _isDespawnRequested = false;
            _onDespawnRequested = null;

            gameObject.SetActive(false);
        }
        
        private void OnEntered(Collider other)
        {
            if (!GetTarget(other, out var target))
                return;
            
            target.TakeDamage(Damage);
            _remainingHitCount--;
            if (_remainingHitCount <= 0)
                RequestDespawn();
        }

        private void OnExited(Collider other)
        {
        }

        private bool GetTarget(Collider other, out IBattleUnit target)
        {
            target = null;
            if (null == other)
                return false;

            var rb = other.attachedRigidbody;
            if (null == rb || !rb.TryGetComponent(out target))
                return false;

            if (rb.gameObject.layer == _targetLayer)
                return true;
            
            return false;
        }
    }
}