using System.Collections.Generic;
using Battle;
using Core;
using UnityEngine;
using Util;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _smoothTime = 0.5f;
        [SerializeField] private float _turnSpeed = 720f;
        [SerializeField] private TriggerReceiver _trigger;
        
        private bool _initialized = false;
        
        protected Movement _movement;
        private List<IBattleUnit> _targetsInRange = new List<IBattleUnit>();
        public Vector3 Position => transform.position;

        public void Initialize()
        {
            if (_initialized)
                return;
            
            _initialized = true;
            _movement = new Movement(transform, _moveSpeed, _smoothTime, _turnSpeed);
            
            _trigger.Activate();
            _trigger.OnTriggerEntered += OnEntered;
            _trigger.OnTriggerExited += OnExited;
            
            SetDefaultValues();
        }

        protected abstract void SetDefaultValues();

        private void OnEntered(Collider other)
        {
            if (!GetTarget(other, out var target))
                return;
            
            if (!_targetsInRange.Contains(target))
                _targetsInRange.Add(target);
        }

        private void OnExited(Collider other)
        {
            if (!GetTarget(other, out var target))
                return;

            _targetsInRange.Remove(target);
        }

        private bool GetTarget(Collider other, out IBattleUnit target)
        {
            target = null;
            if (null == other)
                return false;

            var rb = other.attachedRigidbody;
            if (null == rb || !rb.TryGetComponent(out target))
                return false;

            if (gameObject.layer == GameConfig.AllyLayer)
                return rb.gameObject.layer == GameConfig.EnemyLayer;

            if (gameObject.layer == GameConfig.EnemyLayer)
                return rb.gameObject.layer == GameConfig.AllyLayer;
            
            return false;
        }

        public virtual IReadOnlyList<IBattleUnit> FindTarget()
        {
            _targetsInRange.RemoveAll(t => t == null || t.IsDead);

            _targetsInRange.Sort((a, b) =>
            {
                float da = (a.Position - Position).sqrMagnitude;
                float db = (b.Position - Position).sqrMagnitude;
                return da.CompareTo(db);
            });

            return _targetsInRange;
        }
    }
}