using System;
using GameEvents;
using Towers.Interface;
using UnityEngine;
using UnityEngine.Serialization;

namespace Towers
{
    public class GL_BaseTower : MonoBehaviour, GL_ITower
    {
        [field:SerializeField] public float AttackDamage { get; private set; }
        [field:SerializeField] public float AttackRange { get; private set;  }
        [field:SerializeField] public float AttackCooldown { get; private set; }
        public GL_EnemyDetector EnemyDetector { get; private set; }

        protected bool _canAttack = false;

        protected GameEventEnum _onAttackEvent;

        private void Awake()
        {
            EnemyDetector = gameObject.AddComponent<GL_EnemyDetector>();
            EnemyDetector.Init(AttackRange);
        }


        private void Update()
        {
            if (EnemyDetector.EnemiesInRange.Count <= 0)
            {
                return;
            }
        }
    }
}