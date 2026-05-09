using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Stats")]
        public float maxHp = 10f;
        public float moveSpeed = 3f;
        public float attackPower = 1f;
        public float attackRange = 1f;
        public float attackCooldown = 1f;
    }
}