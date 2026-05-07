using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
    public class EnemyConfig : CharacterConfigBase<Enemy, Enemy.EEnemyType>
    {
        protected override void ApplyConfig(
            Enemy enemy,
            CharacterData<Enemy, Enemy.EEnemyType> config
        )
        {
            enemy.Initialize(config);
        }
    }
}