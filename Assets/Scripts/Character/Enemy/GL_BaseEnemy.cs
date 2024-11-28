using System;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

namespace Character.Enemy
{
    public class GL_BaseEnemy : MonoBehaviour, GL_IEnemy
    {
        public float Damage { get; private set; }
        public GL_PathFollower PathFollower { get; private set; }

        private void Awake()
        {
            PathFollower = GetComponent<GL_PathFollower>();
            GameEventEnum.SetEnemyInfo.AddListener(SetEnemyInfo);
        }

        private void SetEnemyInfo(GameEventInfo eventInfo)
        {
            if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventEnemyInfo gameEventEnemyInfo))
            {
                return;
            }

            Damage = gameEventEnemyInfo.EnemyInfo.Damage;
        }
    }
}