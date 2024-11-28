using System;
using Character.Enemy;
using GameEvents;
using GameEvents.Enum;
using Towers.Interface;
using UnityEngine;

namespace Towers
{
    public class GL_BaseTower : MonoBehaviour, GL_ITower
    {
        [field:SerializeField] public float AttackDamage { get; private set; }
        [field:SerializeField] public float AttackRadius { get; private set;  }
        [field:SerializeField] public float AttackCooldown { get; private set; }
        public GL_EnemyDetector EnemyDetector { get; private set; }
        private float _currentAttackCooldown;

        protected bool _canAttack = false;

        protected GameEventEnum _onAttackEvent;

        private void Awake()
        {
            GameEventEnum.SetTowerInfo.AddListener(SetTowerInfo);
            EnemyDetector = gameObject.AddComponent<GL_EnemyDetector>();
        }

        private void Start()
        {
            EnemyDetector.Init(AttackRadius);
        }

        private void SetTowerInfo(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventTowerInfo gameEventTowerInfo))
            {
                return;
            }
            
            AttackDamage = gameEventTowerInfo.TowerInfo.AttackDamage;
            AttackRadius = gameEventTowerInfo.TowerInfo.AttackRadius;
            AttackCooldown = gameEventTowerInfo.TowerInfo.AttackCooldown;
        }


        private void Update()
        {
            _currentAttackCooldown -= Time.deltaTime;
            TryAttack();
        }

        private void TryAttack()
        {
            if (_currentAttackCooldown >= 0 || EnemyDetector.EnemiesInRange.Count <= 0)
            {
                return;
            }

            _currentAttackCooldown = AttackCooldown;

            GL_BaseEnemy shootingEnemy = EnemyDetector.GetFirstEnemy();
            if (!shootingEnemy)
            {
                return;
            }
            
            Attack();
        }

        public void Attack()
        {
            GL_BaseEnemy shootingEnemy = EnemyDetector.GetFirstEnemy();
            GameEventDamage damageEvent = new GameEventDamage
            {
                Ids = new[] { shootingEnemy.gameObject.GetGameID() },
                Damage = AttackDamage,
                Sender = gameObject,
            };
            GameEventEnum.TakeDamage.Invoke(damageEvent);
        }
    }
}