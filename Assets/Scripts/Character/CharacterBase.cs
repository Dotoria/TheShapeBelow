using System.Collections.Generic;
using Battle;
using Core;
using UnityEngine;
using Util;

namespace Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        
        [Header("Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _smoothTime = 0.5f;
        [SerializeField] private float _turnSpeed = 720f;
        [SerializeField] private TriggerReceiver _trigger;
        
        private bool _initialized = false;
        
        protected Movement _movement;
        private MaterialPropertyBlock _propertyBlock;
        private List<IBattleUnit> _targetsInRange = new List<IBattleUnit>();
        public Vector3 Position => transform.position;
        
        private const string COLOR_PROPERTY = "_MainColor";
        private const string CORRUPTION_PROPERTY = "_Corruption";
        private const float COLOR_OFFSET = 0.25f;

        public void Initialize()
        {
            if (_initialized)
                return;
            
            _initialized = true;
            _movement = new Movement(transform, _moveSpeed, _smoothTime, _turnSpeed);
            _propertyBlock = new MaterialPropertyBlock();
            ShapeGenerator.GenerateShape(_meshFilter);
            
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

        protected void SetColor(Color color)
        {
            Color randomColor = new Color(
                Mathf.Clamp01(color.r + Random.Range(-COLOR_OFFSET, COLOR_OFFSET)),
                Mathf.Clamp01(color.g + Random.Range(-COLOR_OFFSET, COLOR_OFFSET)),
                Mathf.Clamp01(color.b + Random.Range(-COLOR_OFFSET, COLOR_OFFSET)),
                color.a
            );

            _meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(COLOR_PROPERTY, randomColor);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }
        
        protected void SetCorruption(float value)
        {
            _meshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(CORRUPTION_PROPERTY, value);
            _meshRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}