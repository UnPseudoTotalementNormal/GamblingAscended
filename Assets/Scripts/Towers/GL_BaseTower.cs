using System;
using System.Collections.Generic;
using Character.Enemy;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using Towers.Interface;
using UnityEngine;

namespace Towers
{
    public class GL_BaseTower : MonoBehaviour, GL_ITower
    {
        [field:SerializeField] public float AttackDamage { get; private set; }
        [field:SerializeField] public float AttackRange { get; private set;  }
        [field:SerializeField] public float AttackCooldown { get; private set; }

        private GameEventEnum _onAttackEvent;

        private List<GL_BaseEnemy> _enemiesInRange = new();

        private void Awake()
        {
            GameEventEnum.OnTriggerEnter.AddListener(CheckEnemyInRange);
            GameEventEnum.OnTriggerExit.AddListener(CheckEnemyOutOfRange);
        }


        private void CheckEnemyInRange(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventTriggerHandler triggerHandler))
            {
                return;
            }

            if (!triggerHandler.TriggerValue.gameObject.TryGetComponentInParents(out GL_BaseEnemy triggerEnemy) ||
                _enemiesInRange.Contains(triggerEnemy))
            {
                return;
            }
            
            _enemiesInRange.Add(triggerEnemy);
        }

        private void CheckEnemyOutOfRange(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventTriggerHandler triggerHandler))
            {
                return;
            }
            
            if (!triggerHandler.TriggerValue.gameObject.TryGetComponentInParents(out GL_BaseEnemy triggerEnemy) ||
                !_enemiesInRange.Contains(triggerEnemy))
            {
                return;
            }
            
            _enemiesInRange.Remove(triggerEnemy);
        }
        
        private void Update()
        {
            
        }
    }
}