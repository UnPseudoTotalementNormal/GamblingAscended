using System;
using System.Collections.Generic;
using Character.Enemy;
using Extensions;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleField
{
    public class GL_EnemyGoal : MonoBehaviour
    {
        [SerializeField] private GameEvent<GameEventInfo> _onDeathEvent;
        [SerializeField] private GameEvent<GameEventInfo> _takeDamageEvent;

        private void Awake()
        {
            GameEventEnum.OnDeath.AddListener(OnDeath);
        }

        private void OnDeath(GameEventInfo eventInfo)
        {
            if (gameObject.HasGameID(eventInfo.Ids))
            {
                _onDeathEvent?.Invoke(eventInfo);
                Invoke(nameof(LoadMenu), 1f);
            }
        }

        private void LoadMenu()
        {
            SceneManager.LoadScene(0);
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