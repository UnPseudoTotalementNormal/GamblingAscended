using System;
using UnityEngine;

namespace Character.Enemy
{
    public class GL_BaseEnemy : MonoBehaviour, GL_IEnemy
    {
        [field:SerializeField] public float Damage { get; private set; }
        public GL_PathFollower PathFollower { get; private set; }

        private void Awake()
        {
            PathFollower = GetComponent<GL_PathFollower>();
        }
    }
}