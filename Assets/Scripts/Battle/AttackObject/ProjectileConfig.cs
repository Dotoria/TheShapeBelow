using UnityEngine;

namespace Battle.AttackObject
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "Configs/Projectile Config")]
    public class ProjectileConfig 
        : AttackObjectConfigBase<Projectile, EProjectileType, ProjectileData>
    {
        protected override void ApplyConfig(Projectile projectile, ProjectileData data)
        {
            projectile.ApplyConfig(data, DespawnProjectile);
        }

        private void DespawnProjectile(Projectile projectile)
        {
            Despawn(projectile, projectile.ProjectileType);
        }
    }
}