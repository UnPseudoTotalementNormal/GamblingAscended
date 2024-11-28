using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = "TowerObject", menuName = "TowerObject")]
    public class GL_TowerObject : ScriptableObject
    {
        public float AttackDamage;
        public float AttackRange;
        public float AttackCooldown;
    }
}