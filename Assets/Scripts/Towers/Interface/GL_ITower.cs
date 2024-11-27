namespace Towers.Interface
{
    public interface GL_ITower
    {
        public float AttackDamage { get; }
        public float AttackRadius { get; }
        public float AttackCooldown { get; }
        
        public GL_EnemyDetector EnemyDetector { get; }
    }
}