using System;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class GL_Health : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;

    [SerializeField] private List<GameEvent<GameEventInfo>> _takeDamageEvents;
    
    [SerializeField] private GameEvent<GameEventInfo> _onTakeDamageEvent;
    [SerializeField] private GameEvent<GameEventInfo> _onDeathEvent;
    
    private void Awake()
    {
        CurrentHealth = MaxHealth;
        
        foreach (GameEvent<GameEventInfo> takeDamageEvent in _takeDamageEvents)
        {
            takeDamageEvent.AddListener(OnTakeDamage);
        }
    }

    private void OnTakeDamage(GameEventInfo obj)
    {
        if (!obj.TryTo(out GameEventDamage damageInfo))
        {
            return;
        }

        TakeDamage(damageInfo.Damage);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        _onTakeDamageEvent?.Invoke(new GameEventFloat { Value = CurrentHealth });
        
        if (CurrentHealth > 0)
        {
            return;
        }
        
        CurrentHealth = 0;
        _onDeathEvent?.Invoke(new GameEventInfo());
    }
}