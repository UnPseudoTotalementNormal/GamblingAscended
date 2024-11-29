using System;
using System.Collections.Generic;
using Enums;
using GameEvents;
using GameEvents.Enum;
using NavmeshTools;
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

        TakeDamage(new GL_DamageInfo { Amount = damageInfo.Damage, DamageType = damageInfo.DamageType });
    }

    public void TakeDamage(GL_DamageInfo damageInfo)
    {
        var damageResult = GL_DamageProcessor.GetFinalDamageAmount(damageInfo, DamageType.None);
        float damageAmount = damageResult.Amount;
        
        CurrentHealth -= damageAmount;
        
        GameEventEnum.OnDamageTaken.Invoke(new GameEventFloat
            { Ids = new[] { gameObject.GetGameID() }, Value = damageAmount, Sender = gameObject });
        
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