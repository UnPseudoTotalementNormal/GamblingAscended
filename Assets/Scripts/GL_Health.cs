using System;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class GL_Health : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;

    [SerializeField] private List<GameEvent<GameEventInfo>> _damageEvents;
    
    [SerializeField] private GameEvent<GameEventInfo> _onDeathEvent;
    
    private void Awake()
    {
        CurrentHealth = MaxHealth;
        
    }
    
    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            
        }
    }
}