using System;
using GameEvents;
using UnityEngine;

public class GL_MoneyPurse : MonoBehaviour, GL_ICoinHolder
{
    public float MoneyInserted { get; private set; }

    private void Awake()
    {
        AddMoney(15);
    }
    
    public void AddMoney(float value)
    {
        MoneyInserted += value;
    }

    public void RemoveMoney(float value)
    {
        MoneyInserted -= value;
    }
}
