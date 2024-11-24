using System;
using System.Collections.Generic;
using Character.Enemy;
using Extensions;
using GameEvents;
using UnityEngine;

namespace BattleField
{
    public class GL_EnemyGoal : MonoBehaviour
    {
        [SerializeField] private GameEvent<GameEventInfo> _onDeathEvent;
        [SerializeField] private GameEvent<GameEventInfo> _takeDamageEvent;

        private void Awake()
        {
            _onDeathEvent.AddListener(OnDeath);
        }

        private void OnDeath(GameEventInfo eventInfo)
        {
            Debug.Log("u lose");
            Debug.Break();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.TryGetComponentInParents(out GL_BaseEnemy enemy))
            {
                return;
            }
            
            _takeDamageEvent?.Invoke(new GameEventDamage
            {
                Ids = new[] { gameObject.GetGameID() },
                Damage = enemy.Damage,
            }); 
            Destroy(enemy.gameObject);
        }
    }
}