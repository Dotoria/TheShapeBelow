using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Character
{
    public abstract class CharacterConfigBase<TCharacter, TType> : ScriptableObject
        where TCharacter : CharacterBase
        where TType : Enum
    {
        [SerializeField]
        private List<CharacterData<TCharacter, TType>> _entries;

        private Dictionary<TType, CharacterData<TCharacter, TType>> _configMap;
        private Dictionary<TType, ObjectPool<TCharacter>> _poolMap;
        private GameObject _poolRoot;

        public void InitializePools(Transform parent = null)
        {
            BuildConfigMap();

            _poolMap = new Dictionary<TType, ObjectPool<TCharacter>>();
            _poolRoot = new GameObject($"{typeof(TCharacter).Name}Pool");
            
            if (null != parent)
                _poolRoot.transform.SetParent(parent);

            foreach (var entry in _entries)
            {
                if (null == entry)
                    continue;

                if (null == entry.prefab)
                {
                    continue;
                }

                if (_poolMap.ContainsKey(entry.characterType))
                {
                    continue;
                }

                var pool = new ObjectPool<TCharacter>(
                    entry.prefab,
                    entry.initialPoolSize,
                    _poolRoot.transform
                );

                _poolMap.Add(entry.characterType, pool);
            }
        }

        public TCharacter Spawn(TType characterType, Vector3 position)
        {
            if (null == _poolMap)
                return null;

            if (!_poolMap.TryGetValue(characterType, out ObjectPool<TCharacter> pool))
                return null;

            CharacterData<TCharacter, TType> config = Get(characterType);
            if (null == config)
                return null;

            TCharacter character = pool.Get();
            character.transform.position = position;

            if (character is IPoolable poolable)
                poolable.OnSpawned();
            
            ApplyConfig(character, config);
            return character;
        }

        public void Despawn(TCharacter character, TType characterType)
        {
            if (null == character)
                return;

            if (null == _poolMap)
            {
                Destroy(character.gameObject);
                return;
            }

            if (!_poolMap.TryGetValue(characterType, out ObjectPool<TCharacter> pool))
            {
                Destroy(character.gameObject);
                return;
            }

            if (character is IPoolable poolable)
                poolable.OnDespawned();
            
            pool.Release(character);
        }

        public CharacterData<TCharacter, TType> Get(TType characterType)
        {
            if (null == _configMap)
                BuildConfigMap();

            return _configMap.GetValueOrDefault(characterType);
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
            _configMap = new Dictionary<TType, CharacterData<TCharacter, TType>>();

            foreach (var entry in _entries)
            {
                if (null == entry)
                    continue;

                if (!_configMap.TryAdd(entry.characterType, entry))
                {
                    continue;
                }
            }
        }

        protected abstract void ApplyConfig(TCharacter character, CharacterData<TCharacter, TType> config);
    }
}