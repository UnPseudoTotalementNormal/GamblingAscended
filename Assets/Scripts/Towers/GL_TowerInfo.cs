using UnityEngine;
using UnityEngine.Serialization;

namespace Towers
{
    [CreateAssetMenu(fileName = "TowerInfo", menuName = "TowerInfo")]
    public class GL_TowerInfo : ScriptableObject
    {
        public float AttackDamage;
        public float AttackRadius;
        public float AttackCooldown;

        public GameObject TowerModel;
    }
}