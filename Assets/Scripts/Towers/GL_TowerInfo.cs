using UnityEngine;
using UnityEngine.Serialization;

namespace Towers
{
    [CreateAssetMenu(fileName = "TowerObject", menuName = "TowerObject")]
    public class GL_TowerInfo : ScriptableObject
    {
        public float AttackDamage;
        [FormerlySerializedAs("AttackRange")] public float AttackRadius;
        public float AttackCooldown;
    }
}