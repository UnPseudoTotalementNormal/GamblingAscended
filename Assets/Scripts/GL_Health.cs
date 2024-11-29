using System;
using System.Collections.Generic;
using GameEvents;
using GameEvents.Enum;
using UnityEngine;

public class GL_Health : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;
    
    private void Awake()
    {
        CurrentHealth = MaxHealth;
        
        GameEventEnum.TakeDamage.AddListener(OnTakeDamage);
        GameEventEnum.SetEnemyInfo.AddListener(SetEnemyInfo);
    }

    private void OnTakeDamage(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventDamage damageInfo))
        {
            return;
        }

        TakeDamage(damageInfo.Damage);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        GameEventEnum.OnDamageTaken.Invoke(new GameEventFloat
            { Ids = new[] { gameObject.GetGameID() }, Value = damage, Sender = gameObject });
        
        if (CurrentHealth > 0)
        {
            return;
        }
        
        CurrentHealth = 0;
        GameEventEnum.OnDeath.Invoke(new GameEventInfo 
            { Ids = new [] { gameObject.GetGameID() }, Sender = gameObject });
    }
    
    private void SetEnemyInfo(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventEnemyInfo gameEventEnemyInfo))
        {
            return;
        }

        MaxHealth = gameEventEnemyInfo.EnemyInfo.Health;
        CurrentHealth = MaxHealth;
    }
}