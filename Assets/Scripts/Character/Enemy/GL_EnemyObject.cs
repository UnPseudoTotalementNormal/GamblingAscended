using UnityEngine;

namespace Character.Enemy
{
    [CreateAssetMenu(fileName = "EnemyObject", menuName = "Enemy/EnemyObject")]
    public class GL_EnemyObject : ScriptableObject
    {
        public GameObject Prefab;
        public string Name;
        public float Damage;
        public float Health;
        public float MoveSpeed;
        public float MoneyOnDeath;
    }
}