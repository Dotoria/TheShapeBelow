using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Battle.AttackObject
{
    public abstract class AttackObjectConfigBase<TAttackObject, TType, TData> : ScriptableObject
        where TAttackObject : AttackObjectBase
        where TType : Enum
        where TData : AttackObjectData<TAttackObject, TType>
    {
        [SerializeField] private List<TData> _entries;

        private Dictionary<TType, TData> _configMap;
        private Dictionary<TType, ObjectPool<TAttackObject>> _poolMap;
        private GameObject _poolRoot;

        public void InitializePools(Transform parent = null)
        {
            BuildConfigMap();

            _poolMap = new Dictionary<TType, ObjectPool<TAttackObject>>();
            _poolRoot = new GameObject($"{typeof(TAttackObject).Name}Pool");

            if (null != parent)
                _poolRoot.transform.SetParent(parent);

            foreach (TData entry in _entries)
            {
                if (null == entry || null == entry.prefab)
                    continue;

                if (_poolMap.ContainsKey(entry.attackObjectType))
                    continue;

                var pool = new ObjectPool<TAttackObject>(
                    entry.prefab,
                    entry.initialPoolSize,
                    _poolRoot.transform
                );

                _poolMap.Add(entry.attackObjectType, pool);
            }
        }

        public TAttackObject Spawn(TType type, Vector3 position)
        {
            if (null == _poolMap)
                return null;

            if (!_poolMap.TryGetValue(type, out ObjectPool<TAttackObject> pool))
                return null;

            TData data = Get(type);
            if (data == null)
                return null;

            TAttackObject obj = pool.Get();
            obj.transform.position = position;

            ApplyConfig(obj, data);
            obj.OnSpawned();

            return obj;
        }

        public void Despawn(TAttackObject obj, TType type)
        {
            if (null == obj)
                return;

            if (null == _poolMap || !_poolMap.TryGetValue(type, out ObjectPool<TAttackObject> pool))
            {
                Destroy(obj.gameObject);
                return;
            }

            obj.OnDespawned();
            pool.Release(obj);
        }

        public TData Get(TType type)
        {
            if (null == _configMap)
                BuildConfigMap();

            return _configMap.TryGetValue(type, out TData data)
                ? data
                : null;
        }

        public void ClearRuntimePools()
        {
            _poolMap = null;

            if (null != _poolRoot)
            {
                Destroy(_poolRoot);
                _poolRoot = null;
            }
        }

        private void BuildConfigMap()
        {
            _configMap = new Dictionary<TType, TData>();

            foreach (TData entry in _entries)
            {
                if (null == entry)
                    continue;

                if (_configMap.ContainsKey(entry.attackObjectType))
                    continue;

                _configMap.Add(entry.attackObjectType, entry);
            }
        }

        protected abstract void ApplyConfig(TAttackObject obj, TData data);
    }
}