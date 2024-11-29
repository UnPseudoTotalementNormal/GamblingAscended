using System;
using Character.Enemy;
using Enums;
using GameEvents;
using GameEvents.Enum;
using Towers.Interface;
using UnityEngine;

namespace Towers
{
    public class GL_BaseTower : MonoBehaviour, GL_ITower
    {
        public float AttackDamage { get; private set; }
        public float AttackRadius { get; private set;  }
        public float AttackCooldown { get; private set; }
        public AttackType AttackType { get; private set; }
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

            GL_TowerInfo towerInfo = gameEventTowerInfo.TowerInfo;
            AttackDamage = towerInfo.AttackDamage;
            AttackRadius = towerInfo.AttackRadius;
            AttackCooldown = towerInfo.AttackCooldown;
            AttackType = towerInfo.AttackType;
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
            if (!shootingEnemy) //if no enemy in range
            {
                return;
            }

            switch (AttackType)
            {
                case AttackType.Raycast:
                    AttackRaycastType();
                    break;
                case AttackType.Zone:
                    AttackZoneType();
                    break;
                case AttackType.Projectile:
                    AttackProjectileType();
                    break;
            }
        }

        public void AttackRaycastType()
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
        
        public void AttackZoneType()
        {
            var enemiesId = new int[EnemyDetector.EnemiesInRange.Count];
            for (int i = 0; i < EnemyDetector.EnemiesInRange.Count; i++)
            {
                enemiesId[i] = EnemyDetector.EnemiesInRange[i].gameObject.GetGameID();
            }
            
            GameEventDamage damageEvent = new GameEventDamage
            {
                Ids = enemiesId,
                Damage = AttackDamage,
                Sender = gameObject,
            };
            GameEventEnum.TakeDamage.Invoke(damageEvent);
        }

        public void AttackProjectileType()
        {
            
        }
    }
}