using System;
using Character.Enemy;
using GameEvents;
using UnityEngine;

namespace BattleField
{
    public class GL_EnemyGoal : MonoBehaviour
    {
        [SerializeField] private GameEvent<GameEventInfo> _onDeathEvent;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out GL_BaseEnemy enemy))
            {
                
            }
        }
    }
}