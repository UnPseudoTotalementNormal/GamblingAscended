using Enums;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "TowerInfo", menuName = "TowerInfo")]
    public class GL_TowerInfo : ScriptableObject
    {
        public float AttackDamage;
        public float AttackRadius;
        public float AttackCooldown;
        public AttackType AttackType = AttackType.Raycast;
        public DamageType DamageType = DamageType.Distance;

        public GameObject TowerModel;
    }
}