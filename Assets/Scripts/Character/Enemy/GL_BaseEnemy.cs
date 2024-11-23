using UnityEngine;

namespace Character.Enemy
{
    public class GL_BaseEnemy : MonoBehaviour, GL_IEnemy
    {
        [field:SerializeField] public float Damage { get; private set; }
    }
}