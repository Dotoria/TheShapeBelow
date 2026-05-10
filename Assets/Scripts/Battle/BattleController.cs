using System.Collections.Generic;
using Battle.AttackObject;
using Character;
using Onboarding;
using UnityEngine;

namespace Battle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [Header("Config")]
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private FriendConfig _friendConfig;
        [SerializeField] private ProjectileConfig _projectileConfig;

        private readonly List<Enemy> _activeEnemies = new List<Enemy>();
        private readonly List<Friend> _activeFriends = new List<Friend>();

        private static BattleController _instance;
        public static IBattleUnit Player => _instance._player;
        public static IReadOnlyList<Enemy> Enemies => _instance._activeEnemies;
        public static IReadOnlyList<Friend> Friends => _instance._activeFriends;

        public void Initialize()
        {
            _instance = this;
            _player.ApplyConfig(_playerConfig);
            _player.Initialize();
            _enemyConfig.InitializePools(transform);
            _friendConfig.InitializePools(transform);
            _projectileConfig.InitializePools(transform);
        }

        private void OnDestroy()
        {
            _enemyConfig.ClearRuntimePools();
            _friendConfig.ClearRuntimePools();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SpawnEnemy(Enemy.EEnemyType.Default, Vector3.zero);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                OnboardingManager.ShowArrow(_player.transform, _activeEnemies[0].transform);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                OnboardingManager.HideArrow();
            }
        }
#endif

        public Enemy SpawnEnemy(Enemy.EEnemyType enemyType, Vector3 position)
        {
            Enemy enemy = _enemyConfig.Spawn(enemyType, position);

            if (null == enemy)
                return null;

            _activeEnemies.Add(enemy);
            return enemy;
        }

        public Friend SpawnFriend(Friend.EFriendType friendType, Vector3 position)
        {
            Friend friend = _friendConfig.Spawn(friendType, position);

            if (null == friend)
                return null;

            _activeFriends.Add(friend);
            return friend;
        }
        
        public static Projectile SpawnProjectile(EProjectileType projectileType, Vector3 position)
        {
            Projectile projectile = _instance._projectileConfig.Spawn(projectileType, position);

            if (null == projectile)
                return null;

            return projectile;
        }

        public void Despawn(Enemy enemy)
        {
            if (enemy == null)
                return;

            _activeEnemies.Remove(enemy);
            _enemyConfig.Despawn(enemy, enemy.EnemyType);
        }

        public void Despawn(Friend friend)
        {
            if (friend == null)
                return;

            _activeFriends.Remove(friend);
            _friendConfig.Despawn(friend, friend.FriendType);
        }
    }
}