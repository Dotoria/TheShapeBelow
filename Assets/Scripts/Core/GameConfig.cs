using UnityEngine;

namespace Core
{
    public static class GameConfig
    {
        public static readonly int AllyLayer = LayerMask.NameToLayer("Ally");
        public static readonly int EnemyLayer = LayerMask.NameToLayer("Opponent");
    }
}