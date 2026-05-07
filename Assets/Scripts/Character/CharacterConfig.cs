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

        public IReadOnlyList<CharacterData<TCharacter, TType>> Entries => _entries;

        public void Initialize()
        {
            BuildConfigMap();
        }

        public void InitializePools(Transform parent = null)
        {
            BuildConfigMap();

            _poolMap = new Dictionary<TType, ObjectPool<TCharacter>>();
            _poolRoot = new GameObject($"{typeof(TCharacter).Name}PoolRoot");
            
            if (parent != null)
                _poolRoot.transform.SetParent(parent);

            foreach (var entry in _entries)
            {
                if (entry == null)
                    continue;

                if (entry.prefab == null)
                {
                    Debug.LogWarning($"Prefab is null: {entry.characterType}");
                    continue;
                }

                if (_poolMap.ContainsKey(entry.characterType))
                {
                    Debug.LogWarning($"Duplicate pool type: {entry.characterType}");
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
            if (_poolMap == null)
            {
                Debug.LogError("Pools are not initialized. Call InitializePools() first.");
                return null;
            }

            if (!_poolMap.TryGetValue(characterType, out ObjectPool<TCharacter> pool))
            {
                Debug.LogError($"Pool not found: {characterType}");
                return null;
            }

            CharacterData<TCharacter, TType> config = Get(characterType);
            if (config == null)
                return null;

            TCharacter character = pool.Get();
            character.transform.position = position;

            ApplyConfig(character, config);
            character.OnSpawned();

            return character;
        }

        public void Despawn(TCharacter character, TType characterType)
        {
            if (character == null)
                return;

            if (_poolMap == null)
            {
                Debug.LogError("Pools are not initialized.");
                Destroy(character.gameObject);
                return;
            }

            if (!_poolMap.TryGetValue(characterType, out ObjectPool<TCharacter> pool))
            {
                Debug.LogError($"Pool not found: {characterType}");
                Destroy(character.gameObject);
                return;
            }

            character.OnDespawned();
            pool.Release(character);
        }

        public CharacterData<TCharacter, TType> Get(TType characterType)
        {
            if (_configMap == null)
                BuildConfigMap();

            if (_configMap.TryGetValue(characterType, out var entry))
                return entry;

            Debug.LogError($"Config not found: {characterType}");
            return null;
        }

        public void ClearRuntimePools()
        {
            _poolMap = null;

            if (_poolRoot != null)
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
                if (entry == null)
                    continue;

                if (_configMap.ContainsKey(entry.characterType))
                {
                    Debug.LogWarning($"Duplicate config type: {entry.characterType}");
                    continue;
                }

                _configMap.Add(entry.characterType, entry);
            }
        }

        protected abstract void ApplyConfig(
            TCharacter character,
            CharacterData<TCharacter, TType> config
        );
    }
}