using System;
using System.Collections.Generic;
using Character.Enemy;
using Enums;
using GameEvents;
using GameEvents.Enum;
using NavmeshTools;
using UnityEngine;

public class GL_Health : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;
    public bool IsInvincible = false;

    public bool IsDead = false;

    public DamageType DamageTypeImmunity = DamageType.Aucun;
    
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
        if (IsInvincible || IsDead)
        {
            return;
        }
        
        var damageResult = GL_DamageProcessor.GetFinalDamageAmount(damageInfo, DamageTypeImmunity);
        float damageAmount = damageResult.Amount;
        
        CurrentHealth -= damageAmount;
        
        GameEventEnum.OnDamageTaken.Invoke(new GameEventDamage
            { Ids = new[] { gameObject.GetGameID() }, Damage = damageAmount, DamageType = damageInfo.DamageType, Sender = gameObject });
        
        if (CurrentHealth > 0)
        {
            return;
        }
        
        CurrentHealth = 0;
        IsDead = true;
        GameEventEnum.OnDeath.Invoke(new GameEventInfo 
            { Ids = new [] { gameObject.GetGameID() }, Sender = gameObject });
    }
    
    private void SetEnemyInfo(GameEventInfo eventInfo)
    {
        if (!gameObject.HasGameID(eventInfo.Ids) || !eventInfo.TryTo(out GameEventEnemyInfo gameEventEnemyInfo))
        {
            return;
        }

        GL_EnemyInfo enemyInfo = gameEventEnemyInfo.EnemyInfo;
        MaxHealth = enemyInfo.Health;
        DamageTypeImmunity = enemyInfo.DamageTypeImmunity;
        
        CurrentHealth = MaxHealth;
    }
}