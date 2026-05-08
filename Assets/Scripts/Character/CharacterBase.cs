using Battle;
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
        
        private int AllyLayer;
        private int EnemyLayer;
        
        protected Movement _movement;
        public TriggerReceiver Trigger { get; private set; }

        public void Initialize()
        {
            AllyLayer = LayerMask.NameToLayer("Ally");
            EnemyLayer = LayerMask.NameToLayer("Opponent");
            _movement = new Movement(transform, _moveSpeed, _smoothTime, _turnSpeed);
            Trigger = _trigger;
            Trigger.Activate();
            Trigger.OnTriggerEntered += OnEntered;
            Trigger.OnTriggerExited += OnExited;
            SetDefaultValues();
        }

        protected abstract void SetDefaultValues();
        protected abstract void OnEntered(Collider other);
        protected abstract void OnExited(Collider other);

        protected bool GetTarget(Collider other, out IBattleUnit target)
        {
            target = null;
            if (null == other)
                return false;

            var rb = other.attachedRigidbody;

            if (gameObject.layer == AllyLayer)
                return rb.gameObject.layer == EnemyLayer;

            if (gameObject.layer == EnemyLayer)
                return rb.gameObject.layer == AllyLayer;

            if (rb != null && rb.TryGetComponent(out target))
                return true;
            
            return false;
        }
    }
}