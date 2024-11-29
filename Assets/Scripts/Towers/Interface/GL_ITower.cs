using Enums;

namespace Towers.Interface
{
    public interface GL_ITower
    {
        public float AttackDamage { get; }
        public float AttackRadius { get; }
        public float AttackCooldown { get; }
        public AttackType AttackType { get; }
        
        public GL_EnemyDetector EnemyDetector { get; }

        public void AttackRaycastType();
        public void AttackZoneType();
        public void AttackProjectileType();
    }
}