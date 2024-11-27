namespace Character.Enemy
{
    public interface GL_IEnemy
    {
        public float Damage { get; }
        public GL_PathFollower PathFollower { get; }
    }
}