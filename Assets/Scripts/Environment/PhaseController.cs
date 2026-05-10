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

        public void StartPhase(int phaseIndex)
        {
            _currentPhase = phaseIndex;

            PhaseConfig.PhaseData phase = _phaseConfig.GetPhase(phaseIndex);
            if (phase == null)
                return;

            SpawnEnemies(phase);
            SpawnFriends(phase);
        }

        private void SpawnEnemies(PhaseConfig.PhaseData phase)
        {
            foreach (EnemySpawnData data in phase.enemies)
            {
                for (int i = 0; i < data.count; i++)
                {
                    Vector3 pos = GetRandomPosition(_enemySpawnPoints);
                    _battleController.SpawnEnemy(data.enemyType, pos);
                }
            }
        }

        private void SpawnFriends(PhaseConfig.PhaseData phase)
        {
            foreach (FriendSpawnData data in phase.friends)
            {
                for (int i = 0; i < data.count; i++)
                {
                    Vector3 pos = GetRandomPosition(_friendSpawnPoints);
                    Friend friend = _battleController.SpawnFriend(data.friendType, pos);
                }
            }
        }

        private Vector3 GetRandomPosition(Transform[] points)
        {
            if (points == null || points.Length == 0)
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