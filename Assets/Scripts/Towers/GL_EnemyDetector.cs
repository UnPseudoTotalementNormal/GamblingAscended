using System;
using System.Collections.Generic;
using System.Linq;
using Character.Enemy;
using Enums;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using NavmeshTools;
using UnityEngine;

namespace Towers
{
    public class GL_EnemyDetector : MonoBehaviour
    {
        public List<GL_BaseEnemy> EnemiesInRange { get; private set; } = new();
        private float DetectionRange;
        private void Awake()
        {
            GameEventEnum.OnTriggerEnter.AddListener(CheckEnemyInRange);
            GameEventEnum.OnTriggerExit.AddListener(CheckEnemyOutOfRange);
        }

        public void Init(float range)
        {
            DetectionRange = range;
            CreateTrigger();
        }

        private void CreateTrigger()
        {
            var triggerObject = new GameObject("Trigger");
            Transform triggerTransform = triggerObject.transform;
            triggerTransform.SetParent(transform);
            triggerTransform.localPosition = Vector3.zero;
            CapsuleCollider triggerCollider = triggerObject.AddComponent<CapsuleCollider>();
            triggerCollider.height = 20;
            triggerCollider.radius = DetectionRange;
            triggerCollider.isTrigger = true;
            triggerObject.layer = LayerMaskEnum.IgnoreRaycast.GetLayer();
            triggerObject.AddComponent<GL_TriggerHandler>();
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