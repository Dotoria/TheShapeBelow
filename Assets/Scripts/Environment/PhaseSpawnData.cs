using System;
using Character;

namespace Environment
{
    [Serializable]
    public class EnemySpawnData
    {
        public Enemy.EEnemyType enemyType;
        public int count = 1;
    }

    [Serializable]
    public class FriendSpawnData
    {
        public Friend.EFriendType friendType;
        public int count = 1;
    }
}