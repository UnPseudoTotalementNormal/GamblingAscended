using System.Collections.Generic;
using System.Linq;
using Character.Enemy;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

namespace Towers
{
    public class GL_EnemyDetector : MonoBehaviour
    {
        public List<GL_BaseEnemy> EnemiesInRange { get; private set; } = new();

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
                EnemiesInRange.Contains(triggerEnemy))
            {
                return;
            }
            
            EnemiesInRange.Add(triggerEnemy);
        }

        private void CheckEnemyOutOfRange(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventTriggerHandler triggerHandler))
            {
                return;
            }
            
            if (!triggerHandler.TriggerValue.gameObject.TryGetComponentInParents(out GL_BaseEnemy triggerEnemy) ||
                !EnemiesInRange.Contains(triggerEnemy))
            {
                return;
            }
            
            EnemiesInRange.Remove(triggerEnemy);
        }

        public GL_BaseEnemy GetFirstEnemy()
        {
            return EnemiesInRange.OrderByDescending(e => e.PathFollower.CurrentDistance).FirstOrDefault();
        }
        
        public GL_BaseEnemy GetLastEnemy()
        {
            return EnemiesInRange.OrderBy(e => e.PathFollower.CurrentDistance).FirstOrDefault();
        }
    }
}