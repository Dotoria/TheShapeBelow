using Character;
using UnityEngine;

namespace Battle
{
    public class CharacterSpawner : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private FriendConfig _friendConfig;

        public void Initialize()
        {
            _enemyConfig.InitializePools(transform);
            _friendConfig.InitializePools(transform);
        }

        private void OnDestroy()
        {
            _enemyConfig.ClearRuntimePools();
            _friendConfig.ClearRuntimePools();
        }

        public Enemy SpawnEnemy(Enemy.EEnemyType enemyType, Vector3 position)
        {
            return _enemyConfig.Spawn(enemyType, position);
        }

        public Friend SpawnFriend(Friend.EFriendType friendType, Vector3 position)
        {
            return _friendConfig.Spawn(friendType, position);
        }

        public void DespawnEnemy(Enemy enemy)
        {
            _enemyConfig.Despawn(enemy, enemy.EnemyType);
        }

        public void DespawnFriend(Friend friend)
        {
            _friendConfig.Despawn(friend, friend.FriendType);
        }
    }
}