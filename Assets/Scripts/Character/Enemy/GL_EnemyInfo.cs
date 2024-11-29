using Enums;
using UnityEngine;

namespace Character.Enemy
{
    [CreateAssetMenu(fileName = "EnemyInfo", menuName = "Enemy/EnemyInfo")]
    public class GL_EnemyInfo : ScriptableObject
    {
        public GameObject Prefab;
        public string Name;
        public float Damage;
        public float Health;
        public float MoveSpeed;
        public float MoneyOnDeath;
        public DamageType DamageTypeImmunity = DamageType.None;
    }
}