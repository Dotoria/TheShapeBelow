using Battle;
using Character;
using UnityEngine;

namespace Environment
{
    public class PhaseController : MonoBehaviour
    {
        [SerializeField] private PhaseConfig _phaseConfig;
        [SerializeField] private BattleController _battleController;

        [SerializeField] private Transform[] _enemySpawnPoints;
        [SerializeField] private Transform[] _friendSpawnPoints;

        private int _currentPhase;
        private bool _isInitialized;
        
        public void Initialize()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
            StartPhase(0);
        }
        
        private void Update()
        {
            if (!_isInitialized)
                return;
            
            int aliveEnemyCount = BattleController.Enemies.Count;

            if (aliveEnemyCount < _phaseConfig.GetPhase(0).enemies[0].keepCount)
            {
                // SpawnEnemy();
            }
        }

        private void StartPhase(int phaseIndex)
        {
            _currentPhase = phaseIndex;

            PhaseConfig.PhaseData phase = _phaseConfig.GetPhase(phaseIndex);
            if (null == phase)
                return;

            SpawnEnemies(phase);
            SpawnFriends(phase);
        }

        private void SpawnEnemies(PhaseConfig.PhaseData phase)
        {
            foreach (EnemySpawnData data in phase.enemies)
            {
                // for (int i = 0; i < data.count; i++)
                // {
                //     Vector3 pos = GetRandomPosition(_enemySpawnPoints);
                //     _battleController.SpawnEnemy(data.enemyType, pos);
                // }
            }
        }

        private void SpawnFriends(PhaseConfig.PhaseData phase)
        {
            foreach (FriendSpawnData data in phase.friends)
            {
                // for (int i = 0; i < data.count; i++)
                // {
                //     Vector3 pos = GetRandomPosition(_friendSpawnPoints);
                //     Friend friend = _battleController.SpawnFriend(data.friendType, pos);
                // }
            }
        }

        private Vector3 GetRandomPosition(Transform[] points)
        {
            if (null == points || points.Length == 0)
                return Vector3.zero;

            int index = Random.Range(0, points.Length);
            return points[index].position;
        }

        public void NextPhase()
        {
            StartPhase(_currentPhase + 1);
        }
    }
}