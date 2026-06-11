using System;
using Character;

namespace Environment
{
    [Serializable]
    public class SpawnData
    {
        public float spawnDelay = 0f;
        public int totalCount = 1;
        public int keepCount = 1;
    }
    
    [Serializable]
    public class EnemySpawnData : SpawnData
    {
        public Enemy.EEnemyType enemyType;
    }

    [Serializable]
    public class FriendSpawnData
    {
        public Friend.EFriendType friendType;
    }
}