using System;
using GameEvents;
using Towers.Interface;
using UnityEngine;

namespace Towers
{
    public class GL_BaseTower : MonoBehaviour, GL_ITower
    {
        [field:SerializeField] public float AttackDamage { get; private set; }
        [field:SerializeField] public float AttackRange { get; private set;  }
        [field:SerializeField] public float AttackCooldown { get; private set; }
        public GL_EnemyDetector EnemyDetector { get; private set; }

        private GameEventEnum _onAttackEvent;

        private void Awake()
        {
            EnemyDetector = GetComponent<GL_EnemyDetector>();
        }


        private void Update()
        {
            if (EnemyDetector.EnemiesInRange.Count <= 0)
            {
                return;
            }
            
            Debug.Log(EnemyDetector.GetFirstEnemy().name);
        }
    }
}